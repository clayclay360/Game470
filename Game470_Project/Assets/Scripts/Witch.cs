using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.HID;

public class Witch : MonoBehaviour
{
    [Header("Variables")]
    public float currentSpeed;
    public float walkSpeed;
    public float chaseSpeed;
    public float angularSpeed;
    public bool canRoam, canChase,isAlive, isRoaming, isChasing, capturedPlayer;


    [Header("Roam")]
    public float delay;
    public float destinationOffset;
    public Transform[] locations;

    [Header("Animator")]
    public Animator animator;

    [Header("Linecast")]
    public Transform lincecastStart;
    public Transform linecastEnd;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

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
        agent.speed = currentSpeed;
        agent.angularSpeed = angularSpeed;

        if (canRoam && !isRoaming)
        {
            StartCoroutine(Roam());
        }

        if (isChasing)
        {
            agent.SetDestination(player.transform.position);
        }

        Linecast();
    }

    IEnumerator Roam()
    {
        while(isAlive && canRoam)
        {
            isRoaming = true;
            int locationIndex = -1;
            currentSpeed = walkSpeed;

            // set animators
            animator.SetFloat("Blend", 0);

            yield return new WaitForSeconds(delay); //wait

            // set animators
            animator.SetFloat("Blend", .5f);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerController.isCaptured = true;
            //playerController.virtualMainCamera.gameObject.SetActive(false);
            agent.isStopped = true;
            virtualCamera.m_Priority = 12;
            capturedPlayer = true;
            isChasing = false;
            CapturedPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ChasePlayer(other.gameObject);
        }
    }

    private void Linecast()
    {
        RaycastHit hit;
        
        if(Physics.Linecast(lincecastStart.position, linecastEnd.position, out hit))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                PlayerController player = hit.collider.gameObject.GetComponentInParent<PlayerController>();
                if (!isChasing && !capturedPlayer && !player.isHiding)
                {
                    ChasePlayer(hit.collider.gameObject);
                }
            }
        }
    }

    private void ChasePlayer(GameObject target)
    {
        Debug.Log("Chase Player");
        canRoam = false;
        isChasing = true;
        player = target;
        currentSpeed = chaseSpeed;
        animator.SetFloat("Blend", 1f);
    }

    private void CapturedPlayer()
    {
        animator.SetTrigger("Captured");
        GameManager.playerCaptured = true;
        FindObjectOfType<Main>().FadeOut();
    }
}
