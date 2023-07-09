using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentVehicleInteraction : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public Animator agentAnim;
    [SerializeField] private NavMeshAgentFollowPlayer navMeshAgentFollowPlayerScript;
    [SerializeField] private Transform child;
    [SerializeField] private Transform enterPosition;
    [SerializeField] private Transform constraintPosition;
    [SerializeField] private PlayerMovement playerMovementScript;
    public bool enter;
    public bool exit;
    private bool positionConstraint;
    public bool rotationConstraint;
    private bool preOrientEnter;
    public bool preOrientExit;
    [SerializeField] private float rotationSpeed = 600.0f;
    private float elapsed = 0f;
    [SerializeField] private float duration = 0.25f;

    [SerializeField] private CharacterDeath characterDeathScript;

    void Update()
    {
        CharacterMovementAnimation.Movement(agentAnim, agent.velocity, playerMovementScript.runSpeed);

        if (constraintPosition != null && enterPosition != null) 
        {
            if (enter && !preOrientExit)
            {
                navMeshAgentFollowPlayerScript.enabled = false;
                //orient agent position to enter ket
                if (agent.enabled)
                {
                    agent.destination = enterPosition.position;
                }
                if (Vector3.Distance(transform.position, enterPosition.position) < 0.2f)
                {
                    //orient rotation to enter ket
                    agent.enabled = false;
                    child.rotation = Quaternion.RotateTowards(child.rotation, enterPosition.rotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, enterPosition.rotation, rotationSpeed * Time.deltaTime);
                    if (AngleUtils.RotationsApproximatelyEqual(transform, enterPosition) && AngleUtils.RotationsApproximatelyEqual(child, enterPosition))
                    {
                        agentAnim.SetTrigger("ket back");
                        preOrientEnter = true;
                        enter = false;
                    }
                }
            }

            //lerp agent position to vehicle

            if (preOrientEnter)
            {
                if (Vector3.Distance(transform.position, constraintPosition.position) > 0.01f)
                {
                    rotationConstraint = true;

                    elapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, constraintPosition.position, elapsed / duration);
                }
                else
                {
                    positionConstraint = true;
                    preOrientEnter = false;
                    elapsed = 0f;
                }
            }

            if (exit)
            {
                positionConstraint = false;
                rotationConstraint = false;

                //lerp agent position to exit
                if (preOrientExit)
                {
                    if (Vector3.Distance(transform.position, enterPosition.position) > 0.1f && !agent.enabled)
                    {
                        elapsed += Time.deltaTime;
                        transform.position = Vector3.Lerp(transform.position, enterPosition.position, elapsed / duration);
                    }
                    else
                    {
                        agent.enabled = true;
                        navMeshAgentFollowPlayerScript.enabled = true;
                        preOrientExit = false;

                        exit = false;
                        elapsed = 0f;
                    }
                }
            }

            if (positionConstraint)
            {
                transform.position = constraintPosition.position;
            }
            if (rotationConstraint)
            {
                transform.rotation = constraintPosition.rotation;
            }
        }
    }
}
