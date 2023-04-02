using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    public abstract void Interaction(GameObject player);

    public abstract string InteractionText(GameObject heldObject);
}
