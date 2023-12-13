using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Since most areas have congealed music, put this on an Exit object when music needs to be changed.
public class MusicSwitch : MonoBehaviour
{
    public int nextRoomMusicID;

    public void OnSwitch()
    {
        BGMusicController music = FindFirstObjectByType<BGMusicController>();
        if(music.nextSongID == nextRoomMusicID) return;
        music.nextSongID = nextRoomMusicID;
        music.FadeOut();

    }
}
