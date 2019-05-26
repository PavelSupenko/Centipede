using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controllers to save max points value into PlayerPrefs
public class SaveController : MonoBehaviour
{
    private string _saveString = "PointSave";
    private string _sensitivityString = "Sensitivity";
    private string _difficultyString = "Difficulty";

    public int GetDifficulty()
    {
        return PlayerPrefs.GetInt(_difficultyString);
    }

    public int GetSensitivity()
    {
        return PlayerPrefs.GetInt(_sensitivityString);
    }

    public void SaveDifficulty(int value)
    {
        PlayerPrefs.SetInt(_difficultyString, value);
        GlobalVariables.DIFFICULTY = value;
    }

    public void SaveSensitivity(int value)
    {
        PlayerPrefs.SetInt(_sensitivityString, value);
        GlobalVariables.SENSITIVITY = value;
    }

    public void SavePoints(int p)
    {
        if(PlayerPrefs.GetInt(_saveString) < p)
            PlayerPrefs.SetInt(_saveString, p);
    }

    public int GetMaxPointValue()
    {
        return PlayerPrefs.GetInt(_saveString);
    }
}
