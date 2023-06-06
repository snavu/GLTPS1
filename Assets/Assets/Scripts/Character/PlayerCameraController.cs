using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;

    public bool equipCamera;
    [SerializeField] private GameObject[] characterBody;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private GameObject cameraHUD;
    [SerializeField] private Volume volume;


    void OnEnable()
    {
        playerInputScript.actions.Player.Fire.performed += TakePicture;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Fire.performed -= TakePicture;
    }

    private void TakePicture(InputAction.CallbackContext context)
    {
        if (context.performed && equipCamera)
        {
            Debug.Log("take picture");
        }
    }

    void Update()
    {
        if (playerInputScript.actions.Player.ADS.WasReleasedThisFrame() && !playerMovementScript.ADS)
        {
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            equipCamera = false;
            cameraHUD.SetActive(false);

            if (volume.profile.TryGet<FilmGrain>(out FilmGrain filmGrain))
            {
                filmGrain.active = false;
            }
            
            foreach (GameObject body in characterBody)
            {
                body.SetActive(true);
            }
        }

        if (playerInputScript.actions.Player.Sprint.ReadValue<float>() > 0 && !playerMovementScript.ADS)
        {
            if (playerInputScript.actions.Player.ADS.WasPressedThisFrame())
            {
                thirdPersonCamera.enabled = false;
                firstPersonCamera.enabled = true;
                equipCamera = true;
                cameraHUD.SetActive(true);

                if (volume.profile.TryGet<FilmGrain>(out FilmGrain filmGrain))
                {
                    filmGrain.active = true;
                }

                foreach (GameObject body in characterBody)
                {
                    body.SetActive(false);
                }
            }
        }
    }
}
