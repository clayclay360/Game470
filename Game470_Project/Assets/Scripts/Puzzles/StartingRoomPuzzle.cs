using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoomPuzzle : MonoBehaviour
{
    public bool completed;
    public int[] sequence = new int[3];
    public int index;

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
        
        tagStorage[0] = "Null";
        tagStorage[1] = "Null";
        tagStorage[2] = "Null";

        for (int i = 0; i < books.Length; i++)
        {
            int index = Random.Range(0, books.Length);
            //work in progress
            for (int c = 0; c < tagStorage.Length; c++)
            {
                if (tagStorage[c] == idTags[index])
                {
                    books[i].ID = idTags[index];
                    i--;
                }
                else
                {
                    tagStorage[c] = idTags[index];
                    break;
                }
            }
                


        }
    }

}
