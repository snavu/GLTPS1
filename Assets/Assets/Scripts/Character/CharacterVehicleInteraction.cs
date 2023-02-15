using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Cinemachine;
public class CharacterVehicleInteraction : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook freelook;
    [SerializeField]
    private float[] freelookRadius;
    [SerializeField]
    private float smoothRadiusSpeed = 0.2f;
    private float smoothRadiusVelocity = 0f;
    [SerializeField]
    private PlayerMovement playerMovementScript;

    [SerializeField]
    private Animator vehicleAnim;
    [SerializeField]
    private Animator playerAnim;
    public CharacterController controller;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform child;
    [SerializeField]
    private Transform vehicleSeat;
    private bool enterable;
    private float steer;
    private float steerSmooth = 0f;
    [SerializeField]
    private float smoothInputSpeed = 0.2f;
    private float smoothInputVelocity = 0f;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    public float elapsed = 0f;

    public bool preOrientEnter = false;
    public bool preOrientExit = false;
    private float maxSpeed = 4.0f;
    public bool constraint;

    [SerializeField]
    private Transform vehicleRightExit;

    [SerializeField]
    private Transform vehicleLeftExit;
    public bool exitRight;
    public bool exitLeft;
    [SerializeField]
    private float exitDuration = 0.25f;
    [SerializeField]
    private float enterDuration = 1.0f;

    private Transform vehicleEnter;


    void Start()
    {
        Physics.IgnoreLayerCollision(6, 7, false);
        Physics.IgnoreLayerCollision(6, 8, false);

        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerMovementScript.actions.Player.Interact.performed += Interact;
        playerMovementScript.actions.Vehicle.Exit.performed += Exit;
    }

    void Update()
    {

        if (constraint)
        {
            transform.position = vehicleSeat.position;
            transform.rotation = vehicleSeat.rotation;
            ChangeCameraRigRadius(freelookRadius[0], freelookRadius[1], freelookRadius[2]);
        }
        else if (freelook.m_Orbits[1].m_Radius != freelookRadius[4])
        {
            ChangeCameraRigRadius(freelookRadius[3], freelookRadius[4], freelookRadius[5]);
        }

        if (preOrientEnter)
        {
            if (transform.position != vehicleSeat.position)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, vehicleSeat.position, elapsed / enterDuration);
                transform.rotation = vehicleSeat.rotation;
            }
            else
            {
                preOrientEnter = false;
                elapsed = 0f;
            }

        }
        if (preOrientExit)
        {

            if (transform.position != vehicleRightExit.position && exitRight)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(vehicleSeat.position, vehicleRightExit.position, elapsed / exitDuration);
            }
            else
            {
                StartCoroutine(Wait(exitDuration));
            }

            if (transform.position != vehicleLeftExit.position && exitLeft)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(vehicleSeat.position, vehicleLeftExit.position, elapsed / exitDuration);
            }
            else
            {
                StartCoroutine(Wait(exitDuration));
            }
        }

        if (playerMovementScript.actions.Vehicle.Drive.enabled)
        {
            steer = playerMovementScript.actions.Vehicle.Drive.ReadValue<Vector2>().x;
            steerSmooth = Mathf.SmoothDamp(steerSmooth, steer, ref smoothInputVelocity, smoothInputSpeed);
            vehicleAnim.SetFloat("steer", steerSmooth);
            playerAnim.SetFloat("steer", steerSmooth);
        }

        if (agent.isActiveAndEnabled)
        {
            CharacterMovementAnimation.Movement(playerAnim, agent.velocity, maxSpeed);
        }

        //orient position to enter ket
        if (agent.isActiveAndEnabled)
        {
            agent.destination = vehicleEnter.position;
            if (Vector3.Distance(transform.position, vehicleEnter.position) < 0.01f)
            {
                //orient rotation to enter ket
                transform.rotation = Quaternion.RotateTowards(transform.rotation, vehicleEnter.rotation, rotationSpeed * Time.deltaTime);
                child.rotation = Quaternion.RotateTowards(child.rotation, vehicleEnter.gameObject.transform.rotation, rotationSpeed * Time.deltaTime);
                if (transform.rotation == vehicleEnter.rotation && child.rotation == vehicleEnter.rotation)
                {
                    if (vehicleEnter.gameObject.name == "Right")
                    {
                        playerAnim.SetBool("ket right", true);
                    }
                    if (vehicleEnter.gameObject.name == "Left")
                    {
                        playerAnim.SetBool("ket left", true);
                    }
                    agent.enabled = false;
                    preOrientEnter = true;
                }
            }
        }


    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        Physics.IgnoreLayerCollision(6, 7, false);
        playerMovementScript.enabled = true;
        controller.enabled = true;
        preOrientExit = false;
        exitLeft = false;
        exitRight = false;
        elapsed = 0f;
    }

    private void ChangeCameraRigRadius(float top, float middle, float bottom)
    {
        freelook.m_Orbits[0].m_Radius = Mathf.SmoothDamp(freelook.m_Orbits[0].m_Radius, top, ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelook.m_Orbits[1].m_Radius = Mathf.SmoothDamp(freelook.m_Orbits[1].m_Radius, top, ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelook.m_Orbits[2].m_Radius = Mathf.SmoothDamp(freelook.m_Orbits[2].m_Radius, top, ref smoothRadiusVelocity, smoothRadiusSpeed);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && enterable
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Front")
            && !playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back"))
        {
            //ignore collisions between layer 6 (vehicle) and layer 7 (player) 
            Physics.IgnoreLayerCollision(6, 7, true);

            //disable player movement
            playerMovementScript.enabled = false;
            //disable character controller
            controller.enabled = false;
            //enable navmesh agent 
            agent.enabled = true;
            preOrientEnter = false;
            enterable = false;
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed && playerAnim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer"))
        {
            playerAnim.SetTrigger("ket exit");
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ket"))
        {
            vehicleEnter = other.transform;
            enterable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ket"))
        {
            enterable = false;
        }
    }
}
