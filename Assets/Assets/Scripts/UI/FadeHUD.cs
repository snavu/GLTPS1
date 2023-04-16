using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class FadeHUD : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize playerInputScript;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private RawImage ammoCountUI;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private RawImage fuelUI;
    [SerializeField] private RawImage fuelContainerUI;

    [SerializeField] private RawImage waterUI;
    [SerializeField] private RawImage waterContainerUI;

    [SerializeField] private CharacterItemData characterItemDataScript;

    [SerializeField] private float duration = 1f;
    private float elapsed = 0f;
    private bool resetElasped;

    private float currentAmmoUIAlpha;
    private float currentOtherUIalpha;
    void Update()
    {
        //cache current alpha values of ammo UI and fuel/water/food UI
        currentAmmoUIAlpha = ammoCountText.color.a;
        currentOtherUIalpha = fuelUI.color.a;


        if (playerMovementScript.ADS)
        {
            if (!resetElasped)
            {
                elapsed = 0f;
                resetElasped = true;
            }

            LerpHUDAlpha(0, 0.4f);
        }
        else
        {
            if (resetElasped)
            {
                elapsed = 0f;
                resetElasped = false;
            }
            LerpHUDAlpha(0.4f, 0);
        }
    }

    private void LerpHUDAlpha(float alphaA, float alphaB)
    {
        elapsed += Time.deltaTime;

        Color color;

        // Lerp ammo count UI
        color = ammoCountUI.color;
        color.a = Mathf.Lerp(currentAmmoUIAlpha, alphaB, elapsed / duration);
        ammoCountUI.color = color;

        // Lerp ammo count text
        color = ammoCountText.color;
        color.a = Mathf.Lerp(currentAmmoUIAlpha, alphaB, elapsed / duration);
        ammoCountText.color = color;

        // Lerp fuel UI
        color = fuelUI.color;
        color.a = Mathf.Lerp(currentOtherUIalpha, alphaA, elapsed / duration);
        fuelUI.color = color;

        color = fuelContainerUI.color;
        color.a = Mathf.Lerp(currentOtherUIalpha, alphaA, elapsed / duration);
        fuelContainerUI.color = color;

        // Lerp water UI
        color = waterUI.color;
        color.a = Mathf.Lerp(currentOtherUIalpha, alphaA, elapsed / duration);
        waterUI.color = color;

        color = waterContainerUI.color;
        color.a = Mathf.Lerp(currentOtherUIalpha, alphaA, elapsed / duration);
        waterContainerUI.color = color;

        // Lerp foodbar UI
        foreach (GameObject foodbar in characterItemDataScript.newFoodbar)
        {
            color = foodbar.GetComponent<RawImage>().color;
            color.a = Mathf.Lerp(currentOtherUIalpha, alphaA, elapsed / duration);
            foodbar.GetComponent<RawImage>().color = color;
        }
    }
}
