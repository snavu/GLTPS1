using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadSceneOutOfBounds : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Out Of Bounds"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
