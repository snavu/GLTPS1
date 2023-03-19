using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class CharacterPossession : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInputScript;
    [SerializeField]
    private PositionConstraint CMFollowTarget;
    [SerializeField]
    private PositionConstraint CMLookAtTarget;
    [SerializeField]
    private GameObject playerToPossess;
    private static bool possessActionSubscribed = false;

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
            CMFollowTarget.player = playerToPossess.transform;
            CMLookAtTarget.player = playerToPossess.transform;

            //switch player scripts
            GetComponentInChildren<CharacterController>().enabled = false;
            GetComponentInChildren<CapsuleCollider>().enabled = true;
            GetComponentInChildren<PlayerMovement>().enabled = false;
            GetComponentInChildren<CharacterPossession>().enabled = false;
            GetComponentInChildren<CharacterFoodManager>().enabled = false;
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
            playerToPossess.GetComponentInChildren<CharacterFoodManager>().enabled = true;
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
