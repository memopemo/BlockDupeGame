using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadPlayerState : IPlayerState
{
    float circleRadius = 0.5f;
    float flatTopYPosition = 0.5f;

    public void FixedUpdateState(PlayerStateManager manager)
    {
        if (manager.rigidBody.velocity.x != 0 && manager.IsGrounded())
        {
            // add opposing force to our velocity until its zero.
            manager.rigidBody.AddForce(10 * manager.rigidBody.velocity.x * Vector2.left);
        }
    }

    public void OnEnter(PlayerStateManager manager)
    {
        manager.unaliveBox.SetCollisionBox(manager.boxCollider);

        //set circle for easing into pits
        var circle = manager.AddComponent<CircleCollider2D>();
        circle.radius = circleRadius;

        //set flat top for standing on. (for some reason generating )
        var edgeCollider = manager.AddComponent<EdgeCollider2D>();
        edgeCollider.offset = Vector2.up * flatTopYPosition;

        List<Vector2> pointList = new List<Vector2>
        {
            Vector2.left * 0.5f,
            Vector2.right * 0.5f
        };

        edgeCollider.SetPoints(pointList);
        manager.transform.name = "DeadPlayer";
        
        manager.animator2D.SetAnimation(6);
        manager.gameObject.layer = 6;
    }

    public void OnExit(PlayerStateManager manager)
    {
    }

    public void UpdateState(PlayerStateManager manager)
    {
        manager.unaliveBox.SetCollisionBox(manager.boxCollider);
        manager.animator2D.SetAnimation(6);
        
    }
}
