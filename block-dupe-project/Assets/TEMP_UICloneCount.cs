using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TEMP_UICloneCount : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CloneManager cm = FindFirstObjectByType<CloneManager>();
        GetComponent<TMP_Text>().text = "Clones: "+ cm.AllClones.Count + " / " + cm.AllowedClones; 
    }
}
