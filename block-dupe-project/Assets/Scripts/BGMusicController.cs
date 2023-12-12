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
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMs[0].song;
        audioSource.Play();
    }
    void Update()
    {
        if(audioSource.time >= audioSource.clip.length)
        {
            audioSource.timeSamples = BGMs[currentSongID].LoopStartSample;
            audioSource.Play();
        }
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume",1);
    }
    public void FadeOut()
    {
        StartCoroutine(DecreaseVolume());
    }    
    IEnumerator DecreaseVolume()
    {
        while (audioSource.volume != 0)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        SwitchBGM();
    }

    public void SwitchBGM()
    {
        if(currentSongID == nextSongID)
        {
            return;
        }
        audioSource.clip = BGMs[nextSongID].song;
        currentSongID = nextSongID;
    }
}
