using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetSliderValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public float value;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        this.value = value;
    }
}
