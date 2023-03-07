using UnityEngine.AI;
using UnityEngine;
using Unity.Collections;
using System.Collections;

public class EvilSpirit : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float angularSpeed;
    public bool isAlive;
    public bool isRoaming;

    [Header("Detector")]
    public float range;
    public float destinationOffset;
    public int dir;

    [Header("Capture")]
    public bool isTrappingPlayer;
    public bool canTrapPlayer;
    public float trapTime; //The ammount of time the Evil Spirit will trap the player for
    public float trapCooldown; //The ammount of time before the Evil Spirit can trap the player again after releasing them
    public GameObject player;
    public GameObject witch;
    private PlayerController playerScript;

    private float trapTimer = 0f; //The ammount of time the player has been trapped for
    private float cooldownTimer = 0f; //The ammount of time since the player was last trapped by this spirit

    [Header("Roam")]
    public float delay;

    private int lastDir;

    private Vector3 startPos;

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;

    void Start()
    {
        // get all the components
        #region getComponents
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        #endregion

        playerScript = player.GetComponent<PlayerController>();

        startPos = new Vector3(transform.position.x,0f, transform.position.z); // get the players start pos
        StartCoroutine(Roam()); // start roaming
    }

    void Update()
    {
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;

        if (isTrappingPlayer)
        {
            if (trapTimer < trapTime)
            {
                playerScript.speed = 0f;
                trapTimer += Time.deltaTime;
            }
            else
            {
                isTrappingPlayer = false;
                trapTimer = 0;
                canTrapPlayer = false;
                playerScript.speed = 1;
            }
        }
        if(!canTrapPlayer)
        {
            if (cooldownTimer < trapCooldown)
            {
                cooldownTimer += Time.deltaTime;
            }
            else
            {
                canTrapPlayer = true;
                cooldownTimer = 0;
            }
        }
        if (!isRoaming)
        {
            StartCoroutine(Roam());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            if (playerScript.isInSpiritForm && canTrapPlayer)
            {
                isTrappingPlayer = true;
                isRoaming = false;
            }
        }
    }

    IEnumerator Roam()
    {
        //while alive and not trapping the player
        while (isAlive  && !isTrappingPlayer)
        {
            isRoaming = true;
            yield return new WaitForSeconds(delay); //wait

            do
            {
                dir = Random.Range(0, 4); //generate a random number not previous to the last
            }
            while (lastDir == dir);
            lastDir = dir;

            // go in a dir from a certain range
            switch (dir)
            {
                case 0:
                    agent.SetDestination(startPos + (Vector3.right * range));
                    break;
                case 1:
                    agent.SetDestination(startPos + (Vector3.left * range));
                    break;
                case 2:
                    agent.SetDestination(startPos + (Vector3.forward * range));
                    break;
                case 3:
                    agent.SetDestination(startPos + (Vector3.back * range));
                    break;
            }

            // set the difference
            Vector2 difference = new Vector2(1, 1);

            // when the spirit is close enough to the destination, continue. else wait. ps: I have to work on this
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
