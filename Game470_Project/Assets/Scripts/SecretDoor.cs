using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour
{
    public PressurePlate pressurePlate;
    
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        pressurePlate.GetComponent<PressurePlate>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pressurePlate.GetComponent<PressurePlate>() != null)
        {
            animator.SetBool("Open", pressurePlate.isPushed);
        }
    }
}
