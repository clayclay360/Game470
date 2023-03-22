using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public float direction;

    // Update is called once per frame
    void Update()
    {

        Debug.Log(Mathf.Tan(transform.localEulerAngles.y));

        ChangeDirection();

        Rotation();
    }

    public void ChangeDirection()
    {
        //if (Mathf.Tan(transform.localEulerAngles.y) > 1)
        //{
        //    direction = -1;
        //}

        if (Mathf.Tan(transform.localEulerAngles.y) < -1)
        {
            Debug.Log("Positive");
            direction = 1;
        }
    }

    public void Rotation()
    {
        transform.Rotate(new Vector3(0, direction, 0) * speed * Time.deltaTime);
    }
}
