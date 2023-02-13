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

    [Header("Detector")]
    public float range;
    public float destinationOffset;
    public int dir;

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

        startPos = new Vector3(transform.position.x,0f, transform.position.z); // get the players start pos
        StartCoroutine(Roam()); // start roaming
    }

    void Update()
    {
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }

    IEnumerator Roam()
    {
        //while alive
        while (isAlive)
        {
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
            Vector2 difference = new Vector2(1,1);

            // when the spirit is close enough to the destination, continue. else wait. ps: I have to work on this
            while(difference.x > destinationOffset ||
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
