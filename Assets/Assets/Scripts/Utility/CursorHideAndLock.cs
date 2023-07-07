using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHideAndLock : MonoBehaviour
{
    public bool hideAndLock;
    void Start()
    {
        if (hideAndLock)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
