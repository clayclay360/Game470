using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interact
{
    public bool locked;
    public bool isOpened;

    [Header("Audio")]
    [HideInInspector] 
    public AudioSource audioSource;
    public AudioClip creakingDoor, unlockingDoor;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interaction(GameObject player)
    {
        if (!locked)
        {
            isOpened = !isOpened;
            GetComponent<Animator>().SetBool("isOpened",isOpened);
            audioSource.clip = creakingDoor;
            audioSource.Play();
        }
        else if(player.GetComponent<PlayerController>().heldObject != null &&
            player.GetComponent<PlayerController>().heldObject.TryGetComponent<Key>(out Key key))
        {
            if(key.Name == Name)
            {
                locked = false;
                player.GetComponent<PlayerController>().DisposeItem();
                audioSource.clip = unlockingDoor;
                audioSource.Play();
            }
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        if(heldObject != null && heldObject.TryGetComponent<Key>(out Key key) && locked)
        {
            if(key.Name == Name)
            {
                return "Unlock Door";
            }
            else
            {
                return "Door Locked";
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
