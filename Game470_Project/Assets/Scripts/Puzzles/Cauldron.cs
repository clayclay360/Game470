using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Interact
{

    public bool ingredientsPrepared = false;
    public bool potionBrewed = false;

    public GameObject blood, eyes, ashes, petals;
    public GameObject potion;
    public List<GameObject> ingredientsList;

    public override void Interaction(GameObject player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if(!ingredientsPrepared)
        {
            if (playerScript.heldObject && playerScript.heldObject.TryGetComponent<CollectableObject>(out CollectableObject objectScript))
            {
                switch (objectScript.Name)
                {
                    case "Eyes":
                    case "Petals":
                    case "Ashes":
                        {
                            if (!ingredientsList.Contains(playerScript.heldObject))
                            {
                                playerScript.heldObject.transform.SetParent(gameObject.transform);
                                playerScript.heldObject.transform.position = gameObject.transform.position + new Vector3(0, 0.1f, 0);
                                ingredientsList.Add(playerScript.heldObject);
                                playerScript.heldObject = null;
                            }
                            break;
                        }
                    case "Knife":
                        {
                            if (!ingredientsList.Contains(blood))
                            {
                                blood.SetActive(true);
                                ingredientsList.Add(blood);
                            }
                            break;
                        }
                }
                if (ingredientsList.Contains(eyes) && ingredientsList.Contains(ashes) && ingredientsList.Contains(petals) && ingredientsList.Contains(blood))
                {
                    ingredientsPrepared = true;
                }
            }
        }
        else
        {
            foreach (GameObject ingredient in ingredientsList)
            {
                if (ingredient == blood)
                {
                    ingredient.SetActive(false);
                }
                else
                {
                    Destroy(ingredient);
                }
            }
            potionBrewed = true;
            potion.SetActive(true);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        string currIngredients = "";
        if (heldObject && heldObject.TryGetComponent<CollectableObject>(out CollectableObject objectScript))
        {
            if (objectScript.Name == "Ashes" || objectScript.Name == "Eyes" || objectScript.Name == "Petals")
            {
                return "Add " + objectScript.Name;
            }
            else if(objectScript.Name == "Knife" && !ingredientsList.Contains(blood))
            {
                return "Add Blood";
            }
            else
            {
                return "Not an Ingredient";
            }
        }
        else if (ingredientsPrepared)
        {
            return "Brew Potion";
        }
        else if(ingredientsList.Count > 0)
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
