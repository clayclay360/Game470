using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Candle : Interact
{
    public GameObject pointLight;
    public bool on;

    private void Update()
    {
            pointLight.SetActive(on);
    }

    public override void Interaction(GameObject player)
    {
        if (!on && player.GetComponent<PlayerController>().heldObject != null && 
            player.GetComponent<PlayerController>().heldObject.TryGetComponent<PlayerCandle>(out PlayerCandle playerCandle))
        {
            pointLight.SetActive(true);
            on = true;
        }
    }

    public override string InteractionText(GameObject heldObject)
    {
        if (!on && heldObject != null && heldObject.TryGetComponent<PlayerCandle>(out PlayerCandle playerCandle))
        {
            return "Light Candle";
        }
        return "";
    }
}
