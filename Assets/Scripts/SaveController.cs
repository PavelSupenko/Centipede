using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string SaveString = "PointSave";

    public void SavePoints(int p)
    {
        PlayerPrefs.SetInt(SaveString, p);
    }

    public int GetMaxPointValue()
    {
        return PlayerPrefs.GetInt(SaveString);
    }
}
