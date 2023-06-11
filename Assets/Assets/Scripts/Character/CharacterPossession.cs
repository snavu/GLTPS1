using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;

public class CharacterPossession : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private GameObject CMFollowTarget;
    [SerializeField] private GameObject CMLookAtTarget;
    [SerializeField] private GameObject playerToPossess;

    [SerializeField] private Vector3 CMFollowTargetOffsetPos;
    [SerializeField] private Vector3 CMLookAtTargetOffsetPos;

    [SerializeField] private PositionConstraint outOfBoundsBox;
    [SerializeField] private PositionConstraint skybox;
    [SerializeField] private PositionConstraint weatherParticleSystem;
    [SerializeField] private PositionConstraint mapCamera;

    [SerializeField] private NodeData nodeDataScript;

    private float footstepVolume;

    private ConstraintSource source;

    void OnEnable()
    {
        playerInputScript.actions.Player.Possess.performed += Possess;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Possess.performed -= Possess;
    }

    private void Possess(InputAction.CallbackContext context)
    {
        if (context.performed &&
            !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(1).IsTag("Ket") &&
            Time.timeScale == 1)
        {
            PossessCharacter(false);
        }
    }

    public void PossessCharacter(bool isDead)
    {
        nodeDataScript.player = playerToPossess;

        //switch the position contraints
        source.sourceTransform = playerToPossess.transform;
        source.weight = 1;

        outOfBoundsBox.SetSource(0, source);
        outOfBoundsBox.constraintActive = true;

        skybox.SetSource(0, source);
        skybox.constraintActive = true;

        weatherParticleSystem.SetSource(0, source);
        weatherParticleSystem.constraintActive = true;

        mapCamera.SetSource(0, source);
        mapCamera.constraintActive = true;

        //change camera follow and look at target
        CMFollowTarget.transform.parent = playerToPossess.transform;
        CMLookAtTarget.transform.parent = playerToPossess.transform;
        CMFollowTarget.transform.localPosition = CMFollowTargetOffsetPos;
        CMLookAtTarget.transform.localPosition = CMLookAtTargetOffsetPos;
        CMFollowTarget.transform.rotation = playerToPossess.transform.rotation;
        CMLookAtTarget.transform.rotation = playerToPossess.transform.rotation;

        //switch player scripts
        if (isDead)
        {
            GetComponentInChildren<CharacterController>().enabled = false;
            GetComponentInChildren<CapsuleCollider>().enabled = false;
            GetComponentInChildren<PlayerMovement>().enabled = false;
            GetComponentInChildren<CharacterPossession>().enabled = false;
            GetComponentInChildren<CharacterItemInteraction>().enabled = false;
            GetComponentInChildren<CharacterBarrelInteraction>().enabled = false;
            GetComponentInChildren<CharacterFuelInteraction>().enabled = false;
            GetComponentInChildren<CharacterVehicleInteraction>().enabled = false;
            GetComponentInChildren<NavMeshObstacle>().enabled = false;
            GetComponentInChildren<CharacterStatusScreenEffect>().enabled = false;
            GetComponentInChildren<NPCInteraction>().enabled = false;

        }
        else
        {
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
            GetComponentInChildren<CharacterStatusScreenEffect>().enabled = false;
            GetComponentInChildren<NPCInteraction>().enabled = false;
            footstepVolume = GetComponentInChildren<AudioSource>().volume;
            GetComponentInChildren<AudioSource>().volume = playerToPossess.GetComponentInChildren<AudioSource>().volume;
        }

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
        playerToPossess.GetComponentInChildren<CharacterStatusScreenEffect>().enabled = true;
        playerToPossess.GetComponentInChildren<NPCInteraction>().enabled = true;
        playerToPossess.GetComponentInChildren<AudioSource>().volume = footstepVolume;
    }
}
