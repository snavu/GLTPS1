using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class NavMeshAgentCommand : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    public NavMeshAgentFollowPlayer navMeshAgentFollowPlayerScript;

    void OnEnable()
    {
        playerInputScript.actions.Player.AgentWait.performed += AgentWait;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.AgentWait.performed -= AgentWait;
    }

    private void AgentWait(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (navMeshAgentFollowPlayerScript.follow)
            {
                navMeshAgentFollowPlayerScript.follow = false;
            }
            else
            {
                navMeshAgentFollowPlayerScript.follow = true;
            }
        }
    }
}
