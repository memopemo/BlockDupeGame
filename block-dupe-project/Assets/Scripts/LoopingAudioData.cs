using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Song", menuName = "Looping Song", order = 2)]
public class LoopingAudio : ScriptableObject
{
    /* Audio Structure

    Intro | Loop 1 |[LoopStartSample]| Loop 2

    This is to capture the transition between loops, and not between intro and loop.
    Because they may be different due to echos or some other bullshit from the intro.

    Fuck you unity. You really have to make shit this fucking stupid because:
    1. You legit cannot loop a song without a delay

    */
    public AudioClip song;
    public int LoopStartSample;
}
