using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] item;
    [SerializeField]
    private int[] spawnChance;
    void Start()
    {
        RandomWeightedSpawnItem();
    }

    private void RandomWeightedSpawnItem()
    {
        int randomIndex = RandomWeightedGenerator.GenerateRandomIndex(spawnChance);
        if (item[randomIndex] != null)
        {
            GameObject newItem = Instantiate(item[randomIndex], transform.position, transform.rotation);
        }
    }
}
