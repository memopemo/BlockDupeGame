using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerupStatus : MonoBehaviour
{
    public static bool Clone = true;
    public static bool Metal = true;
    public static bool Straight = true;
    public static bool Midair = false;
    public static bool JumpHold = true;
    public static bool isLoaded = true;

    public static void LoadPowerups()
    {
        Clone = SaveManager.HasCloneCollected;
        Metal = SaveManager.HasMetalCollected;
        Straight = SaveManager.HasStraightCollected;
        Midair = SaveManager.HasMidairCollected;
        isLoaded = true;
    }
    public static void SavePowerups()
    {
        SaveManager.HasCloneCollected = Clone;
        SaveManager.HasMetalCollected = Metal;
        SaveManager.HasStraightCollected = Straight;
        SaveManager.HasMidairCollected = Midair;
    }
}
