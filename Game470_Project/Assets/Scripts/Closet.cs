using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : Interact
{
    public GameObject leftDoor, rightDoor, hidingPoint, exitPoint;

    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (!playerScript.isHiding)
        {
            if (!playerScript.isInSpiritForm)
            {
                playerScript.playerBody.transform.position = hidingPoint.transform.position;
            }
            else
            {
                playerScript.playerSpirit.transform.position = hidingPoint.transform.position;
            }
            playerScript.isHiding = true;
            playerScript.playerBody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            if (!playerScript.isInSpiritForm)
            {
                playerScript.playerBody.transform.position = exitPoint.transform.position;
            }
            else
            {
                playerScript.playerSpirit.transform.position = exitPoint.transform.position;
            }
            playerScript.isHiding = false;
            playerScript.playerBody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        return "Hide";
    }
}
