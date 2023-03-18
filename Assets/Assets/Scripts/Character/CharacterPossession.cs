using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
        playerInputScript.actions.Player.Possess.performed += Possess;
    }

    private void Possess(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //change camera follow and look at target
            CMFollowTarget.player = playerToPossess.transform;
            CMLookAtTarget.player = playerToPossess.transform;

            //disable player scripts
            GetComponentInChildren<CharacterPossession>().enabled = false;
            GetComponentInChildren<PlayerMovement>().enabled = true;


            //enable player-to-possess player scripts
            playerToPossess.GetComponentInChildren<CharacterPossession>().enabled = true;
            playerToPossess.GetComponentInChildren<PlayerMovement>().enabled = true;
        }

    }
}
