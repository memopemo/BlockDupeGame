using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Animator2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Animator2D : MonoBehaviour
    {
        public AnimationData[] animations;
        public int currentAnimation;
        private int currentFrameTick;
        private int currentAnimationFrame;
        public void Start()
        {
            GetComponent<SpriteRenderer>().sprite = animations[currentAnimation].AnimationFrames[currentAnimationFrame].SpriteShown;
        }

        public void FixedUpdate()
        {
            if(currentFrameTick == animations[currentAnimation].AnimationFrames[currentAnimationFrame].HoldForTicks)
            {
                GetNextFrame();
            }
            else
            {
                currentFrameTick++;
            }
        }

        public void GetNextFrame()
        {
            currentAnimationFrame += 1;
            //Loop or keep the animation frame on its final frame.
            if(animations[currentAnimation].Looping)
            {
                if (currentAnimationFrame > animations[currentAnimation].LoopEnd)
                {
                    currentAnimationFrame = animations[currentAnimation].LoopStart;
                }
                else if(currentAnimationFrame > animations[currentAnimation].AnimationFrames.Count-1)
                {
                    currentAnimationFrame = animations[currentAnimation].LoopStart;
                }
            }
            if(currentAnimationFrame > animations[currentAnimation].AnimationFrames.Count-1)
            {
                currentAnimationFrame -= 1;
            }

            currentFrameTick = 0;

            GetComponent<SpriteRenderer>().sprite = animations[currentAnimation].AnimationFrames[currentAnimationFrame].SpriteShown;
        }

        public void SetAnimation(int animationIndex, int frame = 0)
        {
            if(animationIndex == currentAnimation) return;
            if(animations.GetUpperBound(0) < animationIndex)
            {
                Debug.LogWarning("i: "+ animationIndex + " is not in range of animations: " + animations.GetUpperBound(0));
                return;
            }
            if(animations[animationIndex].AnimationFrames.Count < frame)
            {
                Debug.LogWarning("frame count: "+ animationIndex + " is not in range of frames: " + animations[animationIndex].AnimationFrames.Count);
                return;
            }

            currentAnimation = animationIndex;
            currentAnimationFrame = frame;
            currentFrameTick = 0;
        }
    }

    [System.Serializable]
    public class AnimFrame
    {
        public Sprite SpriteShown;
        public int HoldForTicks;
    }
}

