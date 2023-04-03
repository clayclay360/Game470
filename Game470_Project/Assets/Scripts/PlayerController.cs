using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float spiritTime; //The ammount of time the player can spend in spirit form
    public float spiritCooldown; //The ammount of time the player has to wait to reenter their spirit form after leaving it
    public float spiritReturnDistance; //The distance from which the player can manually reenter their body
    public bool isInSpiritForm; //This should be false by default
    public float interactionRange;
    public bool isCaptured = false;
    public bool isHiding = false;
    [Header("Components")]
    public GameObject playerBody;
    public GameObject playerSpirit; 
    public GameObject form;
    public GameObject holdPoint;
    public GameObject heldObject;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualBodyCamera, virtualSpiritCamera;
    public Rig holdObjectRig;
    public Animator animator; // get animator
    public Text displayText;
    [HideInInspector]public CinemachineVirtualCamera virtualMainCamera;


    private CinemachineBrain cinemachineBrain;
    private CameraController bodyCameraController, spiritCameraController;
    private PostProcessingScript postProcessingScript;
    private float spiritTimer = 0; //The ammount of time the player has spent in spirit form
    private float bodyTimer = 0; //The ammount of time the player has spent in their body
    private int spiritFormCounter = 0; //The number of times the player has gone into spirit form
    private bool canEnterSpiritForm = true;
    private bool isHoldingObject;
    private string interactionText;

    public Vector2 rot = Vector2.zero;
    public Vector3 moveVal;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        form = playerBody;
        virtualMainCamera = virtualBodyCamera;
        mainCamera.GetComponent<Camera>(); // get camera
        cinemachineBrain = FindObjectOfType<CinemachineBrain>(); // get cinemachine brain
        postProcessingScript = FindObjectOfType<PostProcessingScript>(); // get post procress script
        bodyCameraController = virtualBodyCamera.GetComponent<CameraController>(); // git camera controller
        spiritCameraController = virtualSpiritCamera.GetComponent<CameraController>(); // get camera controller
        holdObjectRig.GetComponent<Rig>(); // get rig
        animator.GetComponent<Animator>();
        displayText.GetComponent<Text>();
        GameManager.canPlayer.interact = true; // player can interact
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured && GameManager.canPlayer.controlCamera)
        {
            Movement();
            DisplayInteraction();
        }
    }

    public void Movement()
    {
        #region Movement
        // Player Movement
        Vector3 forwardMovement = Vector3.zero;
        Vector3 rightMovement = Vector3.zero;
        float moveHorizontal = moveVal.x;
        float moveVertical = moveVal.z;

        animator.SetFloat("Walk", moveVertical);

        if (moveVertical != 0f)
        {
            forwardMovement = new Vector3(virtualMainCamera.transform.forward.x, 0, virtualMainCamera.transform.forward.z);
        }
        forwardMovement.Normalize();
        form.transform.position += forwardMovement * Time.deltaTime * speed * moveVertical;

        //move left and right
        if (moveHorizontal != 0f)
        {
            rightMovement = new Vector3(virtualMainCamera.transform.right.x, 0, virtualMainCamera.transform.right.z);
        }
        rightMovement.Normalize();
        form.transform.position += rightMovement * Time.deltaTime * speed * moveHorizontal;

        float rotY= virtualMainCamera.transform.eulerAngles.y;
        playerBody.transform.eulerAngles = new Vector3(playerBody.transform.eulerAngles.x, rotY, playerBody.transform.eulerAngles.z);

        //Spirit moves with body when not in spirit form
        if (!isInSpiritForm)
        {
            playerSpirit.transform.position = playerBody.transform.position;
            virtualBodyCamera.transform.position = playerBody.transform.position + new Vector3(0, 0.2f, 0);
        }
        #endregion
        #region Timers
        if (isInSpiritForm)
        {
            spiritTimer += Time.deltaTime;
            if (spiritTimer >= spiritTime)
            {
                SwitchForm();
            }
        }
        if (!isInSpiritForm)
        {
            bodyTimer += Time.deltaTime;
            if (bodyTimer < spiritCooldown && spiritFormCounter > 0)
            {
                canEnterSpiritForm = false;
            }
            else
            {
                canEnterSpiritForm = true;
            }
        }
        #endregion

        if (!isInSpiritForm)
        {
            spiritCameraController.moveHorizontal = bodyCameraController.moveHorizontal;
            spiritCameraController.moveVertical = bodyCameraController.moveVertical;
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveVal = new Vector3(input.x, 0f, input.y);
    }

    public void OnSwitchForm(InputValue value)
    {
        SwitchForm();
    }

    public void DisplayInteraction()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponentInParent<Interact>() != null)
            {
                displayText.text = hitObject.GetComponentInParent<Interact>().InteractionText(heldObject);
            }
            else
            {
                displayText.text = "";
            }
        }
        else
        {
            displayText.text = "";
        }
    }

    public void OnInteract(InputValue value)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponentInParent<Interact>() != null)
            {
                hitObject.GetComponentInParent<Interact>().Interaction(gameObject);
                Debug.Log("Grab Item");
            }
            else if(heldObject != null)
            {
                Debug.Log("Drop Item");
                DropItem();
            }
        }
        else
        {
            Debug.Log("Nothing hit");
            DropItem();
        }
    }

    public void DropItem()
    {
        if (heldObject != null)
        {
            Debug.Log("Drop Item");
            heldObject.transform.parent = null;
            heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            heldObject.GetComponentInChildren<Collider>().enabled = true;
            heldObject = null;
            holdObjectRig.weight = 0;
        }
    }

    public void DisposeItem()
    {
        if(heldObject != null)
        {
            Debug.Log("Dispose Item");
            Destroy(heldObject);
            heldObject = null;
            holdObjectRig.weight = 0;
        }
    }

    public void SwitchForm()
    {
        float distanceToBody = Vector3.Distance(playerBody.transform.position, playerSpirit.transform.position);

        if (isInSpiritForm  && (distanceToBody <= spiritReturnDistance || spiritTimer >= spiritTime))
        {
            form = playerBody;
            virtualMainCamera = virtualBodyCamera;
            virtualBodyCamera.gameObject.SetActive(true);
            spiritTimer = 0;
            bodyTimer = 0;
            playerSpirit.SetActive(false);
            isInSpiritForm = false;
            mainCamera.cullingMask = 3959;
        }
        else if(canEnterSpiritForm)
        {
            playerSpirit.SetActive(true);
            form = playerSpirit;
            virtualMainCamera = virtualSpiritCamera;
            virtualBodyCamera.gameObject.SetActive(false);
            spiritTimer = 0;
            bodyTimer = 0;
            isInSpiritForm = true;
            mainCamera.cullingMask = -1;
        }
        StartCoroutine(PostProcessingTimer());
    }

    public IEnumerator PostProcessingTimer()
    {
        if (!isInSpiritForm)
        {
            yield return new WaitForSeconds(cinemachineBrain.m_DefaultBlend.m_Time);
        }
        else
        {
            yield return null;
        }

        postProcessingScript.LensView(isInSpiritForm);
    }
}
