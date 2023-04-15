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

    [SerializeField] private CharacterItemInteraction yuuriItemInteraction;
    [SerializeField] private CharacterItemInteraction chitoItemInteraction;

    [SerializeField] private float hungerRate = 1f;
    [SerializeField] private float maxHungerValue = 100f;

    [SerializeField] private float thirstRate = 2f;
    [SerializeField] private float maxThirstValue = 100f;
    [SerializeField] private float maxWaterLevel = 400f;

    [SerializeField] private RectTransform waterBar;
    private float rectTransformInitialHeight;
    void Start()
    {
        rectTransformInitialHeight = waterBar.sizeDelta.y;

    }

    void Update()
    {

        //scale water bar directly proportionally to current water level
        waterBar.sizeDelta = new Vector2(waterBar.sizeDelta.x, rectTransformInitialHeight * (waterLevel / maxWaterLevel));
        //clamp water level
        waterLevel = Mathf.Clamp(waterLevel, 0, maxWaterLevel);

        yuuriItemInteraction.hungerLevel = Mathf.Clamp(yuuriItemInteraction.hungerLevel, 0, maxHungerValue);
        yuuriItemInteraction.hungerLevel -= hungerRate * Time.deltaTime;

        chitoItemInteraction.hungerLevel = Mathf.Clamp(chitoItemInteraction.hungerLevel, 0, maxHungerValue);
        chitoItemInteraction.hungerLevel -= hungerRate * Time.deltaTime;

        yuuriItemInteraction.thirstLevel = Mathf.Clamp(yuuriItemInteraction.thirstLevel, 0, maxThirstValue);
        yuuriItemInteraction.thirstLevel -= thirstRate * Time.deltaTime;

        chitoItemInteraction.thirstLevel = Mathf.Clamp(chitoItemInteraction.thirstLevel, 0, maxThirstValue);
        chitoItemInteraction.thirstLevel -= thirstRate * Time.deltaTime;

    }
}
