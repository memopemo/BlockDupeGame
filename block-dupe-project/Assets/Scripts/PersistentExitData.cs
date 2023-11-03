//This saves exit data between scenes.

using UnityEngine;

class PersistentExitData : MonoBehaviour
{
    public static PersistentExitData Instance;
    public int exitNum;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}