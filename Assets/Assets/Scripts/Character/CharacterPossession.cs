using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class CharacterPossession : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private GameObject CMFollowTarget;
    [SerializeField] private GameObject CMLookAtTarget;
    [SerializeField] private GameObject playerToPossess;

    [SerializeField] private Vector3 CMFollowTargetOffsetPos;
    [SerializeField] private Vector3 CMLookAtTargetOffsetPos;

    void OnEnable()
    {
        //subscribe the input action
        StartCoroutine(DelayOnEnable());
    }

    IEnumerator DelayOnEnable()
    {
        yield return new WaitForEndOfFrame();
        playerInputScript.actions.Player.Possess.performed += Possess;
    }

    private void Possess(InputAction.CallbackContext context)
    {
        if (context.performed
            && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Ket")
            && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(2).IsTag("Carry"))
        {
            //note: input action callbacks occur even while script is inactive 
            //unsubscribe from the input action before switching script to prevent duplicate calls
            playerInputScript.actions.Player.Possess.performed -= Possess;

            //change camera follow and look at target
            CMFollowTarget.transform.parent = playerToPossess.transform;
            CMLookAtTarget.transform.parent = playerToPossess.transform;
            CMFollowTarget.transform.localPosition = CMFollowTargetOffsetPos;
            CMLookAtTarget.transform.localPosition = CMLookAtTargetOffsetPos;
            CMFollowTarget.transform.rotation = playerToPossess.transform.rotation;
            CMLookAtTarget.transform.rotation = playerToPossess.transform.rotation;


            //switch player scripts
            GetComponentInChildren<CharacterController>().enabled = false;
            GetComponentInChildren<CapsuleCollider>().enabled = true;
            GetComponentInChildren<PlayerMovement>().enabled = false;
            GetComponentInChildren<CharacterPossession>().enabled = false;
            GetComponentInChildren<CharacterItemInteraction>().enabled = false;
            GetComponentInChildren<CharacterBarrelInteraction>().enabled = false;
            GetComponentInChildren<CharacterFuelInteraction>().enabled = false;
            GetComponentInChildren<CharacterVehicleInteraction>().enabled = false;
            GetComponentInChildren<NavMeshObstacle>().enabled = false;
            GetComponentInChildren<NavMeshAgent>().enabled = true;
            GetComponentInChildren<NavMeshAgentVehicleInteraction>().enabled = true;
            GetComponentInChildren<NavMeshAgentFollowPlayer>().enabled = true;

            //switch player-to-possess player scripts
            playerToPossess.GetComponentInChildren<CharacterController>().enabled = true;
            playerToPossess.GetComponentInChildren<CapsuleCollider>().enabled = false;
            playerToPossess.GetComponentInChildren<PlayerMovement>().enabled = true;
            playerToPossess.GetComponentInChildren<CharacterPossession>().enabled = true;
            playerToPossess.GetComponentInChildren<CharacterItemInteraction>().enabled = true;
            playerToPossess.GetComponentInChildren<CharacterBarrelInteraction>().enabled = true;
            playerToPossess.GetComponentInChildren<CharacterFuelInteraction>().enabled = true;
            playerToPossess.GetComponentInChildren<CharacterVehicleInteraction>().enabled = true;
            playerToPossess.GetComponentInChildren<NavMeshAgent>().enabled = false;
            playerToPossess.GetComponentInChildren<NavMeshObstacle>().enabled = true;
            playerToPossess.GetComponentInChildren<NavMeshAgentVehicleInteraction>().enabled = false;
            playerToPossess.GetComponentInChildren<NavMeshAgentFollowPlayer>().enabled = false;
        }
    }
}
