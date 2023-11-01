using System;
using System.Collections.Generic;
using UnityEditor;
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
     float distanceCheckForCollision = 0.1f;
     float wallCollisionScale = 0.8f;

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
     


     //Good gravy! Lotta boxes!
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
          return Physics2D.BoxCast(
               (Vector2)transform.position + boxCollider.offset, 
               boxCollider.bounds.size, 
               0,
               Vector2.down, 
               contactFilter2D, 
               results, 
               distanceCheckForCollision) 
               > 0;
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
          return Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset, boxCollider.bounds.size - Vector3.up * wallCollisionScale, 0, Vector2.left, contactFilter2D, results, distanceCheckForCollision) > 0;
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
          List<RaycastHit2D> results = new(); //dont care about results

          int numberOfHits = Physics2D.BoxCast(
               (Vector2)transform.position + boxCollider.offset,           //origin
               boxCollider.bounds.size - Vector3.one * wallCollisionScale, //boxsize
               0,                                                          //angle
               Vector2.right,                                              //direction
               contactFilter2D,                                            //filter
               results,                                                    //each hit's details
               distanceCheckForCollision);                                 //distance
               
          return numberOfHits > 0;
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
          
          int numberOfHits = Physics2D.BoxCast(
               (Vector2)transform.position + boxCollider.offset, //origin     
               boxCollider.bounds.size,                          //boxsize
               0,                                                //angle
               Vector2.up,                                       //direction
               contactFilter2D,                                  //filter
               results,                                          //each hit's details
               distanceCheckForCollision);                       //distance

          return numberOfHits > 0;
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

     //Debug Box Collision Drawing
     public void OnDrawGizmos()
     {
          if(boxCollider == null) return;

          Vector2 pos = (Vector2)transform.position + boxCollider.offset; // get position shorthand

          //Ground
          Gizmos.color = Color.magenta * 0.5f;
          Gizmos.DrawWireCube(pos + Vector2.down * distanceCheckForCollision, boxCollider.bounds.size);

          // Left Wall
          Gizmos.color = Color.red * 0.5f;
          Gizmos.DrawWireCube(pos + Vector2.left * distanceCheckForCollision, boxCollider.bounds.size - Vector3.up *wallCollisionScale);

          //Right Wall
          Gizmos.color = Color.green * 0.5f;
          Gizmos.DrawWireCube(pos + Vector2.right * distanceCheckForCollision, boxCollider.bounds.size - Vector3.up *wallCollisionScale);

          //Ceiling
          Gizmos.color = Color.blue * 0.5f;
          Gizmos.DrawWireCube(pos + Vector2.up * distanceCheckForCollision, boxCollider.bounds.size);
     }

}
