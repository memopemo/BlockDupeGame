using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TEMPHealthNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CloneManager cm = FindFirstObjectByType<CloneManager>();
        GetComponent<TMP_Text>().text = "Health: "+ cm.currentlyControlledPlayer.health + " / " + cm.currentlyControlledPlayer.maxHealth; 
    }
}
