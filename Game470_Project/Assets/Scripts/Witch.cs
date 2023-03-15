using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class Witch : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float angularSpeed;
    public bool canRoam, canChase,isAlive, isRoaming, isChasing;


    [Header("Roam")]
    public float delay;
    public float destinationOffset;
    public Transform[] locations;

    [Header("Animator")]
    public Animator animator;

    [HideInInspector]public Rigidbody rb;
    [HideInInspector] public NavMeshAgent agent;

    private GameObject player;

    private int lastLocationIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator.GetComponent<Animator>();

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

        if (isChasing)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator Roam()
    {
        while(isAlive && canRoam)
        {
            isRoaming = true;
            int locationIndex = -1;

            // set animators
            animator.SetFloat("Blend", 0);

            yield return new WaitForSeconds(delay); //wait

            // set animators
            animator.SetFloat("Blend", 1);

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Hello");
            canRoam = false;
            isChasing = true;
            player = other.gameObject;
        }
    }
}
