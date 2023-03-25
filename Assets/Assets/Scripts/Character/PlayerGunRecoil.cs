using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerGunRecoil : MonoBehaviour
{
    [SerializeField]
    private PlayerInputInitialize playerInputScript;
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    private float recoilValue;


    void Start()
    {
        playerInputScript.actions.Player.Fire.performed += Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           freeLookCamera.m_YAxis.Value += recoilValue;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
