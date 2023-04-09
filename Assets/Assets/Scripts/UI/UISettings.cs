using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UISettings : MonoBehaviour
{
    [SerializeField] private SetSliderValue mouseSensitivitySlider;
    [SerializeField] private float defaultXAxisMaxSpeed = 1f;
    [SerializeField] private float defaultYAxisMaxSpeed = 0.01f;
    public float mouseSensitivtyXAxisSpeed = 0.15f;
    public float mouseSensitivtyYAxisSpeed = 0.0015f;
    [SerializeField] private SetSliderValue ADSSensitivitySlider;
    public float ADSSensitivtyXAxisSpeed = 0.105f;
    public float ADSSensitivtyYAxisSpeed = 0.00105f;


    [SerializeField] private SetSliderValue globalAudioVolumeSlider;

    void Update()
    {
        //set mouse sensitivity
        mouseSensitivtyXAxisSpeed = mouseSensitivitySlider.value * defaultXAxisMaxSpeed;
        mouseSensitivtyYAxisSpeed = mouseSensitivitySlider.value * defaultYAxisMaxSpeed;

        //set ADS sensitivity
        ADSSensitivtyXAxisSpeed = ADSSensitivitySlider.value * defaultXAxisMaxSpeed;
        ADSSensitivtyYAxisSpeed = ADSSensitivitySlider.value * defaultYAxisMaxSpeed;
        
        //set master volume
        AudioListener.volume = globalAudioVolumeSlider.value;
    }




}
