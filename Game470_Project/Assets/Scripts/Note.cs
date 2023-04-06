using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interact
{
    public Sprite noteSprite;
    public override void Interaction(GameObject player)
    {
        FindObjectOfType<PlayerController>().noteImage.enabled = true;
        FindObjectOfType<PlayerController>().noteImage.sprite = noteSprite;
        FindObjectOfType<PlayerController>().isReading = true;
    }

    public override string InteractionText(GameObject heldObject)
    {
        return "Read Note";
    }
}
