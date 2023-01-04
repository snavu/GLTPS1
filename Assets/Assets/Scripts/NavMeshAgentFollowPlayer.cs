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

        float distance = Vector3.Distance(followPosition.position, transform.position);

        //set destination position and agent speed to player run speed
        if (distance > radius)
        {
            destination.transform.position = followPosition.position;
            agent.speed = playerMovementScript.runSpeed;
        }

        //set agent speed to player walk speed
        if (Mathf.Approximately(playerMovementScript.speed, playerMovementScript.walkSpeed) && distance < radius)
        {
            agent.speed = playerMovementScript.walkSpeed;
        }

        //set agent 
        agent.destination = destination.transform.position;

        //set movement animation
        CharacterMovementAnimation.Movement(GetComponent<Animator>(), agent.velocity, playerMovementScript.runSpeed);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(followPosition.position, radius);
    }
}
