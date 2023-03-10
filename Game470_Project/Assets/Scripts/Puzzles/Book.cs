using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Interact
{
    public string ID;


    private StartingRoomPuzzle puzzle;
    
    public void addMaterial(Material mat)
    {
        switch (ID)
        {
            case "572":
                GetComponentInChildren<Renderer>().material = mat;
                break;
            case "849":
                GetComponentInChildren<Renderer>().material = mat;
                break;
            case "639":
                GetComponentInChildren<Renderer>().material = mat;
                break;

        }
    }

    public override void Interaction(GameObject player)
    {
        GetComponent<Animator>().SetTrigger("Pull");
        FindObjectOfType<StartingRoomPuzzle>().Puzzle(ID);
    }
}
