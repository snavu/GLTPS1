using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] item;
    public int[] spawnChance;
    private Save save = new Save();
    void Start()
    {
        save = save.LoadData("difficultySetting");
        if (save != null)
        {
            for (int i = 0; i < spawnChance.Length; i++)
            {
                spawnChance[i] = save.itemSpawnChance[i];
            }
        }
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
