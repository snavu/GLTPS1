using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class CharacterVehicleInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
    private InputActions actions;
    [SerializeField]
    private Animator vehicleAnim;
    [SerializeField]
    private Animator playerAnim;
    private CharacterController controller;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform child;
    [SerializeField]
    private Transform vehicleSeat;
    private Transform vehicleBackSeat;
    private bool enterable;
    private bool exit;
    private float steer;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    private bool preOrient;
    private float elapsed = 0f;
    private Quaternion rotationDir;
    private float maxSpeed = 4.0f;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerMovementScript.actions.Player.Interact.performed += Interact;
    }

    void Update()
    {

        if(agent.isActiveAndEnabled){
            CharacterMovementAnimation.Movement(playerAnim, agent.velocity, maxSpeed); 
        }
        if (playerAnim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer"))
        {
            transform.position = vehicleSeat.position;
            transform.rotation = vehicleSeat.rotation;
        }
        if (playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back"))
        {

        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && enterable
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Front")
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back"))
        {
            //ignore collisions between layer 6 (vehicle) and layer 7 (player) 
            Physics.IgnoreLayerCollision(6, 7, true);

            //disable player movement
            playerMovementScript.enabled = false;
            //disable character controller
            controller.enabled = false;
            //enable navmesh agent 
            agent.enabled = true;
        }
        enterable = false;
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    void OnTriggerEnter(Collider other)
    {
        enterable = true;
    }
    void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Ket") && !preOrient)
        {
            if (agent.isActiveAndEnabled)
            {
                //set agent destination
                agent.destination = other.transform.position;

                //record rotation of child and disable navmesh agent when arrived at destination
                if (transform.position == agent.destination)
                {
                    agent.enabled = false;
                }
            }
            //orient rotation of partent to enter ket after arriving at destination
            if (!agent.isActiveAndEnabled && !enterable)
            {
                rotationDir = other.gameObject.transform.rotation * Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
            }

            if (other.gameObject.name == "Right" && transform.rotation == rotationDir)
            {
                playerAnim.SetTrigger("ket enter");
                playerAnim.SetBool("ket right", true);
                preOrient = true;
            }
            if (other.gameObject.name == "Left")
            {

            }
            if (other.gameObject.name == "Back")
            {

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        enterable = false;
    }

}
