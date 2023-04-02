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
    }

    public override string InteractionText(GameObject heldObject)
    {
        if (locked)
        {
            return "Doors Locked";
        }
        else if (isOpened)
        {
            return "Close Door";
        }

        return "Open Door";
    }
}
