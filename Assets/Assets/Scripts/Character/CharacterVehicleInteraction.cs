using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Cinemachine;
public class CharacterVehicleInteraction : MonoBehaviour
{
    [SerializeField] private float[] freelookRadius;
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator vehicleAnim;
    public CharacterController controller;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider navMeshAgentCollider;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float angularSpeed = 800f;
    [SerializeField] private Transform child;
    [SerializeField] private Transform vehicleSeat;
    private bool enterable;
    private float steer;
    private float steerSmooth = 0f;
    [SerializeField] private float smoothInputSpeed = 0.2f;
    private float smoothInputVelocity = 0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    public float elapsed = 0f;

    public bool preOrientEnter = false;
    public bool preOrientExit = false;
    private float maxSpeed = 4.0f;
    public bool constraint;

    [SerializeField] private Transform vehicleRightExit;

    [SerializeField] private Transform vehicleLeftExit;
    public bool exitRight;
    public bool exitLeft;
    [SerializeField] private float exitDuration = 0.25f;
    [SerializeField] private float enterDuration = 1.0f;

    [SerializeField] private GameObject light;
    private Transform vehicleEnter;

    [SerializeField] VehicleFuelManager vehicleFuelManagerScript;

    [SerializeField] AudioSource audioSourceVocal;
    [SerializeField] AudioSource audioSourceToggleLight;

    [SerializeField] AudioClip voiceline;
    [SerializeField] AudioClip toggleLightOn;
    [SerializeField] AudioClip toggleLightOff;
    [SerializeField] SetCameraRadius changeCameraRadiusScript;
    public bool changeCameraRadiusFlag1;
    public bool changeCameraRadiusFlag2;

    void OnEnable()
    {
        Physics.IgnoreLayerCollision(6, 7, false);
        Physics.IgnoreLayerCollision(6, 8, false);

        playerInputScript.actions.Player.Interact.performed += Interact;
        playerInputScript.actions.Vehicle.Exit.performed += Exit;
        playerInputScript.actions.Vehicle.Light.performed += Light;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Interact.performed -= Interact;
        playerInputScript.actions.Vehicle.Exit.performed -= Exit;
        playerInputScript.actions.Vehicle.Light.performed -= Light;
    }

    void Update()
    {
        //constrain player to vehicle
        if (constraint)
        {
            transform.position = vehicleSeat.position;
            transform.rotation = vehicleSeat.rotation;
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
        if (playerInputScript.actions.Vehicle.Drive.enabled)
        {
            steer = playerInputScript.actions.Vehicle.Drive.ReadValue<Vector2>().x;
            steerSmooth = Mathf.SmoothDamp(steerSmooth, steer, ref smoothInputVelocity, smoothInputSpeed);
            vehicleAnim.SetFloat("steer", steerSmooth);
            anim.SetFloat("steer", steerSmooth);
        }

        //handle player movement animation when navmesh agent takes over to position player in correct position to enter vehichle
        if (agent.isActiveAndEnabled)
        {
            CharacterMovementAnimation.Movement(anim, agent.velocity, maxSpeed);
        }

        //orient position to enter ket
        if (!preOrientEnter && !GetComponent<NavMeshObstacle>().enabled)
        {
            agent.angularSpeed = angularSpeed;
            if (agent.enabled)
            {
                agent.destination = vehicleEnter.position;
            }
            if (Vector3.Distance(transform.position, vehicleEnter.position) < 0.1f)
            {
                //orient rotation to enter ket
                agent.enabled = false;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, vehicleEnter.rotation, rotationSpeed * Time.deltaTime);
                child.rotation = Quaternion.RotateTowards(child.rotation, vehicleEnter.rotation, rotationSpeed * Time.deltaTime);

                if (AngleUtils.RotationsApproximatelyEqual(transform, vehicleEnter) && AngleUtils.RotationsApproximatelyEqual(child, vehicleEnter))
                {
                    if (vehicleEnter.gameObject.name == "Right")
                    {
                        anim.SetBool("ket right", true);
                    }
                    if (vehicleEnter.gameObject.name == "Left")
                    {
                        anim.SetBool("ket left", true);
                    }
                    preOrientEnter = true;
                }
            }
        }

        //increase camera radius for vehicle movement
        if (changeCameraRadiusFlag1)
        {
            changeCameraRadiusScript.SetRadius(freelookRadius[0], freelookRadius[1], freelookRadius[2]);
            changeCameraRadiusFlag1 = false;
        }
        //decrease camera radius back normal radius for player movement
        if (changeCameraRadiusFlag2)
        {
            changeCameraRadiusScript.SetDefaultRadius();
            changeCameraRadiusFlag2 = false;
        }
    }

    IEnumerator DelayExit(float duration)
    {
        yield return new WaitForSeconds(duration);
        Physics.IgnoreLayerCollision(6, 7, false);
        controller.enabled = true;
        preOrientExit = false;
        exitLeft = false;
        exitRight = false;
        elapsed = 0f;

        GetComponent<CharacterBarrelInteraction>().vehicleRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    IEnumerator DelayEnter()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
    }

    IEnumerator CancelEnter()
    {
        yield return new WaitForSeconds(5f);

        if (!anim.GetCurrentAnimatorStateInfo(1).IsTag("Ket"))
        {
            Physics.IgnoreLayerCollision(6, 7, false);
            playerInputScript.actions.Player.Enable();
            controller.enabled = true;
            GetComponent<NavMeshObstacle>().enabled = true;
            agent.enabled = false;
            enterable = true;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && enterable
            && !anim.GetCurrentAnimatorStateInfo(1).IsTag("Ket")
            && !GetComponent<CharacterBarrelInteraction>().isCarrying &&
                Time.timeScale == 1)
        {
            vehicleFuelManagerScript.playerInputScript = playerInputScript;

            //ignore collisions between layer 6 (vehicle) and layer 7 (player) 
            Physics.IgnoreLayerCollision(6, 7, true);

            //disable player movement
            playerInputScript.actions.Player.Disable();
            //disable character controller
            controller.enabled = false;

            //disable navmesh obstacle, and wait a short time before enabling the agent
            GetComponent<NavMeshObstacle>().enabled = false;
            StartCoroutine(DelayEnter());
            StartCoroutine(CancelEnter());

            enterable = false;

            if (audioSourceVocal != null)
            {
                audioSourceVocal.Stop();
                audioSourceVocal.PlayOneShot(voiceline);
            }
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if (context.performed && anim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer") && Time.timeScale == 1)
        {
            anim.SetTrigger("ket exit");
            GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    private void Light(InputAction.CallbackContext context)
    {
        if (context.performed && anim.GetCurrentAnimatorStateInfo(1).IsName("Ket Steer") && Time.timeScale == 1)
        {
            if (light.activeInHierarchy)
            {
                light.SetActive(false);
                if (audioSourceToggleLight != null)
                {
                    audioSourceToggleLight.PlayOneShot(toggleLightOff);
                }
            }
            else
            {
                light.SetActive(true);
                if (audioSourceToggleLight != null)
                {
                    audioSourceToggleLight.PlayOneShot(toggleLightOn);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
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
