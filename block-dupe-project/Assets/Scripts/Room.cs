using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    public EntranceScene[] entrances;
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.AssertFormat(entrances.Length != 0, "entrances is empty.");
        Debug.AssertFormat(PersistentExitData.Instance.exitNum <= entrances.GetUpperBound(0), "index: " + PersistentExitData.Instance.exitNum + "/" + entrances.GetUpperBound(0));

        FindFirstObjectByType<PlayerStateManager>().transform.position = entrances[PersistentExitData.Instance.exitNum].transform.position;
        Destroy(PersistentExitData.Instance.gameObject); //not needed anymore.
        FindAnyObjectByType<BGMusicController>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchScenes(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }
}
