using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private Collider vehicleCollider;
    [SerializeField]
    private Transform vehicleSeat;
    private Transform vehicleBackSeat;
    private bool enter;
    private bool exit;
    private float steer;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerMovementScript.actions.Player.Interact.performed += Interact;
    }

    void Update()
    {

        if (playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Front"))
        {
            transform.position = vehicleSeat.position;
            transform.rotation = vehicleSeat.rotation;
        }
        if (playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back"))
        {

        }

    }

    void OnTriggerStay(Collider other)
    {
        if (enter && !exit)
        {
            if (other.gameObject.CompareTag("Ket Right"))
            {

            }
            if (other.gameObject.CompareTag("Ket Left"))
            {

            }
            if (other.gameObject.CompareTag("Ket Back"))
            {

            }
        }

        enter = false;

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Front")
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back"))
        {
            //ignore collisions between layer 6 (vehicle) and layer 7 (player) 
            Physics.IgnoreLayerCollision(6, 7, true);
            playerMovementScript.actions.Vehicle.Enable();
            playerMovementScript.actions.Player.Disable();
            enter = true;
            exit = false;
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            enter = false;
            exit = true;
        }
    }

    public void EnterSeat()
    {

    }
}
