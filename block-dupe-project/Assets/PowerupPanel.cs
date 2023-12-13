using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupPanel : MonoBehaviour
{
    int waitFrames = 0;
    const int showForFrames = 200;
    public void Show(Powerup.PowerupType powerupType)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        waitFrames = 0;
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        switch (powerupType)
        {
            case Powerup.PowerupType.Clone:
                text.text = "Powerup Obtained: Clone\nPress and release ctrl to create a clone.";
                break;
            case Powerup.PowerupType.Metal:
                text.text = "Powerup Obtained: Metal Slam\nHold ctrl down for a brief period to create a metal clone.";
                break;
            case Powerup.PowerupType.Straight:
                text.text = "Powerup Obtained: Straight Shot\nHold ctrl down for a brief period to throw straight.";
                break;
            case Powerup.PowerupType.Midair:
                text.text = "Powerup Obtained: Midair Duplication";
                break;
            case Powerup.PowerupType.JumpHold:
                text.text = "Powerup Obtained: Jump Hold\nYou can now jump while holding something.";
                break;
            case Powerup.PowerupType.ClonePack:
                text.text = "Powerup Obtained: Clone Pack\n Your maximum clones has been increased by 3.";
                break;
            case Powerup.PowerupType.HealthPack:
            text.text = "Powerup Obtained: Health Pack\n Your maximum health has been increased by 2.";
                break;
        }
    }
    void ShowContinue()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    void Close()
    {
        gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    //This is
    public void Update()
    {
        if((Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Escape)) && waitFrames > showForFrames)
        {
            Close();
        }
        waitFrames++;
        if(waitFrames > showForFrames)
        {
            ShowContinue();
        }

    }


}
