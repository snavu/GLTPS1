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
    private bool entered;
    private bool exit;
    private float steer;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    private float elapsed = 0f;

    private bool preOrient;
    private Quaternion rotationDir;
    private float maxSpeed = 4.0f;
    private bool followPosition;
    private bool followRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerMovementScript.actions.Player.Interact.performed += Interact;
        playerMovementScript.actions.Vehicle.Exit.performed += Exit;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            CharacterMovementAnimation.Movement(playerAnim, agent.velocity, maxSpeed);
        }
        if (followPosition)
        {
            transform.position = vehicleSeat.position;
        }
        if (followRotation)
        {
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
            preOrient = false;
        }
        enterable = false;
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed && playerAnim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer"))
        {
            //disble vehicle movement
            playerMovementScript.actions.Vehicle.Disable();

            playerAnim.SetTrigger("ket exit");

        }
    }

    public void ExitVehicle()
    {
        Physics.IgnoreLayerCollision(6, 7, false);
        playerMovementScript.enabled = true;
        controller.enabled = true;
        preOrient = false;
    }

    void OnTriggerEnter(Collider other)
    {
        enterable = true;
    }
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Ket") && !preOrient && !enterable)
        {
            //orient position to enter ket
            if (agent.isActiveAndEnabled)
            {
                agent.destination = other.transform.position;
                if (transform.position == agent.destination)
                {
                    //orient rotation to enter ket
                    rotationDir = other.gameObject.transform.rotation * Quaternion.Euler(0, 90, 0);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDir, rotationSpeed * Time.deltaTime);
                    child.rotation = Quaternion.RotateTowards(child.rotation, rotationDir, rotationSpeed * Time.deltaTime);
                    if (other.gameObject.name == "Right" && transform.rotation == rotationDir && child.rotation == rotationDir)
                    {
                        agent.enabled = false;
                        playerAnim.SetTrigger("ket enter");
                        playerAnim.SetBool("ket right", true);
                        preOrient = true;
                    }
                }
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

    //animation events
    public void EnableVehicle()
    {
        playerMovementScript.actions.Vehicle.Enable();
    }
    public void DisableVehicle()
    {
        playerMovementScript.actions.Vehicle.Disable();

    }
    public void SetFollowPositionTrue()
    {
        followPosition = true;
    }
    public void SetFollowPositionFalse()
    {
        followPosition = false;
    }
    public void SetFollowRotationTrue()
    {
        followRotation = true;
    }
    public void SetFollowRotationFalse()
    {
        followRotation = false;
    }

}
