using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{

    //TODO: make it so that this only checks if its of a type, because we will have many different player clone gameobjects.
    [SerializeField] GameObject[] listenFor;
    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(listenFor.Contains(collider.gameObject))
            onEnter.Invoke();
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(listenFor.Contains(collider.gameObject))
            onExit.Invoke();
    }
    
}
