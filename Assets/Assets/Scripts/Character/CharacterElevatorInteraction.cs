using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;

public class CharacterElevatorInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize selfPlayerInputScript;
    [SerializeField] private PlayerInputInitialize otherPlayerInputScript;

    [SerializeField] private bool inElevatorInteractArea;
    [SerializeField] private Animator elevatorAnim;
    [SerializeField] private GameObject chito;
    [SerializeField] private GameObject yuuri;
    private bool flag;
    void OnEnable()
    {
        selfPlayerInputScript.actions.Player.Interact.performed += Interact;
    }
    void OnDisable()
    {
        selfPlayerInputScript.actions.Player.Interact.performed -= Interact;
    }

    void Update()
    {
        // re-enable interaction input when gate failed to close
        if (elevatorAnim != null)
        {
            if (elevatorAnim.enabled)
            {
                if (elevatorAnim.GetBool("isGateOpen"))
                {
                    selfPlayerInputScript.actions.Player.Interact.Enable();
                    otherPlayerInputScript.actions.Player.Interact.Enable();
                }
            }
        }
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

            //disable interaction input when closing gate
            selfPlayerInputScript.actions.Player.Interact.Disable();
            otherPlayerInputScript.actions.Player.Interact.Disable();

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (selfPlayerInputScript.actions.Player.enabled)
        {
            if (other.gameObject.CompareTag("ElevatorTrigger"))
            {
                inElevatorInteractArea = true;
            }
            if (other.gameObject.CompareTag("Elevator"))
            {
                transform.SetParent(other.gameObject.transform);
            }
        }
        if (other.gameObject.CompareTag("Elevator"))
        {
            elevatorAnim = other.gameObject.GetComponent<Animator>();
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
