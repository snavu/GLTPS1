using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class IntSubList
{
    [SerializeField]
    public List<int> value = new List<int>();
}
public class SetDifficultySetting : MonoBehaviour
{
    public Save save;
    public float[] hungerRate;
    public float[] thirstRate;
    public List<IntSubList> temperatureLossRate;
    public List<IntSubList> weatherChance;
    public List<IntSubList> itemSpawnChance;
    void Start()
    {
        save = new Save();
    }
    public void SetEasyDifficulty()
    {
        save.hungerRate = hungerRate[0];
        save.thirstRate = thirstRate[0];
        save.temperatureLossRate = temperatureLossRate[0].value;
        save.weatherChance = weatherChance[0].value;
        save.itemSpawnChance = itemSpawnChance[0].value;
        save.SaveData(save, "difficultySetting");

        SceneManager.LoadScene(1);
    }

    public void SetMediumDifficulty()
    {
        save.hungerRate = hungerRate[1];
        save.thirstRate = thirstRate[1];
        save.temperatureLossRate = temperatureLossRate[1].value;
        save.weatherChance = weatherChance[1].value;
        save.itemSpawnChance = itemSpawnChance[1].value;
        save.SaveData(save, "difficultySetting");

        SceneManager.LoadScene(1);
    }

    public void SetHardDifficulty()
    {
        save.hungerRate = hungerRate[2];
        save.thirstRate = thirstRate[2];
        save.temperatureLossRate = temperatureLossRate[2].value;
        save.weatherChance = weatherChance[2].value;
        save.itemSpawnChance = itemSpawnChance[2].value;
        save.SaveData(save, "difficultySetting");

        SceneManager.LoadScene(1);
    }
}
