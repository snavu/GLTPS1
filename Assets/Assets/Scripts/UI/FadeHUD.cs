using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class FadeHUD : MonoBehaviour
{
    [SerializeField] private PlayerInputInitialize chitoPlayerInput;
    [SerializeField] private PlayerInputInitialize yuuriPlayerInput;
    [SerializeField] private PlayerMovement yuuriPlayerMovementScript;
    [SerializeField] private RawImage ammoCountUI;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private RawImage fuelUI;
    [SerializeField] private RawImage fuelContainerUI;

    [SerializeField] private RawImage waterUI;
    [SerializeField] private RawImage waterContainerUI;

    [SerializeField] private CharacterItemData characterItemDataScript;

    [SerializeField] private float duration = 1f;
    private float elapsed1 = 0f;
    private float elapsed2 = 0f;
    private float elapsed3 = 0f;

    private float ammoUIAlpha;
    private float fuelUIalpha;
    private float foodUIalpha;
    void Update()
    {
        //cache current alpha values of ammo UI and fuel/water/food UI
        ammoUIAlpha = ammoCountText.color.a;
        fuelUIalpha = fuelUI.color.a;
        foodUIalpha = waterUI.color.a;

        if (yuuriPlayerMovementScript.ADS)
        {
            LerpAlpha(0.4f, 0, 0, ref elapsed1);
            elapsed2 = 0f;
            elapsed3 = 0f;
        }
        else if (chitoPlayerInput.actions.Vehicle.enabled || yuuriPlayerInput.actions.Vehicle.enabled)
        {
            LerpAlpha(0, 0.4f, 0, ref elapsed2);

            elapsed1 = 0f;
            elapsed3 = 0f;
        }
        else
        {
            LerpAlpha(0, 0, 0.4f, ref elapsed3);

            elapsed1 = 0f;
            elapsed2 = 0f;
        }

    }
    private void LerpAlpha(float alphaA, float alphaB, float alphaC, ref float elapsed)
    {
        elapsed += Time.deltaTime;

        Color color;

        // Lerp ammo count UI
        color = ammoCountUI.color;
        color.a = Mathf.Lerp(ammoUIAlpha, alphaA, elapsed / duration);
        ammoCountUI.color = color;

        // Lerp ammo count text
        color = ammoCountText.color;
        color.a = Mathf.Lerp(ammoUIAlpha, alphaA, elapsed / duration);
        ammoCountText.color = color;

        // Lerp fuel UI
        color = fuelUI.color;
        color.a = Mathf.Lerp(fuelUIalpha, alphaB, elapsed / duration);
        fuelUI.color = color;

        color = fuelContainerUI.color;
        color.a = Mathf.Lerp(fuelUIalpha, alphaB, elapsed / duration);
        fuelContainerUI.color = color;

        // Lerp water UI
        color = waterUI.color;
        color.a = Mathf.Lerp(foodUIalpha, alphaC, elapsed / duration);
        waterUI.color = color;

        color = waterContainerUI.color;
        color.a = Mathf.Lerp(foodUIalpha, alphaC, elapsed / duration);
        waterContainerUI.color = color;

        // Lerp foodbar UI
        foreach (GameObject foodbar in characterItemDataScript.newFoodbar)
        {
            color = foodbar.GetComponent<RawImage>().color;
            color.a = Mathf.Lerp(foodUIalpha, alphaC, elapsed / duration);
            foodbar.GetComponent<RawImage>().color = color;
        }
    }
}

