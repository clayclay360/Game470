using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public void CanControlCamera(bool condition)
    {
        GameManager.canPlayer.controlCamera = condition;
    }
}
