using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    public PlayerStateManager currentlyControlledPlayer;
    // Start is called before the first frame update
    void Start()
    {
        currentlyControlledPlayer = FindFirstObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
