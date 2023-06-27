using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class WeatherManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] weatherParticleSystem;
    [SerializeField] private float[] particleSystemEmissionRate;
    [SerializeField] private ParticleSystem snowParticleSystem;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;
    [SerializeField] private Color[] tintColor;
    [SerializeField] private Color[] fogColor;
    [SerializeField] private float[] fogDensity;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float[] lightIntensity;
    [SerializeField] private float[] ambientIntensity;
    private float elapsed1 = 0f;
    private float elapsed2 = 0f;


    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private float[] audioVolume;

    [SerializeField] private RawImage mapTextureOverlay;
    [SerializeField] private Color[] mapTextureOverlayColor;
    public int index;
    public bool start;
    public bool stop;
    public bool isWeatherTransitioned;
    public int[] weatherChance;
    [SerializeField] private float transitionDuration;
    [SerializeField] private float repeatInvokeDuration = 60f;
    [SerializeField] private float weatherDuration = 60f;

    private WeatherData weatherData;


    void Start()
    {
        //initialize elasped2 to prevent lerp to default weather state when starting
        elapsed2 = transitionDuration;

        fogColor[0] = RenderSettings.fogColor;
        fogDensity[0] = RenderSettings.fogDensity;
        lightIntensity[0] = directionalLight.intensity;

        StartCoroutine(StartWeather());
    }

    private struct WeatherData
    {
        public ParticleSystem weatherParticleSystem;
        public float particleSystemEmissionRate;
        public Color tintColor;
        public Color fogColor;
        public float fogDensity;
        public float lightIntensity;
        public float ambientIntensity;
        public AudioSource audioSource;
        public float audioVolume;
        public Color mapTextureOverlayColor;
    }

    IEnumerator StartWeather()
    {

        index = RandomWeightedGenerator.GenerateRandomIndex(weatherChance);

        //stop weather  
        if (index == 0)
        {
            elapsed1 = 0f;

            start = false;
            stop = true;

            yield return new WaitForSeconds(repeatInvokeDuration);
            StartCoroutine(StartWeather());
        }
        // start weather
        else
        {
            elapsed2 = 0f;

            weatherData.weatherParticleSystem = weatherParticleSystem[index];
            weatherData.particleSystemEmissionRate = particleSystemEmissionRate[index];
            weatherData.tintColor = tintColor[index];
            weatherData.fogColor = fogColor[index];
            weatherData.fogDensity = fogDensity[index];
            weatherData.lightIntensity = lightIntensity[index];
            weatherData.ambientIntensity = ambientIntensity[index];
            weatherData.audioSource = audioSource[index];
            weatherData.audioVolume = audioVolume[index];
            weatherData.mapTextureOverlayColor = mapTextureOverlayColor[index];

            start = true;
            stop = false;
            yield return new WaitForSeconds(weatherDuration);

            elapsed1 = 0f;

            start = false;
            stop = true;
            yield return new WaitForSeconds(transitionDuration);
            StartCoroutine(StartWeather());
        }
    }

    void Update()
    {
        if (start)
        {
            SetWeather(weatherData);
        }
        else if (stop)
        {
            StopWeather(weatherData);
        }
    }

    private void SetWeather(WeatherData weatherData)
    {
        if (elapsed1 < transitionDuration)
        {
            if (elapsed1 == 0)
            {
                weatherData.audioSource.Play();
                weatherData.weatherParticleSystem.Play();
                snowParticleSystem.Stop();
            }

            elapsed1 += Time.deltaTime;

            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.colorFilter.value = Color.Lerp(tintColor[0], weatherData.tintColor, elapsed1 / transitionDuration);
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(ambientIntensity[0], weatherData.ambientIntensity, elapsed1 / transitionDuration);
            RenderSettings.fogColor = Color.Lerp(fogColor[0], weatherData.fogColor, elapsed1 / transitionDuration);
            RenderSettings.fogDensity = Mathf.Lerp(fogDensity[0], weatherData.fogDensity, elapsed1 / transitionDuration);

            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, weatherData.lightIntensity, elapsed1 / transitionDuration);

            ParticleSystem.EmissionModule emission = weatherData.weatherParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(0, weatherData.particleSystemEmissionRate, elapsed1 / transitionDuration);

            weatherData.audioSource.volume = Mathf.Lerp(0, weatherData.audioVolume, elapsed1 / transitionDuration);

            // lerp map overlay color
            Color color = Color.Lerp(mapTextureOverlayColor[0], weatherData.mapTextureOverlayColor, elapsed1 / transitionDuration);
            mapTextureOverlay.color = color;
        }
        else
        {
            isWeatherTransitioned = true;
        }
    }
    private void StopWeather(WeatherData weatherData)
    {
        if (elapsed2 < transitionDuration)
        {
            elapsed2 += Time.deltaTime;

            if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                colorAdjustments.colorFilter.value = Color.Lerp(weatherData.tintColor, tintColor[0], elapsed2 / transitionDuration);
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(weatherData.ambientIntensity, ambientIntensity[0], elapsed2 / transitionDuration);
            RenderSettings.fogColor = Color.Lerp(weatherData.fogColor, fogColor[0], elapsed2 / transitionDuration);
            RenderSettings.fogDensity = Mathf.Lerp(weatherData.fogDensity, fogDensity[0], elapsed2 / transitionDuration);

            directionalLight.intensity = Mathf.Lerp(weatherData.lightIntensity, lightIntensity[0], elapsed2 / transitionDuration);

            ParticleSystem.EmissionModule emission = weatherData.weatherParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(weatherData.particleSystemEmissionRate, 0, elapsed2 / transitionDuration);

            // lerp map overlay color
            Color color = Color.Lerp(weatherData.mapTextureOverlayColor, mapTextureOverlayColor[0], elapsed2 / transitionDuration);
            mapTextureOverlay.color = color;

            weatherData.audioSource.volume = Mathf.Lerp(weatherData.audioVolume, 0, elapsed2 / transitionDuration);
            if (weatherData.audioSource.volume == 0f)
            {
                weatherData.audioSource.Stop();
                snowParticleSystem.Play();
            }
        }
        else
        {
            isWeatherTransitioned = false;
        }
    }
}
