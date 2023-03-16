using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPossession : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private PositionConstraint CMFollowTarget;
    [SerializeField]
    private PositionConstraint CMLookAtTarget;
    [SerializeField]
    private GameObject playerToPossess;
    [SerializeField]

    private void OnEnable()
    {
        //delay OnEnable to call after Awake function
        StartCoroutine(DelayOnEnable());
    }
    IEnumerator DelayOnEnable()
    {
        yield return new WaitForEndOfFrame();
        playerMovementScript.actions.Player.Possess.performed += Possess;
    }

    private void OnDisable()
    {
        playerMovementScript.actions.Player.Possess.performed -= Possess;
    }

    private void Possess(InputAction.CallbackContext context)
    {
        //change camera follow and look at target
        CMFollowTarget.player = playerToPossess.transform;
        CMLookAtTarget.player = playerToPossess.transform;

        //disable player scripts
        GetComponent<CharacterPossession>().enabled = false;

        //enable player-to-possess player scripts
        playerToPossess.GetComponent<CharacterPossession>().enabled = true;
    }
}
