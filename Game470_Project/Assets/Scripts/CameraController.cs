using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraController : MonoBehaviour
{
    
    public int internCoeffX = 1500;
    public int internCoeffY = 100;
    public float minY = -70;
    public float maxY = 70;
    //[HideInInspector]
    public float moveVertical = 0f, moveHorizontal = 0f;


    private Vector2 rot;

    // Start is called before the first frame update
    void Start()
    {
        //moveHorizontal = 0f;
        //moveVertical = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayer.controlCamera)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            moveHorizontal -= Input.GetAxis("Mouse X") * internCoeffX * Time.deltaTime;
            moveVertical += Input.GetAxis("Mouse Y") * internCoeffY * Time.deltaTime;
            moveVertical = Mathf.Clamp(moveVertical, minY, maxY);

            transform.localEulerAngles = new Vector3(-moveVertical, moveHorizontal, 0f);
        }
    }

    public void OnLookAround(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        rot.x += input.x;
        rot.y -= input.y;
    }

    public void ResetValue()
    {
        moveHorizontal = 0;
        moveVertical = 0;
    }
}
