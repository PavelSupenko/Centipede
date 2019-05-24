using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controllers to save max points value into PlayerPrefs
public class SaveController : MonoBehaviour
{
    private string _saveString = "PointSave";

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
