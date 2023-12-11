using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LoadGamePanel : MonoBehaviour
{
    GameObject[] Saves;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TickedUpdate),0.5f, 0.5f);
    }
    public void DeleteGame(int number)
    {
        string saveFileName = Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav";
        File.Delete(saveFileName);
    }

    // Update is called once per frame
    void TickedUpdate()
    {
        string dmy;
        string area;
        GetFileSave(1, out dmy, out area);
        transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = area;
        transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = dmy;
        GetFileSave(2, out dmy, out area);
        transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = area;
        transform.GetChild(2).GetChild(2).GetComponent<TMP_Text>().text = dmy;
        GetFileSave(3, out dmy, out area);
        transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = area;
        transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = dmy;
    }
    void GetFileSave(int number, out string dayMonthYear, out string areaName)
    {
        FileStream fileStream;
        string saveFileName = Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav";
        try
        {
            fileStream = File.OpenRead(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
        }
        catch (Exception)
        {
            areaName = "New Game";
            dayMonthYear = "";
            return;
        }
        
        DateTime dt = File.GetLastWriteTime(saveFileName);
        dayMonthYear = dt.ToString("d/MMM/yyyy");
        fileStream = File.OpenRead(Application.persistentDataPath + Path.DirectorySeparatorChar + number + ".sav");
        StreamReader streamReader = new(fileStream);
        string SaveScene = streamReader.ReadLine();
        if(SaveScene.StartsWith("Furnace"))
        {
            areaName = "Furnace";
            goto Exit;
        }
        if(SaveScene.StartsWith("Factory"))
        {
            areaName = "Factory";
            goto Exit;
        }
        if(SaveScene.StartsWith("Warehouse"))
        {
            areaName = "Warehouse";
            goto Exit;
        }
        areaName = "Somewhere";

        Exit:
        fileStream.Close();
        return;
    }

}
