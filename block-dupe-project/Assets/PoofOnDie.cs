using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoofOnDie : MonoBehaviour
{
    public GameObject poofObject;
    private bool isQuitting;

    //Had to look this up: OnApplicationQuit is called before OnDestroy()
    public void OnApplicationQuit()
    {
        isQuitting = true;
    }
    public void OnDestroy()
    {
        //Only create gameobject if we are in play mode. (unity gets angry when you create gameobjects OnDestroy)
        if(!isQuitting){
            Instantiate(poofObject,transform.position,transform.rotation);
        }
    }
}
