using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetCameraSpeed : MonoBehaviour
{
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private UISettings UISettingsScript;

    void Update()
    {

    }

    public void SetDefaultSpeed()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = UISettingsScript.mouseSensitivtyXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = UISettingsScript.mouseSensitivtyYAxisSpeed;

        virtualCamera.enabled = true;
    }

    public void SetADSSpeed()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = UISettingsScript.ADSSensitivtyXAxisSpeed;
        freeLookCamera.m_YAxis.m_MaxSpeed = UISettingsScript.ADSSensitivtyYAxisSpeed;
    }

    public void Pause()
    {
        freeLookCamera.m_XAxis.m_MaxSpeed = 0.01f;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0.0001f;

        virtualCamera.enabled = false;
    }
}
