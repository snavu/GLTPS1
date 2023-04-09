using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetCameraSpeed : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private UISettings UISettingsScript;

    void Update()
    {

    }

    public void SetDefaultSpeed()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = UISettingsScript.mouseSensitivtyXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = UISettingsScript.mouseSensitivtyYAxisSpeed;

        freeLookCamera.m_XAxis.m_DecelTime = 0.4f;
        freeLookCamera.m_YAxis.m_DecelTime = 0.4f;
    }

    public void SetADSSpeed()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = UISettingsScript.ADSSensitivtyXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = UISettingsScript.ADSSensitivtyYAxisSpeed;

        freeLookCamera.m_XAxis.m_DecelTime = 0.4f;
        freeLookCamera.m_YAxis.m_DecelTime = 0.4f;
    }

    public void Pause()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0f;
    }
}
