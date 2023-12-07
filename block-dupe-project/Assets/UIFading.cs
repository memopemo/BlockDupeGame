using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Tells a fadeout object on the canvas to either become opaque or transparent over time.
 * This is so that camera 
 *
 * 
 */
public class UIFading : MonoBehaviour
{
    public Color fadeColor = Color.black;
    float targetFade = 1; //information hiding. can only set to 0 or 1.
    private RawImage img;
    public bool done {get; private set;}
    //awake because i want this to be loaded in sooner

    void Awake()
    {
        img = GetComponent<RawImage>();
        img.color = fadeColor;
        Invoke(nameof(WaitOnEnterSceneFadeIn),0.35f);

    }
    void WaitOnEnterSceneFadeIn()
    {
        StartCoroutine(nameof(FadeIn));
    }
    public IEnumerator FadeOut() 
    {
        done = false;
        for (float fadeTime = 0; fadeTime <= 1+0.1f; fadeTime += 0.1f) 
        {
            Color c = img.color;
            c.a = fadeTime;
            img.color = c;
            yield return new WaitForSecondsRealtime(.03f);
        }
        done = true;
    }   
    public IEnumerator FadeIn() 
    {
        done = false;
        for (float fadeTime = 1f; fadeTime >= 0-0.1f; fadeTime -= 0.1f) 
        {
            Color c = img.color;
            c.a = fadeTime;
            img.color = c;
            yield return new WaitForSecondsRealtime(.03f);
        }
        done = true;
    }   
}


/*
The missile knows where it is at all times. It knows this because it knows where it isn't. 
By subtracting where it is from where it isn't, or where it isn't from where it is - whichever is greater - 
it obtains a difference or deviation. 
The guidance subsystem uses deviations to generate corrective commands to drive the missile from a position where it is to a position where it isn't, 
and arriving at a position that it wasn't, it now is. Consequently, 
the position where it is is now the position that it wasn't, 
and if follows that the position that it was is now the position that it isn't. 
In the event that the position that the position that it is in is not the position that it wasn't, 
the system has acquired a variation. The variation being the difference between where the missile is and where it wasn't. 
If variation is considered to be a significant factor, it too may be corrected by the GEA. 
However, the missile must also know where it was. The missile guidance computer scenario works as follows: 
Because a variation has modified some of the information that the missile has obtained, 
it is not sure just where it is. However, it is sure where it isn't, within reason, and it know where it was. 
It now subtracts where it should be from where it wasn't, or vice versa. 
And by differentiating this from the algebraic sum of where it shouldn't be and where it was, 
it is able to obtain the deviation and its variation, which is called error.
*/