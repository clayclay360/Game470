using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Witch : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float angularSpeed;
    public bool canRoam, isAlive, isRoaming;


    [Header("Roam")]
    public float delay;
    public float destinationOffset;
    public Transform[] locations;
    [HideInInspector] public NavMeshAgent agent;


    private int lastLocationIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Roam());
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;

        if (canRoam && !isRoaming)
        {
            StartCoroutine(Roam());
        }
    }

    IEnumerator Roam()
    {
        while(isAlive && canRoam)
        {
            isRoaming = true;
            int locationIndex = -1;

            yield return new WaitForSeconds(delay); //wait

            do
            {
                locationIndex = Random.Range(0, locations.Length); //generate a random number not previous to the last
            }
            while (lastLocationIndex == locationIndex);
            lastLocationIndex = locationIndex;

            agent.SetDestination(locations[locationIndex].position);

            // set the difference
            Vector2 difference = new Vector2(1,1);

            // when the witch is close enough to the destination, continue.
            while (difference.x > destinationOffset ||
                difference.y > destinationOffset)
            {
                yield return null;

                // get the difference between the players position and the destination
                difference = new Vector2(Mathf.Abs(transform.position.x - agent.destination.x),
                Mathf.Abs(transform.position.z - agent.destination.z));
            }
        }
    }
}
