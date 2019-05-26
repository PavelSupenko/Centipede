using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gloval variables that we need from different parts of code
public static class GlobalVariables
{
    public static Vector3 CELL_SIZE;
    public static bool IS_FIRST_SCENE = true;
    public static bool IS_GAME_OVER = false;
    public static int SENSITIVITY = 0;
    public static int DIFFICULTY = 0;
    public static List<CentipedeController> controllers;
}
