using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActions actions;

    // Start is called before the first frame update
    void Awake()
    {
        actions = new InputActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }
}