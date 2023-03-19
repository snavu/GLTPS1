using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public Animator playerAnim;

    [SerializeField]
    private GameObject Chito;
    [SerializeField]
    private GameObject Yuuri;
    private bool allowYuuriPossession = true;

    void Start()
    {
        Chito.GetComponent<PlayerInput>().actions.Player.Possess.performed += Possess;

        playerInput = Chito.GetComponent<PlayerInput>();
        playerAnim = Chito.GetComponentInChildren<Animator>();
    }
    private void Possess(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (allowYuuriPossession)
            {
                playerInput = Yuuri.GetComponent<PlayerInput>();
                playerAnim = Yuuri.GetComponentInChildren<Animator>();
                Yuuri.tag = "Player";
                Chito.tag = "Untagged";
                allowYuuriPossession = false;
            }
            else
            {
                playerInput = Chito.GetComponent<PlayerInput>();
                playerAnim = Chito.GetComponentInChildren<Animator>();
                Yuuri.tag = "Untagged";
                Chito.tag = "Player";
                allowYuuriPossession = true;
            }
        }
    }
}
