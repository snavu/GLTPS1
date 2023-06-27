using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public float hungerRate;
    public float thirstRate;
    public List<int> temperatureLossRate;
    public List<int> weatherChance;
    public List<int> itemSpawnChance;

    public void SaveData(Save save, string saveKey)
    {
        // serialize json
        string saveJson = JsonUtility.ToJson(save);
        PlayerPrefs.SetString(saveKey, saveJson);
        PlayerPrefs.Save();
    }

    public Save LoadData(string saveKey)
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            // deserialize json
            string saveJson = PlayerPrefs.GetString(saveKey);
            Save save = JsonUtility.FromJson<Save>(saveJson);

            return save;
        }
        else
        {
            return null;
        }
    }
}
