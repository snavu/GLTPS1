using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    public InputActions actions;

    void Awake()
    {
        actions = new InputActions();
        actions.Player.Enable();

    }
}
