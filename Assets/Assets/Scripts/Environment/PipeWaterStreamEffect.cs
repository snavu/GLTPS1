using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStreamEffect : MonoBehaviour
{
    [SerializeField]
    private PipeWaterLevel pipeWaterLevelScript;
    [SerializeField]
    private ParticleSystem waterStream;
    private ParticleSystem.EmissionModule emission;
    private float elasped = 0f;
    [SerializeField]
    private float duration = 2f;
    private float decreaseRate = 1f;
    void Start()
    {
        emission = waterStream.emission;
    }
    void Update()
    {
        //check if the reference to the pipe is null
        //note: the reference is set in OnTriggerEnter
        if (pipeWaterLevelScript != null)
        {
            //decrease water level
            pipeWaterLevelScript.waterlevel -= decreaseRate * Time.deltaTime;

            //lerp particle rate to 0
            if (pipeWaterLevelScript.waterlevel <= 0)
            {
                elasped += Time.deltaTime;
                emission.rateOverTime = Mathf.Lerp(emission.rateOverTime.constant, 0, elasped / duration);

                if (emission.rateOverTime.constant == 0)
                {
                    waterStream.Stop();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pipe"))
        {
            pipeWaterLevelScript = other.gameObject.GetComponent<PipeWaterLevel>();
            if (pipeWaterLevelScript.waterlevel > 0)
            {
                waterStream.Play();
            }
        }
    }
}
