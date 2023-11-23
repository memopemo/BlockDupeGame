using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerupStatus : MonoBehaviour
{
    public static bool Clone = false;
    public static bool Metal = false;
    public static bool Straight = false;
    public static bool Midair = false;
    public static bool isLoaded = false;

    public static void LoadPowerups()
    {
        Clone = SaveManager.HasCloneCollected;
        Metal = SaveManager.HasMetalCollected;
        Straight = SaveManager.HasStraightCollected;
        Midair = SaveManager.HasMidairCollected;
        isLoaded = true;
    }
}
