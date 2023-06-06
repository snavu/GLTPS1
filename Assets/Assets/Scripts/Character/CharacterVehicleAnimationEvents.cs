using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVehicleAnimationEvents : MonoBehaviour
{

    [SerializeField] private PlayerInputInitialize playerInputScript;

    [SerializeField] private CharacterVehicleInteraction characterVehicleInteractionScript;
    [SerializeField] private NavMeshAgentVehicleInteraction navMeshAgentVehicleInteractionScript;
    [SerializeField] private NavMeshAgentVehicleInteraction navMeshAgentSelfVehicleInteractionScript;
    [SerializeField] private VehicleMovement vehicleMovementScript;
    [SerializeField] private VehicleFuelManager vehicleFuelManagerScript;

    [SerializeField] private AudioSource vehicleAudioSource;


    //animation events
    public void EnableVehicle()
    {
        vehicleMovementScript.playerInputScript = playerInputScript;
        vehicleMovementScript.playerInputScript = playerInputScript;
        playerInputScript.actions.Vehicle.Enable();
        vehicleAudioSource.Play();
    }
    public void DisableVehicle()
    {
        playerInputScript.actions.Vehicle.Disable();
        vehicleAudioSource.Stop();
    }

    public void ExitVehicle()
    {
        characterVehicleInteractionScript.preOrientExit = true;
    }
    public void SetConstraintTrue()
    {
        characterVehicleInteractionScript.constraint = true;
        characterVehicleInteractionScript.changeCameraRadiusFlag1 = true;
    }
    public void SetConstraintFalse()
    {
        characterVehicleInteractionScript.constraint = false;
        characterVehicleInteractionScript.changeCameraRadiusFlag2 = true;
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
        navMeshAgentSelfVehicleInteractionScript.preOrientExit = true;
    }
}
