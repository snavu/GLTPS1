using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    public PlayerInputInitialize PlayerInputInitialize;
    public Animator playerAnim;

    [SerializeField]
    private GameObject Chito;
    [SerializeField]
    private GameObject Yuuri;
    private bool allowYuuriPossession = true;

    void Start()
    {
        Chito.GetComponent<PlayerInputInitialize>().actions.Player.Possess.performed += Possess;

        PlayerInputInitialize = Chito.GetComponent<PlayerInputInitialize>();
        playerAnim = Chito.GetComponentInChildren<Animator>();
    }
    private void Possess(InputAction.CallbackContext context)
    {
        if (context.performed 
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket")
            && !playerAnim.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(2).IsTag("Carry"))
        {
            if (allowYuuriPossession)
            {
                PlayerInputInitialize = Yuuri.GetComponent<PlayerInputInitialize>();
                playerAnim = Yuuri.GetComponentInChildren<Animator>();
                Yuuri.tag = "Player";
                Chito.tag = "Untagged";
                allowYuuriPossession = false;
            }
            else
            {
                PlayerInputInitialize = Chito.GetComponent<PlayerInputInitialize>();
                playerAnim = Chito.GetComponentInChildren<Animator>();
                Yuuri.tag = "Untagged";
                Chito.tag = "Player";
                allowYuuriPossession = true;
            }
        }
    }
}
