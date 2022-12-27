using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float jumpheight = 1.0f;
    [SerializeField]
    private float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //set is grounded
        isGrounded = Physics.Raycast(transform.position, new Vector3(0,-1,0), controller.skinWidth + 0.001f);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        else{
            velocity.y += gravity * Time.deltaTime;
        }
        
        //set movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), velocity.y, Input.GetAxis("Vertical"));
        controller.Move(move * speed * Time.deltaTime);

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -3.0f * gravity);
        }
    }
}
