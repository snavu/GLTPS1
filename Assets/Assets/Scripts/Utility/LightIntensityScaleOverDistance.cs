using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLightIntensityScaleOverDistance : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float lightMultiplier = 1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int rayCount = 10;
    private float spotAngle;
    private float angleStep;
    private float currentAngle;
    public float[] raycastDistance;
    public float raycastDistanceAverage;
    private Vector3 rayDirection;
    void Start()
    {
        _layerMask = LayerMask.GetMask("Default", "Player");

        spotAngle = _light.spotAngle;
        angleStep = spotAngle / rayCount;

        raycastDistance = new float[rayCount];
        for (int i = 0; i < raycastDistance.Length; i++)
        {
            raycastDistance[i] = 1;
        }

    }
    void Update()
    {
        currentAngle = -spotAngle / 2;

        //assign array of raycast distances, scaled between 0 and 1
        for (int i = 0; i < rayCount; i++)
        {
            rayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _layerMask, QueryTriggerInteraction.Ignore))
            {
                if ((lightMultiplier * hit.distance) / maxIntensity < 1)
                {
                    raycastDistance[i] = (lightMultiplier * hit.distance) / maxIntensity;
                }
            }
            else //no raycast hit due to infinite distance, set distance to max 1
            {
                raycastDistance[i] = 1;
            }
            Debug.DrawLine(transform.position, transform.position + rayDirection * hit.distance, Color.yellow);

            currentAngle += angleStep;
        }

        //calculate average of distances
        float sum = 0;
        for (int i = 0; i < raycastDistance.Length; i++)
        {
            sum += raycastDistance[i];
        }
        raycastDistanceAverage = sum / raycastDistance.Length;

        //scale light intensity proportional to average of distances
        _light.intensity = Mathf.Lerp(1, maxIntensity, raycastDistanceAverage);
    }
}
