using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentVehicleInteraction : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private NavMeshAgentFollowPlayer navMeshAgentFollowPlayerScript;
    [SerializeField]
    private Transform enterPosition;
    [SerializeField]
    private Transform constraintPosition;
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    public Animator agentAnim;
    public bool enter;
    public bool exit;
    private bool constraint;
    private bool preOrientEnter;
    public bool preOrientExit;
    [SerializeField]
    private float rotationSpeed = 600.0f;
    private float elapsed = 0f;
    [SerializeField]
    private float duration = 0.25f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navMeshAgentFollowPlayerScript = GetComponent<NavMeshAgentFollowPlayer>();
        agentAnim = GetComponent<Animator>();
    }


    void Update()
    {
        CharacterMovementAnimation.Movement(GetComponent<Animator>(), agent.velocity, playerMovementScript.runSpeed);

        if (enter)
        {
            navMeshAgentFollowPlayerScript.enabled = false;
            //ignore collisions between layer 6 (vehicle) and layer 8 (AI) 
            Physics.IgnoreLayerCollision(6, 8, true);

            //orient agent position to enter ket
            agent.destination = enterPosition.position;
            if (Vector3.Distance(transform.position, enterPosition.position) < 0.01f)
            {
                //orient rotation to enter ket
                transform.rotation = Quaternion.RotateTowards(transform.rotation, enterPosition.rotation, rotationSpeed * Time.deltaTime);
                if (transform.rotation == enterPosition.rotation)
                {
                    agentAnim.SetTrigger("ket back");
                    agent.enabled = false;
                    preOrientEnter = true;
                    enter = false;
                }
            }
        }

        if (preOrientEnter)
        {
            if (transform.position != constraintPosition.position)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, constraintPosition.position, elapsed / duration);
            }
            else
            {
                constraint = true;
                preOrientEnter = false;
                elapsed = 0f;
            }
        }

        if (exit)
        {
            constraint = false;

            if (preOrientExit)
            {
                if (transform.position != enterPosition.position)
                {
                    elapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, enterPosition.position, elapsed / duration);
                }
                else
                {
                    agent.enabled = true;

                    navMeshAgentFollowPlayerScript.enabled = true;
                    Physics.IgnoreLayerCollision(6, 8, false);
                    preOrientExit = false;
                    exit = false;

                    elapsed = 0f;
                }
            }

        }

        if (constraint)
        {
            transform.position = constraintPosition.position;
            transform.rotation = constraintPosition.rotation;
        }
    }
}
