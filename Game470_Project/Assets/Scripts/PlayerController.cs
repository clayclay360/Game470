using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float spiritTime; //The ammount of time the player can spend in spirit form
    public float spiritCooldown; //The ammount of time the player has to wait to reenter their spirit form after leaving it
    public float spiritReturnDistance; //The distance from which the player can manually reenter their body
    public bool isInSpiritForm; //This should be false by default

    public GameObject playerBody;
    public GameObject playerSpirit;
    public Camera mainCamera;

    private float spiritTimer = 0; //The ammount of time the player has spent in spirit form
    private float bodyTimer = 0; //The ammount of time the player has spent in their body
    private int spiritFormCounter = 0; //The number of times the player has gone into spirit form
    private bool canEnterSpiritForm = true;

    public Vector2 rot = Vector2.zero;
    public Vector3 moveVal;
    private GameObject form;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        form = playerBody;
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        // Player Movement
        Vector3 realFoward = Vector3.Cross(mainCamera.transform.right, Vector3.up);
        Debug.Log(mainCamera.transform.right + " , " + 0 + " , " + realFoward);
        if (Mathf.Abs(moveVal.x) > 0 || Mathf.Abs(moveVal.z) > 0)
        {
            if (moveVal.magnitude > 0.1f)
            {
                form.transform.position += speed * Time.deltaTime * moveVal;
            }
        }
        //Spirit moves with body when not in spirit form
        if (!isInSpiritForm)
        {
            playerSpirit.transform.position = playerBody.transform.position;
            mainCamera.transform.position = playerBody.transform.position + new Vector3(0, 0.2f, 0);
        }
        //Looking Around
        form.transform.localEulerAngles = new Vector3(0, rot.x, 0);
        mainCamera.transform.localEulerAngles = new Vector3(rot.y, 0, 0);
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
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveVal = new Vector3(input.x, 0f, input.y);
        Vector3 realFoward = Vector3.Cross(mainCamera.transform.right, Vector3.up);
        moveVal = moveVal.x * mainCamera.transform.right + moveVal.z * realFoward;
    }

    public void OnLookAround(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        rot.x += input.x;
        rot.y -= input.y;
        Mathf.Clamp(rot.y, -90, 90);
    }

    public void OnSwitchForm(InputValue value)
    {
        SwitchForm();
    }

    public void SwitchForm()
    {
        float distanceToBody = Vector3.Distance(playerBody.transform.position, playerSpirit.transform.position);
        Debug.Log(distanceToBody.ToString());
        if (isInSpiritForm  && (distanceToBody <= spiritReturnDistance || spiritTimer >= spiritTime))
        {
            form = playerBody;
            mainCamera.transform.SetParent(form.transform);
            spiritTimer = 0;
            bodyTimer = 0;
            playerSpirit.SetActive(false);
            isInSpiritForm = false;
        }
        else if(canEnterSpiritForm)
        {
            playerSpirit.SetActive(true);
            form = playerSpirit;
            mainCamera.transform.SetParent(form.transform);
            spiritTimer = 0;
            bodyTimer = 0;
            isInSpiritForm = true;
        }
    }
}
