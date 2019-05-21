using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveClass
{
    public static string SaveString = "PointSave";
    public static bool IsFirstScene { get { return isFirstScene; } set { isFirstScene = value; } }
    private static bool isFirstScene = true;

    public static void SavePoints(int p)
    {
        PlayerPrefs.SetInt(SaveString, p);
    }

    public static int GetMaxPointValue()
    {
        return PlayerPrefs.GetInt(SaveString);
    }
}
