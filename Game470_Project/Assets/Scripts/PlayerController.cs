using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turningspeed;
    public float spiritTime; //The ammount of time the player can spend in spirit form
    public float spiritCooldown; //The ammount of time the player has to wait to reenter their spirit form after leaving it
    public float spiritReturnDistance; //The distance from which the player can manually reenter their body
    public bool isInSpiritForm; //This should be false by default

    public GameObject playerBody;
    public GameObject playerSpirit;
    public Camera mainCamera;

    private float spiritTimer = 0; //The ammount of time the player has spent in spirit form
    private float bodyTimer = 0; //The ammount of time the player has spent in their body
    private bool canEnterSpiritForm = true;

    private Vector2 rot;
    private Vector2 oldMousePosition;
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
        //Moving forward
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z) * speed * Time.deltaTime;
            form.transform.position += dir * speed * Time.deltaTime;
        }
        //Moving backward
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z) * speed * Time.deltaTime * -1;
            form.transform.position += dir * speed * Time.deltaTime;
        }
        //Moving left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z) * speed * Time.deltaTime * -1;
            form.transform.position += dir * speed * Time.deltaTime;
        }
        //Moving right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z) * speed * Time.deltaTime;
            form.transform.position += dir * speed * Time.deltaTime;
        }
        //Spirit moves with body when not in spirit form
        if (!isInSpiritForm)
        {
            playerSpirit.transform.position = playerBody.transform.position;
            mainCamera.transform.position = playerBody.transform.position + new Vector3(0, 0.2f, 0);
        }
        //Body does not follow spirit when in spirit form.
        else
        {
            mainCamera.transform.position = playerSpirit.transform.position;
        }
        //Turning right/left and looking up/down
        rot.x -= Input.GetAxis("Mouse X") * turningspeed;
        rot.y -= Input.GetAxis("Mouse Y") * turningspeed;
        form.transform.localRotation = Quaternion.Euler(0, rot.x, 0);
        mainCamera.transform.localRotation = Quaternion.Euler(rot.y, 0, 0);
        #endregion
        #region Actions
        //Enter spirit form
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchForm();
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
            if(bodyTimer < spiritCooldown)
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
