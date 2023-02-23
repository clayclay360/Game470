using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : Interact
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        transform.SetParent(player.transform.GetChild(playerScript.playerBody.transform.GetSiblingIndex()));
        transform.position = playerScript.holdPoint.transform.position;
        playerScript.heldObject = gameObject;
        gameObject.GetComponentInChildren<Collider>().enabled = false;
    }
}
