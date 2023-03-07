using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPodium : MonoBehaviour
{
    public GameObject podium1;
    public GameObject podium2;
    public GameObject podium3;
    public GameObject spiritOrb;
    public GameObject spirit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(podium1.GetComponent<MinorPodium>().hasTotem && podium2.GetComponent<MinorPodium>().hasTotem && podium3.GetComponent<MinorPodium>().hasTotem)
        {
            spiritOrb.SetActive(false);
            spirit.SetActive(true);
        }
    }
}
