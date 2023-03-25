using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHunger : MonoBehaviour
{
    public float hunger = 100f;
    [SerializeField]
    private float hungerRate = 1f;
    [SerializeField]
    private float maxHungerValue = 100f;
    [SerializeField]
    private float minHungerValue = 0f;
    public int index;
    public int offsetXPos;
    public List<GameObject> newFoodbar;

    void Update()
    {
        //clamp hunger so player does not exceed max hunger value when refilling hunger
        hunger = Mathf.Clamp(hunger, minHungerValue, maxHungerValue);
        hunger -= hungerRate * Time.deltaTime;
    }
}
