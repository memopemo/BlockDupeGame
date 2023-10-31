using System;
using System.Collections;
using System.Collections.Generic;
using Animator2D;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
     public CollisionBox unaliveBox;
     float maxLedgeHeight;

     public Liftable nearestLiftableObj; //May be null.
     public Liftable carryingObj;
     
     public DefaultPlayerState defaultPlayerState = new();
     public DeadPlayerState deadPlayerState = new();

     public HeldPlayerState heldPlayerState = new();
     //... add more states here.
     

     void Start()
     {
          rigidBody = GetComponent<Rigidbody2D>();
          animator2D = GetComponent<Animator2D.Animator2D>();
          boxCollider = GetComponent<BoxCollider2D>();
          currentState ??= defaultPlayerState;
          currentState.OnEnter(this);
          //can also be simplified to the weird statement: currentState ??= defaultPlayerState;
     }

     void Update()
     {
          currentState.UpdateState(this);
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

     public void Lift()
     {
          if(nearestLiftableObj)
          {
               nearestLiftableObj.OnBeingLifted();
               nearestLiftableObj.transform.SetParent(transform.GetChild(0));
               carryingObj = nearestLiftableObj;
          }
     }
     public void Clone()
     {
          // create the clone at the "lift point"
          var newClone = Instantiate(gameObject, transform.GetChild(0).position, transform.rotation, transform.GetChild(0));
          newClone.GetComponent<PlayerStateManager>().Init(); // init clone

          //lift clone, set nearestLiftableObject so the function knows who to lift.
          nearestLiftableObj = newClone.GetComponent<Liftable>(); 
          Lift();


          rigidBody.AddForce(40f * Vector2.up, ForceMode2D.Impulse);
          
          FindAnyObjectByType<CloneManager>().CreateClone(newClone.GetComponent<PlayerStateManager>());
     }
     public void Init()
     {
          //it has not been initialized, so we need to override the state ourself.
          currentState = heldPlayerState;
          heldPlayerState.OnEnter(this);     
          transform.name = "Player";
     }
     public void ThrowHeldObject()
     {
          if(carryingObj.TryGetComponent(out PlayerStateManager a) && a.currentState == a.heldPlayerState)
          {
               //we die, clone lives!
               ChangeState(deadPlayerState);
               carryingObj.GetComponent<PlayerStateManager>().ChangeState(defaultPlayerState);
          }
          //Set velocity
          if(carryingObj.TryGetComponent(out Rigidbody2D rb))
          {
               rb.velocity = GetThrowVector();
          }

          carryingObj.OnBeingThrown();
          carryingObj.transform.parent = null; //get rid of our parent
          carryingObj = null;
     }
     public Vector2 GetThrowVector()
     {
          float flip = transform.localScale.x; // this is so it matches the direction of the player. ([<-] -1 or 1 [->])
          if(Input.GetAxis("Vertical") > 0) // is holding up
          {
               return Vector2.up * 20;
          }
          else if(Input.GetAxis("Vertical") < 0) // is holding down
          {
               return Vector2.up * 20;
          }
          //TODO: add throw direction change based on holding up, or down.
          return new Vector2(flip, 1)*15;
          
     }

     //Good gravy!
     public bool IsGrounded()
     {
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = ground,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.down, contactFilter2D, results, 0.01f) > 0;
     }
     public bool IsTouchingLeftWall()
     {
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = wall,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size * Vector2.one * 0.8f, 0, Vector2.left, contactFilter2D, results, 0.05f) > 0;
     }
     public bool IsTouchingRightWall()
     {
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = wall,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size * Vector2.one * 0.8f, 0, Vector2.right, contactFilter2D, results, 0.05f) > 0;
     }
     public bool IsTouchingCeiling()
     {
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = wall,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size, 0, Vector2.up, contactFilter2D, results, 0.1f) > 0;
     }
     public bool HasSpaceToLift(BoxCollider2D collider)
     {
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = wall,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset  + Vector2.up, collider.bounds.size, 0, Vector2.zero, contactFilter2D, results, 0) > 0;
     }
     public bool SnapToGround(out float amount)
     {
          ContactFilter2D contactFilter2D = new()
          {
               layerMask = ground,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> resultsl = new(); //dont care
          List<RaycastHit2D> resultsr = new(); //dont care
          Vector2 pos = (Vector2)transform.position;
          float length = boxCollider.bounds.max.y;
          float minLedgeHeight = boxCollider.offset.y;

          Vector2 originr = new Vector2(
               boxCollider.bounds.extents.x + Input.GetAxis("Horizontal") *  8 / 23, 
               boxCollider.bounds.extents.y
               );

          Vector2 originl = new Vector2(
               -boxCollider.bounds.extents.x + Input.GetAxis("Horizontal") * 8 / 23, 
               boxCollider.bounds.extents.y
               );

          var groundr = Physics2D.Raycast(
               pos + originr, 
               Vector2.down, 
               contactFilter2D, 
               resultsl, 
               length
               );

          var groundl = Physics2D.Raycast(
               pos + originl, 
               Vector2.down, 
               contactFilter2D, 
               resultsr, 
               length
               );
          
          Debug.DrawRay(pos+originl, Vector2.down * length);
          Debug.DrawRay(pos+originr, Vector2.down * length);

          bool isTooHigh(float y) => y > minLedgeHeight;

          amount = 0;

          if(resultsr.Count != 0 && !isTooHigh(resultsr[0].point.y))
          {
               amount = resultsr[0].point.y + boxCollider.bounds.extents.y - boxCollider.offset.y;
               return true;
          }
          else if(resultsl.Count != 0 && !isTooHigh(resultsl[0].point.y))
          {
               amount = resultsl[0].point.y + boxCollider.bounds.extents.y - boxCollider.offset.y;
               return true;
          }
          return false;
     }

     [SerializeField]
     public class Animations 
     {
          //Syntax:
          // X0 = Idle,
          // X1 = Run,
          // X2 = Jump,
          // X3 = Fall,
          // X4 = Land,
          // X5 to 9 = Other (not automatically indexed)
          public const byte 
          Idle = 0,
          CarryIdle = 10,
          DuckIdle = 20,
          CloneStruggleIdle = 30,
          ThrowStruggleIdle = 40,  
          Wall = 50,
          LookUpIdle = 60,
          CarryDuckIdle = 70,
          CarryWall = 80,
          CarryLookUpIdle = 90;
     }

    public void OnDrawGizmos()
    {
          //Vector2 pos = (Vector2)transform.position + GetComponent<Collider2D>().offset;
          /*Gizmos.color = Color.magenta;
          Gizmos.DrawWireCube(pos + Vector2.down * 0.5f, GetComponent<Collider2D>().bounds.size);
          Gizmos.color = Color.red;
          Gizmos.DrawWireCube(pos + Vector2.left * 0.1f, GetComponent<Collider2D>().bounds.size);
          Gizmos.color = Color.green;
          Gizmos.DrawWireCube(pos + Vector2.right * 0.1f, GetComponent<Collider2D>().bounds.size);
          Gizmos.color = Color.blue;
          Gizmos.DrawWireCube(pos + Vector2.up * 0.1f, GetComponent<Collider2D>().bounds.size);
          Gizmos.DrawWireCube(pos + Vector2.up, GetComponent<Collider2D>().bounds.size);
          Gizmos.DrawWireCube(pos, GetComponent<Collider2D>().bounds.size*2f); */

    }

}
