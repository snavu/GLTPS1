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
    private PlayerInput playerInputScript;
    [SerializeField]
    private CharacterManager characterManagerScript;
    [SerializeField]
    private Animator vehicleAnim;
    public CharacterController controller;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float angularSpeed = 800f;
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

        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Vehicle.Exit.performed += Exit;
    }

    void Update()
    {
        //constrain player to vehicle, increase camera radius for vehicle movement
        if (constraint)
        {
            transform.position = vehicleSeat.position;
            transform.rotation = vehicleSeat.rotation;
            ChangeCameraRigRadius(freelookRadius[0], freelookRadius[1], freelookRadius[2]);
        }
        //constrain is false, lerp camera radius back normal radius for player movement
        else if (freelook.m_Orbits[1].m_Radius != freelookRadius[4])
        {
            ChangeCameraRigRadius(freelookRadius[3], freelookRadius[4], freelookRadius[5]);
        }

        //lerp player position to vehicle
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

        //lerp player position to exit vehicle
        if (preOrientExit)
        {
            //exit to the right
            if (transform.position != vehicleRightExit.position && exitRight)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(vehicleSeat.position, vehicleRightExit.position, elapsed / exitDuration);
            }
            else
            {
                //wait for duration of exiting vehicle before reenabling player movement and resetting flags
                StartCoroutine(DelayExit(exitDuration));
            }

            //exit to the left 
            if (transform.position != vehicleLeftExit.position && exitLeft)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(vehicleSeat.position, vehicleLeftExit.position, elapsed / exitDuration);
            }
            else
            {
                StartCoroutine(DelayExit(exitDuration));
            }
        }

        //vehicle movement controls
        if (characterManagerScript.playerInput.actions.Vehicle.Drive.enabled)
        {
            steer = characterManagerScript.playerInput.actions.Vehicle.Drive.ReadValue<Vector2>().x;
            steerSmooth = Mathf.SmoothDamp(steerSmooth, steer, ref smoothInputVelocity, smoothInputSpeed);
            vehicleAnim.SetFloat("steer", steerSmooth);
            characterManagerScript.playerAnim.SetFloat("steer", steerSmooth);
        }

        //handle player movement animation when navmesh agent takes over to position player in correct position to enter vehichle
        if (agent.isActiveAndEnabled)
        {
            CharacterMovementAnimation.Movement(characterManagerScript.playerAnim, agent.velocity, maxSpeed);
        }

        //orient position to enter ket
        if (agent.isActiveAndEnabled && !GetComponent<NavMeshObstacle>().enabled)
        {
            agent.angularSpeed = angularSpeed;
            agent.destination = vehicleEnter.position;
            if (Vector3.Distance(transform.position, vehicleEnter.position) < 0.1f)
            {
                //orient rotation to enter ket
                transform.rotation = Quaternion.RotateTowards(transform.rotation, vehicleEnter.rotation, rotationSpeed * Time.deltaTime);
                child.rotation = Quaternion.RotateTowards(child.rotation, vehicleEnter.gameObject.transform.rotation, rotationSpeed * Time.deltaTime);
                if (transform.rotation == vehicleEnter.rotation && child.rotation == vehicleEnter.rotation)
                {
                    if (vehicleEnter.gameObject.name == "Right")
                    {
                        characterManagerScript.playerAnim.SetBool("ket right", true);
                    }
                    if (vehicleEnter.gameObject.name == "Left")
                    {
                        characterManagerScript.playerAnim.SetBool("ket left", true);
                    }
                    agent.enabled = false;
                    preOrientEnter = true;
                }
            }
        }


    }

    IEnumerator DelayExit(float duration)
    {
        yield return new WaitForSeconds(duration);
        Physics.IgnoreLayerCollision(6, 7, false);
        characterManagerScript.playerInput.actions.Player.Enable();
        controller.enabled = true;
        preOrientExit = false;
        exitLeft = false;
        exitRight = false;
        elapsed = 0f;
    }

    IEnumerator DelayEnter()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
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
            && !characterManagerScript.playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Front")
            && !characterManagerScript.playerAnim.GetCurrentAnimatorStateInfo(1).IsTag("Ket Seat Back")
            && !GetComponent<CharacterBarrelInteraction>().isCarrying)
        {
            //ignore collisions between layer 6 (vehicle) and layer 7 (player) 
            Physics.IgnoreLayerCollision(6, 7, true);

            //undo constraints set for freezing kettengrad rigidbody to prevent movement from player collider clipping bug
            GetComponent<CharacterBarrelInteraction>().vehicleRigidbody.constraints = RigidbodyConstraints.None;

            //disable player movement
            characterManagerScript.playerInput.actions.Player.Disable();
            //disable character controller
            controller.enabled = false;
            //disable navmesh obstacle, and wait a short time before enabling the agent
            GetComponent<NavMeshObstacle>().enabled = false;
            StartCoroutine(DelayEnter());
            
            preOrientEnter = false;
            enterable = false;
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed && characterManagerScript.playerAnim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer"))
        {
            characterManagerScript.playerAnim.SetTrigger("ket exit");
            GetComponent<NavMeshObstacle>().enabled = true;
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
