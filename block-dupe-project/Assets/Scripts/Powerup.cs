using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType {Clone, Metal, Straight, Midair, JumpHold, ClonePack, HealthPack}
    public PowerupType powerupType;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent(out PlayerStateManager player) && player.currentState is DefaultPlayerState )
        {
            Collect();
        }
    }
    public void Collect()
    {
        FindFirstObjectByType<PowerupPanel>(FindObjectsInactive.Include).Show(powerupType);
        SetPowerup();
        //Close UI
        Destroy(gameObject);
    }
    void SetPowerup()
    {
        switch (powerupType)
        {
            case PowerupType.Clone:
                PowerupStatus.Clone = true;
                break;
            case PowerupType.Metal:
                PowerupStatus.Metal = true;
                break;
            case PowerupType.Straight:
                PowerupStatus.Straight = true;
                break;
            case PowerupType.Midair:
                PowerupStatus.Midair = true;
                break;
            case PowerupType.JumpHold:
                PowerupStatus.JumpHold = true;
                break;
            case PowerupType.ClonePack:
                SaveManager.NumOfClonePacks++;
                break;
            case PowerupType.HealthPack:
                SaveManager.NumOfHealthPacks++;
                break;
            default:
                PowerupStatus.Clone = true;
                break;
        }
    }
}
