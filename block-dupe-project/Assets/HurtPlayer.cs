using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damageAmount;
    public float bounceForce = 10f;
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent(out PlayerStateManager player) && player.currentState is DefaultPlayerState)
        {
            player.TakeDamage(damageAmount, (player.transform.position - transform.position).x > 0);

            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            }
        }
    }
}
