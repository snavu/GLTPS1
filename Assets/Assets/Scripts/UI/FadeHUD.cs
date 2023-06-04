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
    [SerializeField] private RawImage ammoCountUIRawImage;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private RawImage[] fuelUIRawImage;
    [SerializeField] private RawImage[] waterUIRawImage;
    [SerializeField] private RawImage[] compassUIRawImage;
    [SerializeField] private Image[] compassUIImage;


    [SerializeField] private CharacterItemData characterItemDataScript;
    public VehicleBarrel vehicleBarrelScript;
    [SerializeField] private NPCInteraction chitoNPCInteractionScript;
    [SerializeField] private NPCInteraction yuuriNPCInteractionScript;


    [SerializeField] private float duration = 1f;
    private float elapsed1 = 0f;
    private float elapsed2 = 0f;
    private float elapsed3 = 0f;
    private float elapsed4 = 0f;


    private float ammoUIAlpha;
    private float fuelUIalpha;
    private float foodUIalpha;
    private float compassUIalpha;

    void Update()
    {
        //cache current alpha values of ammo UI and fuel/water/food UI
        ammoUIAlpha = ammoCountText.color.a;
        fuelUIalpha = fuelUIRawImage[0].color.a;
        foodUIalpha = waterUIRawImage[0].color.a;
        compassUIalpha = compassUIRawImage[0].color.a;


        //lerp HUD for dialogue interaction
        if (chitoNPCInteractionScript.triggerDialogue ||
            yuuriNPCInteractionScript.triggerDialogue)
        {
            LerpAlpha(0, 0, 0, 0, ref elapsed4);
            elapsed3 = 0f;
        }
        //lerp HUD for ADS
        else if (yuuriPlayerMovementScript.ADS)
        {
            LerpAlpha(0.4f, 0, 0, 0.4f, ref elapsed1);
            elapsed3 = 0f;
        }
        //lerp HUD for fuel when in vehicle
        else if (chitoPlayerInput.actions.Vehicle.enabled ||
                 yuuriPlayerInput.actions.Vehicle.enabled)
        {
            LerpAlpha(0, 0.4f, 0, 0.4f, ref elapsed2);
            elapsed3 = 0f;
        }
        //lerp HUD for fuel if barrel is being carried or fueling
        else if (vehicleBarrelScript != null)
        {
            if (vehicleBarrelScript.characterBarrelInteractionScript.isCarrying ||
                 vehicleBarrelScript.isFueling)
            {
                LerpAlpha(0, 0.4f, 0, 0.4f, ref elapsed2);
                elapsed3 = 0f;
            }
            //lerp HUD for food/water
            else
            {
                LerpAlpha(0, 0, 0.4f, 0.4f, ref elapsed3);
                elapsed2 = 0f;
            }
        }
        //lerp HUD for food/water
        else
        {
            LerpAlpha(0, 0, 0.4f, 0.4f, ref elapsed3);
            elapsed1 = 0f;
            elapsed2 = 0f;
            elapsed4 = 0f;
        }

    }

    // note: alphaA corresponds to alpha of ammoUI
    //       alphaB corresponds to alpha of fuelUIRawImage
    //       alphaC corresponds to alpha of foodUI
    //       alphaD corresponds to alpha of compassUI
    private void LerpAlpha(float alphaA, float alphaB, float alphaC, float alphaD, ref float elapsed)
    {
        elapsed += Time.deltaTime;

        Color color;

        // Lerp ammo count UI
        color = ammoCountUIRawImage.color;
        color.a = Mathf.Lerp(ammoUIAlpha, alphaA, elapsed / duration);
        ammoCountUIRawImage.color = color;

        // Lerp ammo count text
        color = ammoCountText.color;
        color.a = Mathf.Lerp(ammoUIAlpha, alphaA, elapsed / duration);
        ammoCountText.color = color;

        // Lerp fuel UI
        foreach (RawImage UIComponent in fuelUIRawImage)
        {
            color = UIComponent.color;
            color.a = Mathf.Lerp(fuelUIalpha, alphaB, elapsed / duration);
            UIComponent.color = color;
        }

        // Lerp foodbar UI
        foreach (GameObject foodbar in characterItemDataScript.newFoodbar)
        {
            color = foodbar.GetComponent<RawImage>().color;
            color.a = Mathf.Lerp(foodUIalpha, alphaC, elapsed / duration);
            foodbar.GetComponent<RawImage>().color = color;
        }
        // Lerp water UI
        foreach (RawImage UIComponent in waterUIRawImage)
        {
            color = UIComponent.color;
            color.a = Mathf.Lerp(foodUIalpha, alphaC, elapsed / duration);
            UIComponent.color = color;
        }

        // Lerp compass UI
        foreach (RawImage UIComponent in compassUIRawImage)
        {
            color = UIComponent.color;
            color.a = Mathf.Lerp(compassUIalpha, alphaD, elapsed / duration);
            UIComponent.color = color;
        }
        foreach (Image UIComponent in compassUIImage)
        {
            color = UIComponent.color;
            color.a = Mathf.Lerp(compassUIalpha, alphaD, elapsed / duration);
            UIComponent.color = color;
        }
    }
}

