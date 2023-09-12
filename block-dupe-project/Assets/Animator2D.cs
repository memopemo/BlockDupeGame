using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Animator2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Animator2D : MonoBehaviour
    {
        public AnimationData[] animations;
        [SerializeField] private int currentAnimation;
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

    }

    [System.Serializable]
    public class AnimFrame
    {
        public Sprite SpriteShown;
        public int HoldForTicks;
    }
}

