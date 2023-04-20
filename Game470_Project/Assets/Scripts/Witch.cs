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
    public bool canRoam, canChase, detectedNoise, isAlive, isRoaming, isChasing, capturedPlayer, followingNoise;


    [Header("Roam")]
    public float delay;
    public float destinationOffset;
    public Transform[] locations;
    [HideInInspector]
    public Vector3 noiseLocation;

    [Header("Animator")]
    public Animator animator;

    [Header("Linecast")]
    public Transform lincecastStart;
    public Transform linecastEnd;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Collider")]
    public Collider areaCollider;

    [Header ("Audio")]
    private AudioSource audioSource;

    [HideInInspector]public Rigidbody rb;
    [HideInInspector] public NavMeshAgent agent;

    private GameObject player;
    private Vector3 startPos;

    private int lastLocationIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        startPos = transform.position;

        StartCoroutine(PlayLaugh());
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
        areaCollider.enabled = !followingNoise; // if the witch is following the noice, disable collider else enable
    }
    
    public void ResetVariables()
    {
        transform.position = startPos;
        detectedNoise = false;
        followingNoise = false;
        capturedPlayer = false;
        isChasing = false;
        isRoaming = true;
        canRoam = true;
        animator.SetTrigger("Reset");
        agent.enabled = true;
        StartCoroutine(Roam());
    }

    IEnumerator Roam()
    {
        while (isAlive && canRoam)
        {
            isRoaming = true;
            int locationIndex = -1;
            currentSpeed = walkSpeed;

            // set animators
            animator.SetFloat("Blend", 0);

            yield return new WaitForSeconds(delay); //wait

            // set animators
            animator.SetFloat("Blend", .5f);

            // if the witch is not following the noise, roamm around the manson
            if (!detectedNoise)
            {
                do
                {
                    locationIndex = Random.Range(0, locations.Length); //generate a random number not previous to the last
                }
                while (lastLocationIndex == locationIndex);
                lastLocationIndex = locationIndex;

                agent.SetDestination(locations[locationIndex].position);
            }
            else
            {
                Debug.Log("FOLLOW NOISE");
                agent.SetDestination(noiseLocation);
                followingNoise = true;
                detectedNoise = false;
            }

            // set the difference
            Vector2 difference = new Vector2(1,1);

            // when the witch is close enough to the destination, continue.
            while (difference.x > destinationOffset ||
                difference.y > destinationOffset)
            {
                yield return null;

                // get the difference between the witch position and the destination
                difference = new Vector2(Mathf.Abs(transform.position.x - agent.destination.x),
                Mathf.Abs(transform.position.z - agent.destination.z));
            }

            // when the witch has completed following the noise then continue to roam
            if (followingNoise)
            {
                followingNoise = false;
            }
        }
    }

    public void FollowNoise(Vector3 location)
    {
        detectedNoise = true;
        noiseLocation = location;
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
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }

    IEnumerator PlayLaugh()
    {
        while (true)
        {
            float time = Random.Range(5, 25);

            yield return new WaitForSeconds(time);
            audioSource.Play();
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
        agent.enabled = false;
        FindObjectOfType<Main>().FadeOut();
    }
}
