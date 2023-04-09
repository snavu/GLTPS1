using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputInitialize : MonoBehaviour
{
    public InputActions actions;

    void Awake()
    {
        actions = new InputActions();
        if (gameObject.name == "ChitoParent")
        {
            actions.Player.Enable();
        }
        else if (gameObject.name == "YuuriParent")
        {
            actions.Player.Disable();
        }
    }
}
