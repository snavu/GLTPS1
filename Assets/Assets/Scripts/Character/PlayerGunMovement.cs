using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Cinemachine;
public class PlayerGunMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private CharacterManager characterManagerScript;
    [SerializeField] private Vector2 horizontalInput;
    [SerializeField] private Vector2 horizontalMovement;
    private Vector3 velocityXZ;
    private Vector2 smoothMovement;
    [SerializeField] private float smoothInputSpeed;
    [SerializeField] private float currentSpeed = 2f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator anim;

    [SerializeField] private float runSpeed = 4f;

    [SerializeField] private Transform gun;
    [SerializeField] private Transform backSocket;
    [SerializeField] private Transform frontSocket;
    private bool flagEquip = false;

    [SerializeField] private Transform camera;
    [SerializeField] private float rotationSpeed = 600.0f;
    [SerializeField] private Transform spineBone;

    [SerializeField] private Vector3 CMFollowTargetOffsetPos;
    [SerializeField] private Vector3 CMLookAtTargetOffsetPos;
    [SerializeField] private Vector3 CMFollowTargetOffsetPosRight;
    [SerializeField] private Vector3 CMLookAtTargetOffsetPosRight;
    [SerializeField] private Vector3 CMFollowTargetOffsetPosLeft;
    [SerializeField] private Vector3 CMLookAtTargetOffsetPosLeft;

    [SerializeField] private Transform CMFollowTarget;
    [SerializeField] private Transform CMLookAtTarget;
    private Vector3 CMFollowTargetInitialLocalPos;

    private Vector3 CMLookAtTargetInitialLocalPos;
    [SerializeField] private Transform child;

    [SerializeField] private GameObject crosshair;

    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float ADSDecelTime = 0.05f;
    [SerializeField] private float DefaultDecelTime = 0.3f;

    void Start()
    {
        CMFollowTargetInitialLocalPos = new Vector3(0, CMFollowTarget.transform.position.y, 0);
        CMLookAtTargetInitialLocalPos = new Vector3(0, CMLookAtTarget.transform.position.y, 0);
    }

    void Update()
    {
        //check if yuuri is the player  
        if (characterManagerScript.PlayerInputInitialize == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            playerMovementScript.isGrounded)
        {
            playerMovementScript.enabled = false;
        }

        if (characterManagerScript.PlayerInputInitialize == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            !playerMovementScript.enabled
            )
        {
            //get horizontal input
            horizontalInput = playerInputScript.actions.Player.Move.ReadValue<Vector2>();
            horizontalMovement = Vector2.SmoothDamp(horizontalMovement, horizontalInput, ref smoothMovement, smoothInputSpeed);

            velocityXZ = Vector3.ClampMagnitude(new Vector3(horizontalMovement.x, 0, horizontalMovement.y), 1.0f) * currentSpeed;
            velocityXZ = transform.TransformDirection(velocityXZ);

            //move character based on input
            controller.Move((velocityXZ) * Time.deltaTime);

            //pass velocityXZ to drive movement animation
            CharacterMovementAnimation.Movement(anim, velocityXZ, runSpeed);

            //rotate child in direction of camera      
            child.rotation = Quaternion.Euler(0f, camera.rotation.eulerAngles.y, 0f);

            //rotate player to camera
            Vector3 camXZ = new Vector3(camera.TransformDirection(Vector3.forward).x, 0, camera.TransformDirection(Vector3.forward).z);
            Quaternion rotationDirXZ = Quaternion.LookRotation(camXZ);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirXZ, rotationSpeed * Time.deltaTime);

            //set player ADS animation
            if (flagEquip)
            {
                //set camera position
                CMFollowTarget.localPosition = CMFollowTargetOffsetPos;
                CMLookAtTarget.localPosition = CMLookAtTargetOffsetPos;

                //set freelook camera values
                freeLookCamera.m_XAxis.m_DecelTime = ADSDecelTime;
                freeLookCamera.m_YAxis.m_DecelTime = ADSDecelTime;

                //set gun to hand
                StopAllCoroutines();
                AttachToGunFrontSocket();
                anim.SetBool("ADS", true);
                crosshair.SetActive(true);
                flagEquip = false;
            }
        }
        else
        {
            if (!flagEquip)
            {
                //reset camera position
                CMFollowTarget.localPosition = CMFollowTargetInitialLocalPos;
                CMLookAtTarget.localPosition = CMLookAtTargetInitialLocalPos;

                //set freelook camera values
                freeLookCamera.m_XAxis.m_DecelTime = DefaultDecelTime;
                freeLookCamera.m_YAxis.m_DecelTime = DefaultDecelTime;

                //set gun to back
                AttachToGunBackSocket();
                anim.SetBool("ADS", false);
                playerMovementScript.enabled = true;
                crosshair.SetActive(false);
                flagEquip = true;
            }
        }

        //switch shoulder camera
        if (playerInputScript.actions.Player.SwitchShoulder.ReadValue<float>() > 0)
        {
            CMFollowTargetOffsetPos = CMFollowTargetOffsetPosRight;
            CMLookAtTargetOffsetPos = CMLookAtTargetOffsetPosRight;
            //set camera position
            CMFollowTarget.localPosition = CMFollowTargetOffsetPos;
            CMLookAtTarget.localPosition = CMLookAtTargetOffsetPos;
        }
        else if (playerInputScript.actions.Player.SwitchShoulder.ReadValue<float>() < 0)
        {
            CMFollowTargetOffsetPos = CMFollowTargetOffsetPosLeft;
            CMLookAtTargetOffsetPos = CMLookAtTargetOffsetPosLeft;
            //set camera position
            CMFollowTarget.localPosition = CMFollowTargetOffsetPos;
            CMLookAtTarget.localPosition = CMLookAtTargetOffsetPos;
        }
    }

    void LateUpdate()
    {
        if (characterManagerScript.PlayerInputInitialize == playerInputScript &&
            playerInputScript.actions.Player.ADS.ReadValue<float>() > 0f &&
            !playerMovementScript.enabled
            )
        {
            spineBone.rotation = camera.rotation;
        }
    }
    public void AttachToGunBackSocket()
    {
        //set parent of gun to back
        gun.parent = backSocket;
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;
    }
    public void AttachToGunFrontSocket()
    {
        gun.parent = frontSocket;
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;
    }
}
