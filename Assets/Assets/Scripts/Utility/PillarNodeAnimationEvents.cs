using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PillarNodeAnimationEvents : MonoBehaviour
{
    public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClip;
    public int index = 0;
    private bool closeGate;

    [SerializeField] SetCameraRadius changeCameraRadiusScript;
    [SerializeField] private float[] freelookRadius;
    [SerializeField] private float[] freelookHeight;
    [SerializeField] private float smoothRadiusSpeed = 0.2f;

    [SerializeField] private GameObject endScreen;

    void Start()
    {
        changeCameraRadiusScript = GameObject.FindWithTag("CMFreeLookCamera").GetComponent<SetCameraRadius>();
        endScreen = GameObject.FindWithTag("EndingScreen").transform.GetChild(0).gameObject;
    }

    public void PlayElevatorSequenceAudioClip()
    {
        _audioSource.PlayOneShot(_audioClip[index]);
        index++;
    }

    public void ChangeCameraRadius()
    {
        changeCameraRadiusScript.SetRadius(freelookRadius[0], freelookRadius[1], freelookRadius[2], smoothRadiusSpeed);
        changeCameraRadiusScript.SetHeight(freelookHeight[0], freelookHeight[1], freelookHeight[2]);
    }

    public void PauseGameState()
    {
        //GameObject.FindWithTag("Player").GetComponent<PlayerInputInitialize>().actions.Disable();
        GameObject.FindWithTag("CharacterManager").SetActive(false);
    }

    public void DisableCamera()
    {
        GameObject.FindWithTag("CMFreeLookCamera").SetActive(false);
    }

    public void PlayEndingSequence()
    {
        endScreen.SetActive(true);
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }




}
