using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorPodium : Interact
{
    public bool hasTotem = false;
    
    public GameObject holdPoint;

    // Start is called before the first frame update
    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (playerScript.heldObject != null && playerScript.heldObject.TryGetComponent<Totem>(out Totem totem))
        {
            playerScript.heldObject = null;
            playerScript.holdObjectRig.weight = 0;
            totem.transform.parent = holdPoint.transform;
            totem.transform.localPosition =  Vector3.zero;
            totem.transform.parent = holdPoint.transform;
            totem.transform.eulerAngles = Vector3.zero;
            hasTotem = true;
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        if(heldObject != null && heldObject.TryGetComponent<Totem>(out Totem totem))
        {
            return "Place Totem";
        }
        else if (!hasTotem)
        {
            return "Need Totem";
        }

        return "";
    }
}
