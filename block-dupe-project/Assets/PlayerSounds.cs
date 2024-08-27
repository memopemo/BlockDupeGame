using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    AudioSource source;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip struggle;
    public AudioClip throwNormal;
    public AudioClip throwStraight;
    public AudioClip clone;
    public AudioClip cloneMetal;
    public AudioClip metal;
    public AudioClip duck;
    public AudioClip unduck;
    public AudioClip oof;
    public AudioClip step;
    public AudioClip skid;


    public void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayJump()
    {
        source.PlayOneShot(jump);
    }
    public void PlayLand()
    {
        source.PlayOneShot(land);
    }
    public void PlayStruggle()
    {
        source.PlayOneShot(struggle);
    }
    public void PlayThrow()
    {
        source.PlayOneShot(throwNormal);
    }
    public void PlayStraight()
    {
        source.PlayOneShot(throwStraight);
    }
    public void PlayClone()
    {
        source.PlayOneShot(clone);
    }
    public void PlayCloneMetal()
    {
        source.PlayOneShot(cloneMetal);
    }
    public void PlayMetal()
    {
        source.PlayOneShot(metal);
    }
    public void PlayDuck()
    {
        source.PlayOneShot(duck);
    }
    public void PlayUnduck()
    {
        source.PlayOneShot(unduck);
    }
    public void PlayGeneric()
    {
        source.PlayOneShot(oof);
    }
    public void PlayStep()
    {
        source.PlayOneShot(step);
    }
    public void PlaySkid()
    {
        source.PlayOneShot(skid);
    }
}
