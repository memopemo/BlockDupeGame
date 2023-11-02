using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject Panel;
    
    public void OpenPanel(){
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    public void ClosePanel(){
        if (Panel == null)
        {
            Panel.SetActive(false);
        }
    }

}
