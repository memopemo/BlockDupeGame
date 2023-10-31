using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animator2D;

[CreateAssetMenu(fileName = "Anim2D", menuName = "AnimationData", order = 1)]
public class AnimationData: ScriptableObject
{
    public string StateName;
    public List<AnimFrame> AnimationFrames;
    public bool Looping;
    public int LoopStart;
    public int LoopEnd;
    public Vector2 HeldItemPosition;
}

