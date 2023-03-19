using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleUtils : MonoBehaviour
{
    public static bool RotationsApproximatelyEqual(Transform t1, Transform t2, float threshold = 0.1f)
    {
        float angle = Quaternion.Angle(t1.rotation, t2.rotation);
        return angle < threshold;
    }
}
