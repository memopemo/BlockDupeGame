using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TEMP_UICloneCount : MonoBehaviour
{
    public Sprite normalIcon;
    public Sprite fullIcon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CloneManager cm = FindFirstObjectByType<CloneManager>();
        TMP_Text tMP_Text = GetComponentInChildren<TMP_Text>();

        tMP_Text.text = cm.AllClones.Count + " / " + cm.AllowedClones; 
        
        if(cm.AllClones.Count == cm.AllowedClones)
        {
            tMP_Text.color = Color.red;
            GetComponent<Image>().sprite = fullIcon;
        }
        else
        {
            tMP_Text.color = Color.white;
            GetComponent<Image>().sprite = normalIcon;
        }
    }
}
