using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : Interact
{

    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        transform.parent = playerScript.holdPoint.transform;
        transform.position = playerScript.holdPoint.transform.position;
        transform.localEulerAngles = new Vector3(0,0,0);
        playerScript.heldObject = gameObject;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerScript.holdObjectRig.weight = 1;
        gameObject.GetComponentInChildren<Collider>().enabled = false;
        GameManager.canPlayer.interact = false;
    }
}
