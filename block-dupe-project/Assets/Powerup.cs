using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent(out PlayerStateManager player) && player.currentState is DefaultPlayerState )
        {
            Collect();
        }
    }
    public void Collect()
    {
        //Trigger UI
        //Switch Powerup ON
        //Close UI
    }
}
