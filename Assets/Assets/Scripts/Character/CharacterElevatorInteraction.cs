using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;

public class CharacterElevatorInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private bool inElevatorInteractArea;
    [SerializeField] private Vector3 elevatorPosition;
    [SerializeField] private Animator elevatorAnim;
    [SerializeField] private GameObject chito;
    [SerializeField] private GameObject yuuri;
    private bool isInElevator;
    void OnEnable()
    {
        playerInputScript.actions.Player.Interact.performed += Interact;
    }
    void OnDisable()
    {
        playerInputScript.actions.Player.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && inElevatorInteractArea && elevatorAnim.GetBool("isGateOpen"))
        {
            elevatorAnim.SetBool("CloseGate", true);
            elevatorAnim.SetBool("isGateOpen", false);

            //disable navmesh agent follow
            chito.GetComponent<NavMeshAgentFollowPlayer>().follow = false;
            yuuri.GetComponent<NavMeshAgentFollowPlayer>().follow = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerInputScript.actions.Player.enabled)
        {
            if (other.gameObject.CompareTag("ElevatorTrigger"))
            {
                inElevatorInteractArea = true;
            }
            if (other.gameObject.CompareTag("Elevator"))
            {
                elevatorAnim = other.gameObject.GetComponent<Animator>();

                transform.SetParent(other.gameObject.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ElevatorTrigger"))
        {
            inElevatorInteractArea = false;
        }
        if (other.gameObject.CompareTag("Elevator"))
        {
            elevatorAnim = null;

            transform.parent = null;
        }
    }
}
