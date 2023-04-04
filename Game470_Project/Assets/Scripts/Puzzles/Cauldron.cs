using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Interact
{

    public bool ingredientsPrepared = false;
    public List<GameObject> ingredientsList;

    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if(playerScript.heldObject && playerScript.heldObject.TryGetComponent<CollectableObject>(out CollectableObject objectScript))
        {
            switch (objectScript.Name)
            {
                case "Eyes": 
                case "Petal": 
                case "Ashes":
                    {
                        if (!ingredientsList.Contains(playerScript.heldObject))
                        {
                            playerScript.heldObject.transform.SetParent(gameObject.transform);
                            playerScript.heldObject.transform.position = gameObject.transform.position;
                            ingredientsList.Add(playerScript.heldObject);
                            playerScript.heldObject = null;
                        }
                        break;
                    }
                case "Knife":
                    {

                        break;
                    }
            }
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        if(heldObject.TryGetComponent<CollectableObject>(out CollectableObject objectScript))
        {
            return objectScript.Name;
        }
        string currIngredients = "";
        if(ingredientsList.Count > 0)
        {
            foreach (GameObject ingredient in ingredientsList)
            {
                CollectableObject ingredientScript = ingredient.GetComponent<CollectableObject>();
                currIngredients += "Contains " + ingredientScript.Name + "\n";
            }
            return currIngredients;
        }
        else
        {
            return "No Ingredients";
        }
    }
}
