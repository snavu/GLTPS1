using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform playerPosition;
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private Transform followPosition;
    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private float radius;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (Vector3.Distance(followPosition.position, transform.position) > radius)
        {
            destination.transform.position = followPosition.position;
        }
        if(Mathf.Approximately(playerMovementScript.speed, playerMovementScript.walkSpeed)){
            agent.speed = playerMovementScript.walkSpeed;
        }
        if(Mathf.Approximately(playerMovementScript.speed, playerMovementScript.runSpeed)){
            agent.speed = playerMovementScript.runSpeed;
        }
        agent.destination = destination.transform.position;
        CharacterMovementAnimation.Movement(GetComponent<Animator>(), agent.velocity, playerMovementScript.runSpeed);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(followPosition.position, radius);
    }
}
