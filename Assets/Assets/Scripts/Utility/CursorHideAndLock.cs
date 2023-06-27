using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHideAndLock : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
