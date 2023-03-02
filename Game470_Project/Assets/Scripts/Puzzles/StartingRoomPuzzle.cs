using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoomPuzzle : MonoBehaviour
{
    [Header("Puzzle")]
    public bool completed;
    public int index;

    [Space]
    public GameObject door;

    [Header("Books")]
    public Material[] mat;

    private Book[] books;
    private string[] idTags = new string[3];

    public void Start()
    {
        idTags[0] = "572";
        idTags[1] = "849";
        idTags[2] = "639";

        RandomizeBooks();
    }

    public void RandomizeBooks()
    {
        books = FindObjectsOfType<Book>();
        string[] tagStorage = new string[3];
        
        // set tag storage to be null
        tagStorage[0] = "Null";
        tagStorage[1] = "Null";
        tagStorage[2] = "Null";

        // this randomizes the book numbers
        for (int i = 0; i < books.Length; i++)
        {
            int index = Random.Range(0, books.Length);
            //work in progress
            for (int c = 0; c < tagStorage.Length; c++)
            {
                if (tagStorage[c] == idTags[index])
                {
                    i--;
                    break;
                }
                else if(tagStorage[c] == "Null")
                {
                    books[i].ID = idTags[index];
                    tagStorage[c] = idTags[index];
                    books[i].addMaterial(mat[index]);
                    break;
                }
            }
        }
    }

    public void Puzzle(string ID)
    {
        if (idTags[index] == ID)
        {
            index++;
            Debug.Log("Good Job");
        }
        else
        {
            Debug.Log("Start Over");
            index = 0;
        }

        if(index >= idTags.Length - 1)
        {
            completed = true;
            Complete();
        }
    }

    public void Complete()
    {
        door.SetActive(false);
    }

}
