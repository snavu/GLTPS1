using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SetCameraRadius : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freelookCamera;
    public float top;
    public float middle;
    public float bottom;

    [SerializeField] private float smoothRadiusSpeed = 0.2f;
    private float smoothRadiusVelocity = 0f;
    void Start()
    {
        top = freelookCamera.m_Orbits[0].m_Radius;
        middle = freelookCamera.m_Orbits[1].m_Radius;
        bottom = freelookCamera.m_Orbits[2].m_Radius;
    }
    void Update()
    {
        freelookCamera.m_Orbits[0].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[0].m_Radius, top, ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[1].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[1].m_Radius, middle, ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[2].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[2].m_Radius, bottom, ref smoothRadiusVelocity, smoothRadiusSpeed);
    }
}
