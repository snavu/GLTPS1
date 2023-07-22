using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private UIInputActions actions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject photoMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject DifficultyMenu;
    [SerializeField] SetCameraSpeed setCameraSpeedScript;
    [SerializeField] private UISettings UISettingsScript;
    [SerializeField] private GameObject[] photoPanel;
    private int panelUIIndex;
    public int panelAssignmentIndex;
    public int imageAssignmentIndex;
    [SerializeField] private GameObject photoMenuSelectedImage;
    [SerializeField] private GameObject dialogueBoxFlag;
    [SerializeField] private bool isTitleMenu;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;
    void OnEnable()
    {
        actions = new UIInputActions();
        actions.UI.Enable();
        actions.UI.Menu.performed += Menu;
    }

    void OnDisable()
    {
        actions.UI.Menu.performed -= Menu;
        actions.UI.Disable();
    }

    public void PlayButtonHoverSFX()
    {
        audioSource.PlayOneShot(audioClip[0]);
    }
    public void PlayButtonDownSFX()
    {
        audioSource.PlayOneShot(audioClip[1]);
    }

    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (photoMenu != null)
            {
                if (photoMenuSelectedImage.activeInHierarchy)
                {
                    HideImage();
                    return;
                }
                else if (photoMenu.activeInHierarchy)
                {
                    ClosePhotoMenu();
                    OpenMainMenu();
                    PlayButtonDownSFX();
                    return;
                }
            }

            if (settingsMenu.activeInHierarchy)
            {
                UISettingsScript.SaveSettings();
                CloseSettingsMenu();
                OpenMainMenu();
                PlayButtonDownSFX();
                return;
            }

            if (mainMenu.activeInHierarchy)
            {
                if (!isTitleMenu)
                {
                    CloseMainMenu();
                    PlayButtonDownSFX();
                }
            }
            else if (!dialogueBoxFlag.activeInHierarchy)
            {
                OpenMainMenu();
                PlayButtonDownSFX();
            }
        }
    }

    public void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockAndUnhideCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 0.001f;
        UnlockAndUnhideCursor();
        mainMenu.SetActive(true);

        if (HUD != null)
        {
            HUD.SetActive(false);
        }
        if (setCameraSpeedScript != null)
        {
            setCameraSpeedScript.Pause();
        }
    }

    public void CloseMainMenu()
    {
        Time.timeScale = 1;
        LockAndHideCursor();
        mainMenu.SetActive(false);

        if (HUD != null)
        {
            HUD.SetActive(true);
        }
        if (setCameraSpeedScript != null)
        {
            setCameraSpeedScript.SetDefaultSpeed();
        }
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        HideMainMenu();
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        OpenMainMenu();
    }
    public void OpenPhotoMenu()
    {
        photoMenu.SetActive(true);
        HideMainMenu();
    }
    public void ClosePhotoMenu()
    {
        photoMenu.SetActive(false);
        OpenMainMenu();
    }

    public void NextPhotoPanel()
    {
        if (panelUIIndex < photoPanel.Length - 1)
        {
            photoPanel[panelUIIndex].SetActive(false);
            panelUIIndex++;
            photoPanel[panelUIIndex].SetActive(true);
        }
    }
    public void PreviousPhotoPanel()
    {
        if (panelUIIndex > 0)
        {
            photoPanel[panelUIIndex].SetActive(false);
            panelUIIndex--;
            photoPanel[panelUIIndex].SetActive(true);
        }
    }

    public void ShowImage()
    {
        photoMenuSelectedImage.SetActive(true);
        photoMenuSelectedImage.GetComponent<RawImage>().texture = EventSystem.current.currentSelectedGameObject.GetComponent<RawImage>().texture;
    }

    public void HideImage()
    {
        photoMenuSelectedImage.SetActive(false);
    }

    public void OpenDifficultyMenu()
    {
        DifficultyMenu.SetActive(true);
    }

    public void QuitToTitleMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
