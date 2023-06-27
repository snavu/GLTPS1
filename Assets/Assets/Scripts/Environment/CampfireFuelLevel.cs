using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireFuelLevel : MonoBehaviour
{
    public float maxFuelLevel = 100f;
    public float currentFuelLevel;
    [SerializeField] private float fuelLossRate = 1f;
    [SerializeField] private Light campFireLight;
    private float initialLightIntensity;
    [SerializeField] private ParticleSystem campfireParticleSystem;
    [SerializeField] private float particleSystemEmissionRate = 10f;
    [SerializeField] private float smokeDurationAfterExtinguished = 20f;
    [SerializeField] WeatherManager weatherManagerScript;
    [SerializeField] float isUnderCeilingHeight = 20f;
    private float elasped = 0;
    void Start()
    {
        initialLightIntensity = campFireLight.intensity;
        currentFuelLevel = maxFuelLevel;

        weatherManagerScript = GameObject.FindWithTag("Weather").GetComponent<WeatherManager>();
    }
    void Update()
    {
        //decrease fuel by fuel loss rate per second
        currentFuelLevel -= fuelLossRate * Time.deltaTime;

        //decrease light intensity proportional to fuel level
        campFireLight.intensity = initialLightIntensity * (currentFuelLevel / maxFuelLevel);

        if (currentFuelLevel <= 0)
        {
            elasped += Time.deltaTime;
            ParticleSystem.EmissionModule emission = campfireParticleSystem.emission;
            emission.rateOverTime = Mathf.Lerp(particleSystemEmissionRate, 0, elasped / smokeDurationAfterExtinguished);
        }


        //check if campfire is under ceiling
        if (!Physics.Raycast(transform.position, Vector2.up, isUnderCeilingHeight) && weatherManagerScript.isWeatherTransitioned)
        {
            currentFuelLevel = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.up * isUnderCeilingHeight);
    }

}
