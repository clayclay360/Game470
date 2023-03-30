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
        if (playerScript.heldObject != null && playerScript.heldObject.TryGetComponent<CollectableObject>(out CollectableObject CO))
        {
            playerScript.heldObject = null;
            playerScript.holdObjectRig.weight = 0;
            CO.transform.parent = holdPoint.transform;
            CO.transform.localPosition =  Vector3.zero;
            CO.transform.parent = holdPoint.transform;
            CO.transform.eulerAngles = Vector3.zero;
            hasTotem = true;
        }
    }
}
