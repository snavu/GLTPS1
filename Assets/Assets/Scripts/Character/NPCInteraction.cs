using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    private Collider other;
    private bool isInteractable;
    public bool triggerDialogue;
    public Animator dialogueBoxAnim;
    public CharacterPossession characterPossessionScript;
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
        if (context.performed && triggerDialogue)
        {
            other.gameObject.GetComponent<DialogueManager>().NextLine();
        }

        if (context.performed && isInteractable && !triggerDialogue && dialogueBoxAnim.GetCurrentAnimatorStateInfo(0).IsTag("Close"))
        {
            // stop agent patrol
            other.gameObject.GetComponent<NavMeshAgentPatrol>().patrol = false;

            // rotate agent to player
            other.gameObject.GetComponent<RotateToPlayer>().enabled = true;
            other.gameObject.GetComponent<RotateToPlayer>().player = transform;

            // set agent destination to in place
            other.gameObject.GetComponent<NavMeshAgentPatrol>().agent.destination = other.gameObject.transform.position;

            // trigger dialogue
            other.gameObject.GetComponent<DialogueManager>().enabled = true;
            other.gameObject.GetComponent<DialogueManager>().ShowDialogueBox();
            triggerDialogue = true;

            // disable switch character
            characterPossessionScript.enabled = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kanazawa"))
        {
            this.other = other;
            other.gameObject.GetComponent<DialogueManager>().NPCInteractionScript = this;
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Kanazawa"))
        {
            isInteractable = false;

            // start agent patrol
            other.gameObject.GetComponent<NavMeshAgentPatrol>().patrol = true;
            other.gameObject.GetComponent<RotateToPlayer>().enabled = false;
            StartCoroutine(other.gameObject.GetComponent<NavMeshAgentPatrol>().SetDestination());

            //reset dialogue
            if (!dialogueBoxAnim.GetCurrentAnimatorStateInfo(0).IsTag("Close"))
            {
                other.gameObject.GetComponent<DialogueManager>().HideDialogueBox();
                triggerDialogue = false;

                characterPossessionScript.enabled = true;
            }
        }
    }
}
