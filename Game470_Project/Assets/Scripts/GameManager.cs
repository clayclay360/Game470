using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CanPlayer
{
    public bool controlCamera;
    public bool walk;
    public bool jump;
    public bool attack;
}

public struct IsPlayer
{
    public bool walking;
    public bool jumping;
    public bool attacking;
}

public class GameManager
{
    public const bool debug = false;
    public static bool isGamePaused = false;
    public static bool playerCaptured;

    //Player
    public static CanPlayer canPlayer = new CanPlayer();
    public static IsPlayer isPlayer = new IsPlayer();
    public const float playerWalkSpeed = 1.75f;
    public const float playerSprintSpeed = 3f;
    public const float playerJumpMagnitude = 5000f;

    public static void ResetVariables()
    {
        canPlayer.controlCamera = true;
        canPlayer.walk = true;
        canPlayer.jump = true;
        canPlayer.attack = true;

        isPlayer.walking = false;
        isPlayer.jumping = false;
        isPlayer.attacking = false;
        Debug.Log("variables reset");
    }
}
