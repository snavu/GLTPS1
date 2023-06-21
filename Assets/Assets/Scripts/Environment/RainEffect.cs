using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class RainEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticleSystem;
    [SerializeField] private float rainParticleSystemEmmisionRate;
    [SerializeField] private ParticleSystem snowParticleSystem;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;
    [SerializeField] private Color initialTintColor;
    [SerializeField] private Color rainTintColor;
    [SerializeField] private Color initialFogColor;
    [SerializeField] private Color rainFogColor;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float initialLightIntensity;
    [SerializeField] private float rainLightIntensity;
    [SerializeField] private float duration;
    private float elapsed1 = 0f;
    private float elapsed2 = 0f;

    public bool startRain;
    public bool stopRain;

    public int[] rainChance;
    [SerializeField] private float repeatInvokeDuration = 5f;
    [SerializeField] private float rainDuration = 60f;

    [SerializeField] private AudioSource rainAudioSource;

    [SerializeField] private RawImage mapTextureOverlay;
    [SerializeField] private Color mapTextureOverlaySnow;
    [SerializeField] private Color mapTextureOverlayRain;


    void Start()
    {
        //initliaze elapsed2 to prevent lerp to no rain
        elapsed2 = duration;

        initialFogColor = RenderSettings.fogColor;
        initialLightIntensity = directionalLight.intensity;

        StartCoroutine(Rain());
    }

    IEnumerator Rain()
    {
        int randomIndex = RandomWeightedGenerator.GenerateRandomIndex(rainChance);

        //no rain 
        if (randomIndex == 0)
        {
            startRain = false;
            stopRain = true;
            yield return new WaitForSeconds(repeatInvokeDuration);
            StartCoroutine(Rain());
        }
        //rain
        else if (randomIndex == 1)
        {
            startRain = true;
            stopRain = false;
            yield return new WaitForSeconds(rainDuration);
            StartCoroutine(Rain());
        }
    }

    void Update()
    {
        if (startRain && elapsed1 < duration)
        {
            if (elapsed1 == 0)
            {
                rainAudioSource.Play();
                snowParticleSystem.Stop();
            }

            elapsed1 += Time.deltaTime;

            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.colorFilter.value = Color.Lerp(initialTintColor, rainTintColor, elapsed1 / duration);
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(1, 0.5f, elapsed1 / duration);
            RenderSettings.fogColor = Color.Lerp(initialFogColor, rainFogColor, elapsed1 / duration);

            directionalLight.intensity = Mathf.Lerp(initialLightIntensity, rainLightIntensity, elapsed1 / duration);

            var emission = rainParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(0, rainParticleSystemEmmisionRate, elapsed1 / duration);

            rainAudioSource.volume = Mathf.Lerp(0, 0.08f, elapsed1 / duration);

            // lerp map overlay color
            Color color = Color.Lerp(mapTextureOverlaySnow, mapTextureOverlayRain, elapsed1 / duration);
            mapTextureOverlay.color = color;

            elapsed2 = 0f;

        }
        else if (stopRain && elapsed2 < duration)
        {
            elapsed2 += Time.deltaTime;

            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.colorFilter.value = Color.Lerp(rainTintColor, initialTintColor, elapsed2 / duration);
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(0.5f, 1, elapsed2 / duration);
            RenderSettings.fogColor = Color.Lerp(rainFogColor, initialFogColor, elapsed2 / duration);

            directionalLight.intensity = Mathf.Lerp(rainLightIntensity, initialLightIntensity, elapsed2 / duration);

            var emission = rainParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(rainParticleSystemEmmisionRate, 0, elapsed2 / duration);

            // lerp map overlay color
            Color color = Color.Lerp(mapTextureOverlayRain, mapTextureOverlaySnow, elapsed1 / duration);
            mapTextureOverlay.color = color;

            rainAudioSource.volume = Mathf.Lerp(0.08f, 0, elapsed2 / duration);
            if (rainAudioSource.volume == 0f)
            {
                rainAudioSource.Stop();
                snowParticleSystem.Play();
            }

            elapsed1 = 0f;
        }
    }
}
