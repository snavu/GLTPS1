using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCompass : MonoBehaviour
{
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private RectTransform compass;

    void Update()
    {
        compass.rotation =  Quaternion.Euler(0,0,camera.eulerAngles.y);
    }
}
