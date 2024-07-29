using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem
{
    private static SaveLoadSystem _instance;
    

    public static SaveLoadSystem Instance { get {
        if (_instance == null) {
            _instance = new SaveLoadSystem();
        }
        return _instance;
    }}

    public static int GetDifficulty(int defaultValue) {
        return PlayerPrefs.GetInt("Difficulty", defaultValue);
    }

    public static void SetDifficulty(int difficulty) {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();
    }
}
