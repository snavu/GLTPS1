using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SetCameraRadius : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freelookCamera;
    public float[] radius;
    public float[] height;
    public float[] defaultRadius;
    public float[] defaultHeight;

    [SerializeField] private float smoothRadiusSpeed = 0.6f;
    private float defaultSmoothRadiusSpeed;
    private float smoothRadiusVelocity = 0f;
    void Start()
    {
        defaultRadius[0] = freelookCamera.m_Orbits[0].m_Radius;
        defaultRadius[1] = freelookCamera.m_Orbits[1].m_Radius;
        defaultRadius[2] = freelookCamera.m_Orbits[2].m_Radius;

        defaultHeight[0] = freelookCamera.m_Orbits[0].m_Height;
        defaultHeight[1] = freelookCamera.m_Orbits[1].m_Height;
        defaultHeight[2] = freelookCamera.m_Orbits[2].m_Height;

        SetDefaultRadius();
        SetDefaultHeight();

        defaultSmoothRadiusSpeed = smoothRadiusSpeed;
    }
    void Update()
    {
        freelookCamera.m_Orbits[0].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[0].m_Radius, radius[0], ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[1].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[1].m_Radius, radius[1], ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[2].m_Radius = Mathf.SmoothDamp(freelookCamera.m_Orbits[2].m_Radius, radius[2], ref smoothRadiusVelocity, smoothRadiusSpeed);

        freelookCamera.m_Orbits[0].m_Height = Mathf.SmoothDamp(freelookCamera.m_Orbits[0].m_Height, height[0], ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[1].m_Height = Mathf.SmoothDamp(freelookCamera.m_Orbits[1].m_Height, height[1], ref smoothRadiusVelocity, smoothRadiusSpeed);
        freelookCamera.m_Orbits[2].m_Height = Mathf.SmoothDamp(freelookCamera.m_Orbits[2].m_Height, height[2], ref smoothRadiusVelocity, smoothRadiusSpeed);
    }

    public void SetDefaultRadius()
    {
        radius[0] = defaultRadius[0];
        radius[1] = defaultRadius[1];
        radius[2] = defaultRadius[2];
    }
    public void SetDefaultHeight()
    {
        height[0] = defaultHeight[0];
        height[1] = defaultHeight[1];
        height[2] = defaultHeight[2];
    }
    public void SetDefaultSmoothRadiusSpeed()
    {
        smoothRadiusSpeed = defaultSmoothRadiusSpeed;
    }

    public void SetRadius(float topRadius, float middleRadius, float bottomRadius)
    {
        radius[0] = topRadius;
        radius[1] = middleRadius;
        radius[2] = bottomRadius;
        this.smoothRadiusSpeed = defaultSmoothRadiusSpeed;
    }
    public void SetRadius(float topRadius, float middleRadius, float bottomRadius, float smoothRadiusSpeed)
    {
        radius[0] = topRadius;
        radius[1] = middleRadius;
        radius[2] = bottomRadius;
        this.smoothRadiusSpeed = smoothRadiusSpeed;
    }

    public void SetHeight(float topHeight, float middleHeight, float bottomHeight)
    {
        height[0] = topHeight;
        height[1] = middleHeight;
        height[2] = bottomHeight;
    }
}
