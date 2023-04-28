using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentFollowPlayer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator agentAnim;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private Transform followPosition;
    [SerializeField] private GameObject destination;
    [SerializeField] private float radius;
    [SerializeField] private float angularSpeed = 120f;
    [SerializeField] private Transform child;

    void Update()
    {
        float distance = Vector3.Distance(followPosition.position, transform.position);

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.angularSpeed = angularSpeed;
            //set destination position 
            if (distance > radius)
            {
                destination.transform.position = followPosition.position;
            }

            //set agent speed to player walk speed
            if (Mathf.Approximately(playerMovementScript.currentSpeed, playerMovementScript.walkSpeed) && distance < radius)
            {
                agent.speed = playerMovementScript.walkSpeed;
            }
            //set agent speed to player run speed
            if (Mathf.Approximately(playerMovementScript.currentSpeed, playerMovementScript.runSpeed) || distance > radius + 0.5f)
            {
                agent.speed = playerMovementScript.runSpeed;
            }

            //set agent destination
            agent.destination = destination.transform.position;

            child.rotation = Quaternion.RotateTowards(child.rotation, transform.rotation, angularSpeed * Time.deltaTime);
            //set movement animation
            CharacterMovementAnimation.Movement(agentAnim, agent.velocity, playerMovementScript.runSpeed);
        }
    }

    float CalculateVelocity()
    {
        Vector3 previous;
        previous = transform.position;
        float velocity = (transform.position - previous).magnitude / Time.deltaTime;
        return velocity;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(followPosition.position, radius);
    }
}
