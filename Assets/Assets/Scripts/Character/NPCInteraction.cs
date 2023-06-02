using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;

    private Collider other;
    private bool isInteractable;

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
        if (context.performed && isInteractable)
        {
            // stop agent patrol
            other.gameObject.GetComponent<NavMeshAgentPatrol>().patrol = false;
            StopCoroutine(other.gameObject.GetComponent<NavMeshAgentPatrol>().SetDestination());

            // rotate agent to player
            other.gameObject.GetComponent<RotateToPlayer>().enabled = true;
            other.gameObject.GetComponent<RotateToPlayer>().player = transform;

            //set agent destination to in place
            other.gameObject.GetComponent<NavMeshAgentPatrol>().agent.destination = other.gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kanazawa"))
        {
            this.other = other;
            isInteractable = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Kanazawa"))
        {
            isInteractable = false;

            // start agent patrol
            other.gameObject.GetComponent<RotateToPlayer>().enabled = false;
            other.gameObject.GetComponent<NavMeshAgentPatrol>().patrol = true;
            StartCoroutine(other.gameObject.GetComponent<NavMeshAgentPatrol>().SetDestination());
        }
    }
}
