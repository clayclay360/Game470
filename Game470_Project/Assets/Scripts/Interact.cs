using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    public string Name;
    public bool canMakeNoiseWhenDropped;

    public abstract void Interaction(GameObject player);

    public abstract string InteractionText(GameObject heldObject);

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (canMakeNoiseWhenDropped)
            {
                FindObjectOfType<Witch>().FollowNoise(transform.position);
                Debug.Log("Make Some Noise!!!");
            }
        }
    }
}
