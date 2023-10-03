using System;
using System.Collections;
using System.Collections.Generic;
using Animator2D;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
     public IPlayerState currentState;
     public Rigidbody2D rigidBody;

     [SerializeField] LayerMask ground;

     [SerializeField] LayerMask wall;

     public bool direction;
     public Animator2D.Animator2D animator2D;
     public BoxCollider2D boxCollider;

     [Serializable]
     public class CollisionBox
     {
          public Vector2 size;
          public Vector2 offset;
          public void SetCollisionBox(BoxCollider2D boxCollider2D)
          {
               boxCollider2D.size = size;
               boxCollider2D.offset = offset;
          }
     }

     public CollisionBox normalBox;
     public CollisionBox duckBox;
     public CollisionBox carryBox;
     public CollisionBox carryDuckBox;

     public bool carryingObj;
     
     public DefaultPlayerState defaultPlayerState = new DefaultPlayerState();
     //... add more states here.
     

     void Start()
     {
          rigidBody = GetComponent<Rigidbody2D>();
          animator2D = GetComponent<Animator2D.Animator2D>();
          boxCollider = GetComponent<BoxCollider2D>();
          currentState = defaultPlayerState;
     }

     void Update()
     {
          currentState.UpdateState(this);

          if(Input.GetAxisRaw("Horizontal") != 0)
          {
               transform.localScale = new Vector3 (Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
               direction = Input.GetAxisRaw("Horizontal") > 0;
          }
     }
     void FixedUpdate()
     {
          currentState.FixedUpdateState(this);
     }
     public void ChangeState(IPlayerState newState)
     {
          currentState.OnExit(this);
          currentState = newState;
          currentState.OnEnter(this);
     }
     public bool IsGrounded()
     {
        // if(col) Debug.DrawLine((Vector2)transform.position + GetComponent<Collider2D>().offset, col.point, Color.magenta);
        return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.down, 0.5f, ground);
     }
     public bool IsTouchingLeftWall()
     {
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.left, 0.1f, wall);
     }
     public bool IsTouchingRightWall()
     {
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.right, 0.1f, wall);
     }
     public bool IsTouchingCeiling()
     {
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.up, 0.1f, wall);
     }
     public bool HasSpaceToLift(BoxCollider2D collider)
     {
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset  + Vector2.up, collider.bounds.size, 0, Vector2.zero, 0, wall);
     }
     [SerializeField]
     public class Animations 
     {
          public const byte 
          Idle = 0, Run = 1, Jump = 2, Fall = 3, Land = 4, IdleLookUp = 5,
          CarryIdle = 10, CarryRun = 11, CarryJump = 12, CarryFall = 13, CarryLand = 14,
          DuckIdle = 20, DuckRun = 21,
          CloneStruggleIdle = 30, CloneStruggleRun = 31, CloneStruggleJump = 32, CloneStruggleFall = 33, CloneStruggleLand = 34,
          ThrowStruggleIdle = 40, ThrowStruggleRun = 41, ThrowStruggleJump = 42, ThrowStruggleFall = 43, ThrowStruggleLand = 44,   
          WallRun = 51, WallJump = 52,
          LookUpIdle = 60,
          CarryDuckIdle = 70, CarryDuckRun = 71,
          CarryWallRun = 81, CarryWallJump = 82,
          CarryLookUpIdle = 90;
     }

    public void OnDrawGizmos()
    {
        Vector2 pos = (Vector2)transform.position + GetComponent<Collider2D>().offset;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(pos + Vector2.down * 0.5f, GetComponent<Collider2D>().bounds.size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos + Vector2.left * 0.1f, GetComponent<Collider2D>().bounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos + Vector2.right * 0.1f, GetComponent<Collider2D>().bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos + Vector2.up * 0.1f, GetComponent<Collider2D>().bounds.size);
    }

}
