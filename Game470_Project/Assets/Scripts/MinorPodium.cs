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
        switch (gameObject.name)
        {
            case ("Minor Podium 1"):
                if(playerScript.heldObject.name == "Totem 1")
                {
                    playerScript.heldObject.transform.position = holdPoint.transform.position;
                    playerScript.heldObject.transform.SetParent(gameObject.transform);
                    hasTotem = true;
                }
                else
                {
                    Debug.Log("Wrong Item");
                }
                break;
            case ("Minor Podium 2"):
                if (playerScript.heldObject.name == "Totem 2")
                {
                    playerScript.heldObject.transform.position = holdPoint.transform.position;
                    playerScript.heldObject.transform.SetParent(gameObject.transform);
                    hasTotem = true;
                }
                else
                {
                    Debug.Log("Wrong Item");
                }
                break;
            case ("Minor Podium 3"):
                if (playerScript.heldObject.name == "Totem 3")
                {
                    playerScript.heldObject.transform.position = holdPoint.transform.position;
                    playerScript.heldObject.transform.SetParent(gameObject.transform);
                    hasTotem = true;
                }
                else
                {
                    Debug.Log("Wrong Item");
                }
                break;
        }
    }
}
