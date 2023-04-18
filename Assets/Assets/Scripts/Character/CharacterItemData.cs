using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterItemData : MonoBehaviour
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

    [SerializeField] private RectTransform waterBar;
    private float waterBarRectTransformInitialLength;

    [SerializeField] private RectTransform chitoHungerBar;
    private float chitoHungerBarRectTransformInitialLength;
    [SerializeField] private RectTransform chitoThirstBar;
    private float chitoThirstBarRectTransformInitialLength;

    [SerializeField] private RectTransform yuuriHungerBar;
    private float yuuriHungerBarRectTransformInitialLength;
    [SerializeField] private RectTransform yuuriThirstBar;
    private float yuuriThirstBarRectTransformInitialLength;



    void Start()
    {
        waterBarRectTransformInitialLength = waterBar.sizeDelta.y;
        chitoHungerBarRectTransformInitialLength = chitoHungerBar.sizeDelta.x;
        chitoThirstBarRectTransformInitialLength = chitoThirstBar.sizeDelta.x;
        yuuriHungerBarRectTransformInitialLength = yuuriHungerBar.sizeDelta.x;
        yuuriThirstBarRectTransformInitialLength = yuuriThirstBar.sizeDelta.x;
    }

    void Update()
    {
        UpdateRectTransformSizeY(waterBar, waterBarRectTransformInitialLength, waterLevel, maxWaterLevel);
        UpdateRectTransformSizeX(chitoHungerBar, chitoHungerBarRectTransformInitialLength, chitoItemInteraction.hungerLevel, maxHungerValue);
        UpdateRectTransformSizeX(chitoThirstBar, chitoThirstBarRectTransformInitialLength, chitoItemInteraction.thirstLevel, maxThirstValue);
        UpdateRectTransformSizeX(yuuriHungerBar, yuuriHungerBarRectTransformInitialLength, yuuriItemInteraction.hungerLevel, maxHungerValue);
        UpdateRectTransformSizeX(yuuriThirstBar, yuuriThirstBarRectTransformInitialLength, yuuriItemInteraction.thirstLevel, maxThirstValue);


        float _hungerRate;
        float _thirstRate;
        if (chitoPlayerInput.actions.Vehicle.enabled || yuuriPlayerInput.actions.Vehicle.enabled)
        {
            _hungerRate = hungerRate * slowDepletionRate;
            _thirstRate = thirstRate * slowDepletionRate;
        }
        else
        {
            _hungerRate = hungerRate;
            _thirstRate = thirstRate;
        }

        yuuriItemInteraction.hungerLevel = Mathf.Clamp(yuuriItemInteraction.hungerLevel, 0, maxHungerValue);
        yuuriItemInteraction.hungerLevel -= _hungerRate * Time.deltaTime;

        chitoItemInteraction.hungerLevel = Mathf.Clamp(chitoItemInteraction.hungerLevel, 0, maxHungerValue);
        chitoItemInteraction.hungerLevel -= _hungerRate * Time.deltaTime;

        yuuriItemInteraction.thirstLevel = Mathf.Clamp(yuuriItemInteraction.thirstLevel, 0, maxThirstValue);
        yuuriItemInteraction.thirstLevel -= _thirstRate * Time.deltaTime;

        chitoItemInteraction.thirstLevel = Mathf.Clamp(chitoItemInteraction.thirstLevel, 0, maxThirstValue);
        chitoItemInteraction.thirstLevel -= _thirstRate * Time.deltaTime;
    }

    private void UpdateRectTransformSizeY(RectTransform rectTransform, float initalLength, float currentLength, float maxLength)
    {
        //scale length directly proportionally to current length
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, initalLength * (currentLength / maxLength));
        //clamp length
        currentLength = Mathf.Clamp(currentLength, 0, maxLength);
    }
    private void UpdateRectTransformSizeX(RectTransform rectTransform, float initalLength, float currentLength, float maxLength)
    {
        //scale length directly proportionally to current length
        rectTransform.sizeDelta = new Vector2(initalLength * (currentLength / maxLength), rectTransform.sizeDelta.y);
        //clamp length
        currentLength = Mathf.Clamp(currentLength, 0, maxLength);
    }

}
