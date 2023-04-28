using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator agentAnim;
    [SerializeField] private GameObject destination;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private float angularSpeed = 800;

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            //set agent destination
            agent.destination = destination.transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation, angularSpeed * Time.deltaTime);
            
            //set movement animation
            CharacterMovementAnimation.Movement(agentAnim, agent.velocity, playerMovementScript.runSpeed);
        
        }
    }
}
