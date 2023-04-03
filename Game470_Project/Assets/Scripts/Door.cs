using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interact
{
    public bool locked;
    public bool isOpened;

    public override void Interaction(GameObject player)
    {
        if (!locked)
        {
            isOpened = !isOpened;
            GetComponent<Animator>().SetBool("isOpened",isOpened);
        }
        else if(player.GetComponent<PlayerController>().heldObject != null &&
            player.GetComponent<PlayerController>().heldObject.TryGetComponent<Key>(out Key key))
        {
            if(key.Name == Name)
            {
                locked = false;
                player.GetComponent<PlayerController>().DisposeItem();
            }
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        if(heldObject != null && heldObject.TryGetComponent<Key>(out Key key))
        {
            if(key.Name == Name)
            {
                return "Unlock Door";
            }
        }
        else if (locked)
        {
            return Name + " Door Locked";
        }
        else if (isOpened)
        {
            return "Close Door";
        }
        return "Open Door";
    }
}
