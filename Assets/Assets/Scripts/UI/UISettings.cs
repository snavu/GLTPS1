using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettings : MonoBehaviour
{
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private float defaultXAxisMaxSpeed = 1f;
    [SerializeField] private float defaultYAxisMaxSpeed = 0.01f;
    public float mouseSensitivtyXAxisSpeed = 0.15f;
    public float mouseSensitivtyYAxisSpeed = 0.0015f;
    [SerializeField] private Slider ADSSensitivitySlider;
    public float ADSSensitivtyXAxisSpeed = 0.105f;
    public float ADSSensitivtyYAxisSpeed = 0.00105f;
    [SerializeField] private Slider globalAudioVolumeSlider;

    void OnEnable()
    {
        //set initial slider values
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivitySliderValue", mouseSensitivitySlider.value);
        ADSSensitivitySlider.value = PlayerPrefs.GetFloat("ADSSensitivitySliderValue", ADSSensitivitySlider.value);
        globalAudioVolumeSlider.value = PlayerPrefs.GetFloat("globalAudioVolumeSlider", globalAudioVolumeSlider.value);
    }

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

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("mouseSensitivitySliderValue", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("ADSSensitivitySliderValue", ADSSensitivitySlider.value);
        PlayerPrefs.SetFloat("globalAudioVolumeSlider", globalAudioVolumeSlider.value);
    }
}
