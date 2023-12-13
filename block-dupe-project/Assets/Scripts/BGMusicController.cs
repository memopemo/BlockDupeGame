using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    // This object is always loaded in the game in the DontDestroyOnLoad scene
    public LoopingAudio[] BGMs;
    AudioSource audioSource;
    public int currentSongID;
    public int nextSongID;
    public bool canManuallyChangeVolume;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMs[0].song;
        audioSource.Play();
        canManuallyChangeVolume = true;
    }
    void Update()
    {
        if(audioSource.time >= audioSource.clip.length)
        {
            audioSource.timeSamples = BGMs[currentSongID].LoopStartSample;
            audioSource.Play();
        }
        if(canManuallyChangeVolume)
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume",1);
        }
        
    }
    public void FadeOut()
    {
        canManuallyChangeVolume = false;
        StartCoroutine(DecreaseVolume());
    }    
    IEnumerator DecreaseVolume()
    {
        for(float i = audioSource.volume; i >= 0; i -= 0.1f)
        {
            audioSource.volume = i;
            print("yo");
            yield return null;
        }
        SwitchBGM();
    }
    public void FadeIn()
    {
       StartCoroutine(IncreaseVolume()); 
       canManuallyChangeVolume = true;
    }
    IEnumerator IncreaseVolume()
    {
        for(float i = audioSource.volume; i <= PlayerPrefs.GetFloat("MusicVolume",1); i += 0.1f)
        {
            audioSource.volume = i;
            print("hi");
            yield return null;
        }
    }

    public void SwitchBGM()
    {
        if(currentSongID == nextSongID)
        {
            return;
        }
        audioSource.clip = BGMs[nextSongID].song;
        currentSongID = nextSongID;
        audioSource.Play();
    }
}
