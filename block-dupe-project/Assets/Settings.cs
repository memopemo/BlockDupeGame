using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider game;
    public Slider mus;
    public void Start()
    {
        mus.value = PlayerPrefs.GetFloat("GameVolume",1);
        game.value = PlayerPrefs.GetFloat("MusicVolume",1);
    }

    //Has to be a System.Single so that Unity Slider Objects can recieve the value automatically.
    public void SetGameVolume(System.Single vol)
    {
        PlayerPrefs.SetFloat("GameVolume",vol);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(System.Single vol)
    {
        PlayerPrefs.SetFloat("MusicVolume",vol);
        PlayerPrefs.Save();
    }
    public void ChangeWindow(Int32 i)
    {
        switch (i)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;

        }
    }


    public void CloseAndSave()
    {
        
    }

}
