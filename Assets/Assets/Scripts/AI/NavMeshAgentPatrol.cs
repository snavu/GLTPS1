using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentPatrol : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private Animator agentAnim;
    [SerializeField] private GameObject destination;
    [SerializeField] private float angularSpeed = 800;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitDuration = 4f;
    private int index;
    private bool flag = false;

    public bool patrol = true;

    // Update is called once per frame
    void Start()
    {
        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            StartCoroutine(SetDestination());
        }
    }
    void Update()
    {
        // set agent movement anim
        CharacterMovementAnimation.Movement(agentAnim, agent.velocity, maxSpeed);

        // set desintation when arrived
        if (agent.isOnNavMesh && agent.isActiveAndEnabled && patrol)
        {
            if (Vector3.Distance(transform.position, patrolPoints[index].position) < 0.1f && !flag)
            {
                StartCoroutine(SetDestination());
                flag = true;
            }
        }
    }

    public IEnumerator SetDestination()
    {
        // wait at current destination
        yield return new WaitForSeconds(waitDuration);

        if (patrol)
        {
            // set next destination position
            index++;
            if (index == patrolPoints.Length)
            {
                index = 0;
            }

            agent.destination = patrolPoints[index].position;
            flag = false;
        }
    }
}
