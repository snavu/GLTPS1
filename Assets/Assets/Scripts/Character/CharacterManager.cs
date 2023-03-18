using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerChitoMovementScript;
    [SerializeField]

    private PlayerInput playerChitoInputScript;
    [SerializeField]
    private PlayerInput playerYuuriInputScript;

    void Update()
    {
        //get input
        if (playerChitoMovementScript.enabled)
        {
            playerInput = playerChitoInputScript;
        }
        else
        {
            playerInput = playerYuuriInputScript;
        }
    }
}
