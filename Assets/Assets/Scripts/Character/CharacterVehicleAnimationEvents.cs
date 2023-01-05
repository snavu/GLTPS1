using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVehicleAnimationEvents : MonoBehaviour
{

    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private CharacterVehicleInteraction characterVehicleInteractionScript;
    [SerializeField]
    private NavMeshAgentVehicleInteraction navMeshAgentVehicleInteractionScript;

    //animation events
    public void EnableVehicle()
    {
        playerMovementScript.actions.Vehicle.Enable();

    }
    public void DisableVehicle()
    {
        playerMovementScript.actions.Vehicle.Disable();
    }

    public void ExitVehicle()
    {
        characterVehicleInteractionScript.preOrientExit = true;
    }
    public void SetConstraintTrue()
    {
        characterVehicleInteractionScript.constraint = true;
    }
    public void SetConstraintFalse()
    {
        characterVehicleInteractionScript.constraint = false;
    }

    public void ExitRight()
    {
        characterVehicleInteractionScript.exitRight = true;
    }
    public void ExitLeft()
    {
        characterVehicleInteractionScript.exitLeft = true;
    }
    public void AgentEnter()
    {
        navMeshAgentVehicleInteractionScript.enter = true;
    }
    public void AgentExitBack()
    {
        navMeshAgentVehicleInteractionScript.exit = true;
        navMeshAgentVehicleInteractionScript.agentAnim.SetTrigger("ket exit");

    }
    public void AgentExitVehicle()
    {
        navMeshAgentVehicleInteractionScript.preOrientExit = true;
    }
    public void AgentRotationConstraint()
    {
        navMeshAgentVehicleInteractionScript.rotationConstraint = true;
    }

}
