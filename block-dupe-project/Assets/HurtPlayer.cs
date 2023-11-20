using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damageAmount;
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent(out PlayerStateManager player) && player.currentState is DefaultPlayerState)
        {
            player.TakeDamage(damageAmount, (player.transform.position - transform.position).x > 0);
        }
    }
}
