using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
public class CharacterStatusScreenEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    [SerializeField] private float vignetteMinIntensity = 0.25f;

    [SerializeField] private float vignetteMaxIntensity = 0.5f;
    [SerializeField] private CharacterItemInteraction characterItemInteractionScript;
    public AudioReverbFilter reverbFilter;
    public AudioReverbPreset defaultReverbPreset;
    public AudioReverbPreset targetReverbPreset;


    public float hungerThreshold = 30f;
    public float thirstThreshold = 30f;

    private float elapsed = 0f;
    [SerializeField] private float duration = 2f;


    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;

    private bool triggerDialogue;

    void Update()
    {
        //fade screen to black and increase audio reverb
        if (characterItemInteractionScript.hungerLevel < hungerThreshold ||
            characterItemInteractionScript.thirstLevel < thirstThreshold)
        {
            //normalize player hunger/thirst 
            float normalizedStatus;
            float normalizedHunger = characterItemInteractionScript.hungerLevel / hungerThreshold;
            float normalizedThirst = characterItemInteractionScript.thirstLevel / thirstThreshold;

            if (normalizedHunger < normalizedThirst)
            {
                normalizedStatus = normalizedHunger;
            }
            else
            {
                normalizedStatus = normalizedThirst;
            }

            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                //scale vignette intensity inversely proportional to player hunger/thirst
                float scaledVignetteIntensity = (1 - normalizedStatus) * (vignetteMaxIntensity - vignetteMinIntensity);
                vignette.intensity.Override(vignetteMinIntensity + scaledVignetteIntensity);
            }

            //set audio reverb
            reverbFilter.reverbPreset = targetReverbPreset;

            if (_audioSource != null && !triggerDialogue)
            {
                _audioSource.Stop();
                _audioSource.PlayOneShot(_audioClip);
                triggerDialogue = true;
            }
        }
        else
        {
            //reset screen effect
            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                if (!Mathf.Approximately(vignette.intensity.GetValue<float>(), vignetteMinIntensity))
                {
                    elapsed += Time.deltaTime;
                    vignette.intensity.Override(Mathf.Lerp(vignette.intensity.GetValue<float>(), vignetteMinIntensity, elapsed / duration));
                }
                else
                {
                    elapsed = 0f;
                }
            }
            reverbFilter.reverbPreset = defaultReverbPreset;
            triggerDialogue = false;
        }
    }
}
