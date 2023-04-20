using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticleSystem;
    [SerializeField] private float rainParticleSystemEmmisionRate;
    [SerializeField] private ParticleSystem snowParticleSystem;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color initialSkyboxColor;
    [SerializeField] private Color rainSkyboxColor;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float initialLightIntensity;
    [SerializeField] private float rainLightIntensity;

    [SerializeField] private float duration;
    private float elapsed1 = 0f;
    private float elapsed2 = 0f;

    [SerializeField] private bool startRain;
    [SerializeField] private bool stopRain;



    void Start()
    {
        initialSkyboxColor = _renderer.material.color;
        initialLightIntensity = directionalLight.intensity;
    }
    void Update()
    {
        if (startRain)
        {
            elapsed1 += Time.deltaTime;
            _renderer.material.color = Color.Lerp(initialSkyboxColor, rainSkyboxColor, elapsed1 / duration);

            directionalLight.intensity = Mathf.Lerp(initialLightIntensity, rainLightIntensity, elapsed1 / duration);

            var emission = rainParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(0, rainParticleSystemEmmisionRate, elapsed1 / duration);


            rainParticleSystem.Play();
            snowParticleSystem.Stop();
            elapsed2 = 0f;

        }
        else if (stopRain)
        {
            startRain = false;
            
            elapsed2 += Time.deltaTime;
            _renderer.material.color = Color.Lerp(rainSkyboxColor, initialSkyboxColor, elapsed2 / duration);

            directionalLight.intensity = Mathf.Lerp(rainLightIntensity, initialLightIntensity, elapsed2 / duration);

            var emission = rainParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(rainParticleSystemEmmisionRate, 0, elapsed2 / duration);


            rainParticleSystem.Stop();
            snowParticleSystem.Play();
            elapsed1 = 0f;
        }
    }
}
