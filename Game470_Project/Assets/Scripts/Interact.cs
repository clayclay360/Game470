using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    public string Name;
    public bool canMakeNoiseWhenDropped;
    public AudioClip droppingNoise;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public abstract void Interaction(GameObject player);

    public abstract string InteractionText(GameObject heldObject = null);

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (canMakeNoiseWhenDropped)
            {
                FindObjectOfType<Main>().witch.SetActive(true);
                FindObjectOfType<Witch>().FollowNoise(transform.position);
                audioSource.clip = droppingNoise;
                audioSource.Play();
                Debug.Log("Make Some Noise!!!");
            }
        }
    }
}
