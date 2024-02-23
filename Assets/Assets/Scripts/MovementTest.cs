using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementTest : MonoBehaviour
{
    public InputManager inputManager;
    public Vector2 horizontalInput;
    public float moveSpeed;

    public CharacterController cc;

    public Transform cam;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        inputManager.actions.Player.Jump.performed += Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) { }
    }

    // Update is called once per frame
    void Update() {
        //read horizontal input
        horizontalInput = inputManager.actions.Player.Move.ReadValue<Vector2>();

        //map xy of horizontal input to xz move vector 
        Vector3 moveDir = new Vector3(horizontalInput.x, 0, horizontalInput.y);
        //move vector with respect to rotation of transform
        moveDir = transform.TransformDirection(moveDir);
        cc.Move(moveDir * moveSpeed * Time.deltaTime);
        
        //rotates transform to direction of camera
        Vector3 camDir = new Vector3(cam.TransformDirection(Vector3.forward).x, 0, cam.TransformDirection(Vector3.forward).z);
        Quaternion camRot = Quaternion.LookRotation(camDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, camRot, rotSpeed * Time.deltaTime);

    }
}
