using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScrollingBG : MonoBehaviour
{
    public Vector2 directionAndSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (Vector3)directionAndSpeed*Time.fixedDeltaTime;
    }
}
