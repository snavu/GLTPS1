using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] private float firstPersonCameraFOVZoomDefault;
    [SerializeField] private float firstPersonCameraFOVZoomIn = 20f;
    public bool equipCamera;
    private bool ready = true;
    [SerializeField] private GameObject[] characterBody;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private GameObject cameraHUD;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject[] panel;
    [SerializeField] private UIManager UIManagerScript;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cameraShutterSFX;
    [SerializeField] private AudioClip cameraZoomSFX;
    private bool flag;


    void Start()
    {
        firstPersonCameraFOVZoomDefault = firstPersonCamera.m_Lens.FieldOfView;
    }

    void OnEnable()
    {
        playerInputScript.actions.Player.Fire.performed += TakePicture;
        playerInputScript.actions.Vehicle.Fire.performed += TakePicture;
    }

    void OnDisable()
    {
        playerInputScript.actions.Player.Fire.performed -= TakePicture;
        playerInputScript.actions.Vehicle.Fire.performed -= TakePicture;
    }

    private void TakePicture(InputAction.CallbackContext context)
    {
        if (context.performed && equipCamera && Time.timeScale == 1 && ready)
        {
            StartCoroutine(RecordFrame());
            ready = false;
        }
    }
    IEnumerator RecordFrame()
    {
        cameraHUD.SetActive(false);
        audioSource.PlayOneShot(cameraShutterSFX);

        yield return new WaitForEndOfFrame();

        if (UIManagerScript.panelAssignmentIndex < panel.Length)
        {
            if (UIManagerScript.imageAssignmentIndex == panel[UIManagerScript.panelAssignmentIndex].transform.childCount - 1)
            {
                UIManagerScript.panelAssignmentIndex++;
                UIManagerScript.imageAssignmentIndex = 0;
            }
            else
            {

                Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

                // correct white texture bug
                Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                newScreenShot.SetPixels(texture.GetPixels());
                newScreenShot.Apply();

                panel[UIManagerScript.panelAssignmentIndex].transform.GetChild(UIManagerScript.imageAssignmentIndex).GetComponent<RawImage>().texture = newScreenShot;
                panel[UIManagerScript.panelAssignmentIndex].transform.GetChild(UIManagerScript.imageAssignmentIndex).GetComponent<Button>().enabled = true;

                UIManagerScript.imageAssignmentIndex++;
            }
        }
        cameraHUD.SetActive(true);

        yield return new WaitForSeconds(1);
        ready = true;
    }

    void Update()
    {
        //diable camera
        if (!playerMovementScript.ADS && Time.timeScale == 1)
        {
            if (playerInputScript.actions.Player.ADS.WasReleasedThisFrame() ||
                playerInputScript.actions.Vehicle.ADS.WasReleasedThisFrame())
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

            if (playerInputScript.actions.Player.ADS.WasPressedThisFrame() ||
                playerInputScript.actions.Vehicle.ADS.WasPressedThisFrame())
            {
                thirdPersonCamera.enabled = false;
                firstPersonCamera.enabled = true;
                CinemachinePOV pov = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>();
                pov.m_VerticalAxis.Value = 0;
                pov.m_HorizontalAxis.Value = transform.eulerAngles.y;

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

        // camera zoom
        if ((playerInputScript.actions.Player.CameraZoom.ReadValue<Vector2>().y > 0 ||
            playerInputScript.actions.Vehicle.CameraZoom.ReadValue<Vector2>().y > 0)
             && !flag && equipCamera)
        {
            firstPersonCamera.m_Lens.FieldOfView = firstPersonCameraFOVZoomIn;
            audioSource.PlayOneShot(cameraZoomSFX);
            flag = true;
        }
        else if ((playerInputScript.actions.Player.CameraZoom.ReadValue<Vector2>().y < 0 ||
                 playerInputScript.actions.Vehicle.CameraZoom.ReadValue<Vector2>().y < 0)
                 && flag && equipCamera)
        {
            firstPersonCamera.m_Lens.FieldOfView = firstPersonCameraFOVZoomDefault;
            audioSource.PlayOneShot(cameraZoomSFX);
            flag = false;
        }
    }
}
