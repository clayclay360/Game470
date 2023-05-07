using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : CollectableObject
{
    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        gameObject.GetComponentInChildren<Collider>().enabled = false;
        transform.parent = playerScript.holdPoint.transform;
        transform.position = playerScript.holdPoint.transform.position;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        playerScript.heldObject = gameObject;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerScript.holdObjectRig.weight = 1;
        GameManager.canPlayer.interact = false;
    }

    public override string InteractionText(GameObject heldObject)
    {
        return "Grab " + Name + " Key";
    }
}
