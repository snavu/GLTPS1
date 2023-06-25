using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterStatus : MonoBehaviour
{
    public int index;
    public int offsetXPos;
    public List<GameObject> newFoodbar;
    public float waterLevel = 200;
    public int maxIndex = 10;

    [SerializeField] private PlayerInputInitialize yuuriPlayerInput;
    [SerializeField] private PlayerInputInitialize chitoPlayerInput;
    [SerializeField] private CharacterItemInteraction yuuriItemInteraction;
    [SerializeField] private CharacterItemInteraction chitoItemInteraction;

    [SerializeField] private float slowDepletionRate = 0.5f;
    [SerializeField] private float hungerRate = 1f;
    [SerializeField] private float maxHungerValue = 100f;

    [SerializeField] private float thirstRate = 2f;
    [SerializeField] private float maxThirstValue = 100f;
    [SerializeField] private float maxWaterLevel = 400f;
    [SerializeField] private float waterSlowRefillRate = 0.1f;

    [SerializeField] private float[] weatherTemperatureLossRate;

    [SerializeField] private float temperatureLossRate = 1f;
    [SerializeField] private float maxTemperatureValue = 100f;

    [SerializeField] private RectTransform waterBar;
    private float waterBarRectTransformInitialLength;

    [SerializeField] private Image chitoHungerBar;
    private float chitoHungerBarInitialFillAmount;
    [SerializeField] private Image chitoThirstBar;
    private float chitoThirstBarInitialFillAmount;
    [SerializeField] private Image chitoTemperatureBar;
    private float chitoTemperatureBarInitialFillAmount;


    [SerializeField] private Image yuuriHungerBar;
    private float yuuriHungerBarInitialFillAmount;
    [SerializeField] private Image yuuriThirstBar;
    private float yuuriThirstBarInitialFillAmount;
    [SerializeField] private Image yuuriTemperatureBar;
    private float yuuriTemperatureBarInitialFillAmount;


    [SerializeField] private WeatherManager weatherManagerScript;

    public

    void Start()
    {
        waterBarRectTransformInitialLength = waterBar.sizeDelta.y;

        chitoHungerBarInitialFillAmount = chitoHungerBar.fillAmount;
        chitoThirstBarInitialFillAmount = chitoThirstBar.fillAmount;
        chitoTemperatureBarInitialFillAmount = chitoTemperatureBar.fillAmount;

        yuuriHungerBarInitialFillAmount = yuuriHungerBar.fillAmount;
        yuuriThirstBarInitialFillAmount = yuuriThirstBar.fillAmount;
        yuuriTemperatureBarInitialFillAmount = yuuriTemperatureBar.fillAmount;
    }

    void Update()
    {
        UpdateRectTransformSizeY(waterBar, waterBarRectTransformInitialLength, waterLevel, maxWaterLevel);

        UpdateImageFillAmount(chitoHungerBar, chitoHungerBarInitialFillAmount, chitoItemInteraction.hungerLevel, maxHungerValue);
        UpdateImageFillAmount(chitoThirstBar, chitoThirstBarInitialFillAmount, chitoItemInteraction.thirstLevel, maxThirstValue);
        UpdateImageFillAmount(chitoTemperatureBar, chitoTemperatureBarInitialFillAmount, chitoItemInteraction.temperatureLevel, maxTemperatureValue);

        UpdateImageFillAmount(yuuriHungerBar, yuuriHungerBarInitialFillAmount, yuuriItemInteraction.hungerLevel, maxHungerValue);
        UpdateImageFillAmount(yuuriThirstBar, yuuriThirstBarInitialFillAmount, yuuriItemInteraction.thirstLevel, maxThirstValue);
        UpdateImageFillAmount(yuuriTemperatureBar, yuuriTemperatureBarInitialFillAmount, yuuriItemInteraction.temperatureLevel, maxTemperatureValue);


        float scaledHungerRate;
        float scaledThirstRate;
        float scaledTemperatureLossRate;

        if (chitoPlayerInput.actions.Vehicle.enabled || yuuriPlayerInput.actions.Vehicle.enabled)
        {
            scaledHungerRate = hungerRate * slowDepletionRate;
            scaledThirstRate = thirstRate * slowDepletionRate;
        }
        else
        {
            scaledHungerRate = hungerRate;
            scaledThirstRate = thirstRate;
        }
        waterLevel = Mathf.Clamp(waterLevel, 0, maxWaterLevel);

        if (weatherManagerScript.index == 1 && (!chitoItemInteraction.isUnderCeiling || !yuuriItemInteraction.isUnderCeiling))
        {
            waterLevel += waterSlowRefillRate * Time.deltaTime;
        }

        if (!chitoItemInteraction.isUnderCeiling)
        {
            scaledTemperatureLossRate = temperatureLossRate * weatherTemperatureLossRate[weatherManagerScript.index];
        }
        else
        {
            scaledTemperatureLossRate = temperatureLossRate;
        }

        chitoItemInteraction.temperatureLevel = Mathf.Clamp(chitoItemInteraction.temperatureLevel, 0, maxTemperatureValue);
        chitoItemInteraction.temperatureLevel -= scaledTemperatureLossRate * Time.deltaTime;

        chitoItemInteraction.hungerLevel = Mathf.Clamp(chitoItemInteraction.hungerLevel, 0, maxHungerValue);
        chitoItemInteraction.hungerLevel -= scaledHungerRate * Time.deltaTime;
        chitoItemInteraction.thirstLevel = Mathf.Clamp(chitoItemInteraction.thirstLevel, 0, maxThirstValue);
        chitoItemInteraction.thirstLevel -= scaledThirstRate * Time.deltaTime;


        if (!chitoItemInteraction.isUnderCeiling)
        {
            scaledTemperatureLossRate = temperatureLossRate * weatherTemperatureLossRate[weatherManagerScript.index];
        }
        else
        {
            scaledTemperatureLossRate = temperatureLossRate;
        }
        
        yuuriItemInteraction.temperatureLevel = Mathf.Clamp(yuuriItemInteraction.temperatureLevel, 0, maxTemperatureValue);
        yuuriItemInteraction.temperatureLevel -= scaledTemperatureLossRate * Time.deltaTime;

        yuuriItemInteraction.hungerLevel = Mathf.Clamp(yuuriItemInteraction.hungerLevel, 0, maxHungerValue);
        yuuriItemInteraction.hungerLevel -= scaledHungerRate * Time.deltaTime;
        yuuriItemInteraction.thirstLevel = Mathf.Clamp(yuuriItemInteraction.thirstLevel, 0, maxThirstValue);
        yuuriItemInteraction.thirstLevel -= scaledThirstRate * Time.deltaTime;
    }

    private void UpdateRectTransformSizeY(RectTransform rectTransform, float initalLength, float currentLength, float maxLength)
    {
        //scale length directly proportionally to current length
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, initalLength * (currentLength / maxLength));
    }
    private void UpdateRectTransformSizeX(RectTransform rectTransform, float initalLength, float currentLength, float maxLength)
    {
        //scale length directly proportionally to current length
        rectTransform.sizeDelta = new Vector2(initalLength * (currentLength / maxLength), rectTransform.sizeDelta.y);
    }

    private void UpdateImageFillAmount(Image image, float initalFillAmount, float currentFillAmount, float maxFillAmount)
    {
        image.fillAmount = initalFillAmount * (currentFillAmount / maxFillAmount);
    }
}
