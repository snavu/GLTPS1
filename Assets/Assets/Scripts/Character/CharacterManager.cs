using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public Animator playerAnim;
    [SerializeField]
    private PlayerMovement playerChitoMovementScript;
    [SerializeField]

    private PlayerInput playerChitoInputScript;
    [SerializeField]
    private PlayerInput playerYuuriInputScript;
    [SerializeField]
    private Animator chitoAnim;
    [SerializeField]
    private Animator yuuriAnim;

    void Start()
    {
        playerInput = playerChitoInputScript;
        playerAnim = chitoAnim;
    }
    void Update()
    {
        if (playerChitoMovementScript.enabled)
        {
            playerInput = playerChitoInputScript;
            playerAnim = chitoAnim;
        }
        else
        {
            playerInput = playerYuuriInputScript;
            playerAnim = yuuriAnim;
        }
    }
}
