using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private float initialVolume;
    public bool fadeOut;
    public bool fadeIn;
    private float elapsed = 0f;
    [SerializeField] private float duration = 10f;
    void Start()
    {
        initialVolume = audioSource.volume;
    }

    void Update()
    {
        float currentVolume = audioSource.volume;
        if (fadeOut)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, 0, elapsed / duration);
            if (elapsed > duration)
            {
                fadeOut = false;
                elapsed = 0;
            }
        }
        if (fadeIn)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, initialVolume, elapsed / duration);
            if (elapsed > duration)
            {
                fadeIn = false;
                elapsed = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PillarVoicelineTrigger"))
        {
            fadeIn = false;
            fadeOut = true;
            elapsed = 0;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PillarVoicelineTrigger"))
        {
            fadeOut = false;
            fadeIn = true;
            elapsed = 0;
        }
    }
}
