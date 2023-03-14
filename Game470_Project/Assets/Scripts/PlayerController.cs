using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float spiritTime; //The ammount of time the player can spend in spirit form
    public float spiritCooldown; //The ammount of time the player has to wait to reenter their spirit form after leaving it
    public float spiritReturnDistance; //The distance from which the player can manually reenter their body
    public bool isInSpiritForm; //This should be false by default
    public float interactionRange;

    public GameObject playerBody;
    public GameObject playerSpirit; 
    public GameObject form;
    public GameObject holdPoint;
    public GameObject heldObject;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualBodyCamera, virtualSpiritCamera;

    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera virtualMainCamera;
    private CameraController bodyCameraController, spiritCameraController;
    private PostProcessingScript postProcessingScript;
    private float spiritTimer = 0; //The ammount of time the player has spent in spirit form
    private float bodyTimer = 0; //The ammount of time the player has spent in their body
    private int spiritFormCounter = 0; //The number of times the player has gone into spirit form
    private bool canEnterSpiritForm = true;

    public Vector2 rot = Vector2.zero;
    public Vector3 moveVal;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        form = playerBody;
        virtualMainCamera = virtualBodyCamera;
        mainCamera.GetComponent<Camera>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        postProcessingScript = FindObjectOfType<PostProcessingScript>();
        bodyCameraController = virtualBodyCamera.GetComponent<CameraController>();
        spiritCameraController = virtualSpiritCamera.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        // Player Movement
        Vector3 forwardMovement = Vector3.zero;
        Vector3 rightMovement = Vector3.zero;
        float moveHorizontal = moveVal.x;
        float moveVertical = moveVal.z;

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
            if(bodyTimer < spiritCooldown && spiritFormCounter > 0)
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

    public void OnInteract(InputValue value)
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, interactionRange))
        {
            GameObject hitObject = hit.collider.gameObject;
            //Debug.Log(hitObject);
            if(hitObject.name == "Mason" && heldObject != null)
            {
                heldObject.transform.SetParent(null);
                heldObject.GetComponentInChildren<Collider>().enabled = true;
                heldObject = null;
            }
            else
            {
                if (hitObject.GetComponentInParent<Interact>() != null)
                {
                    hitObject.GetComponentInParent<Interact>().Interaction(gameObject);
                }
                else if(hitObject.GetComponent<Interact>() != null)
                {
                    hitObject.GetComponent<Interact>().Interaction(gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Nothing hit");
        }
    }

    public void SwitchForm()
    {
        float distanceToBody = Vector3.Distance(playerBody.transform.position, playerSpirit.transform.position);

        Debug.Log(distanceToBody.ToString());
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
