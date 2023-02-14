using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turningspeed;

    public Camera mainCamera;

    private Vector2 rot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        //Moving forward
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z) * speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }
        //Moving backward
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z) * speed * Time.deltaTime * -1;
            transform.position += dir * speed * Time.deltaTime;
        }
        //Moving left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z) * speed * Time.deltaTime * -1;
            transform.position += dir * speed * Time.deltaTime;
        }
        //Moving right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 dir = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.forward.z) * speed * Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
        }
        //Turning right/left and looking up/down
        rot.x -= Input.GetAxis("Mouse X") * turningspeed;
        rot.y -= Input.GetAxis("Mouse Y") * turningspeed;
        transform.localRotation = Quaternion.Euler(0, rot.x, 0);
        mainCamera.transform.localRotation = Quaternion.Euler(rot.y, 0, 0);
        #endregion
    }
}
