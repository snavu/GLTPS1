using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeDifficultySetting : MonoBehaviour
{
    [SerializeField] CharacterStatus characterStatusScript;
    [SerializeField] WeatherManager weatherManagerScript;
    public Save save = new Save();

    void Awake()
    {
        save = save.LoadData("difficultySetting");
        if (save != null)
        {
            characterStatusScript.hungerRate = save.hungerRate;
            characterStatusScript.thirstRate = save.thirstRate;
            for (int i = 0; i < characterStatusScript.temperatureLossRate.Length; i++)
            {
                characterStatusScript.temperatureLossRate[i] = save.temperatureLossRate[i];
            }
            for (int i = 0; i < weatherManagerScript.weatherChance.Length; i++)
            {
                weatherManagerScript.weatherChance[i] = save.weatherChance[i];
            }
        }
    }
}
