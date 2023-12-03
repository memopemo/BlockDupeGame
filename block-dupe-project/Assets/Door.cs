using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public void Open()
    {
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Animator>().SetTrigger("Open");
    }
    public void Close()
    {
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Animator>().SetTrigger("Close");

    }

}
