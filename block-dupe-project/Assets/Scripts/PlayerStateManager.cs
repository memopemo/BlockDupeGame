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

     private enum DebugState{Default, Thrown, Dead, Held}
     [SerializeField] private DebugState debugState;

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

     public float timeForActivatingPowerup = 0.5f;

     public int health;
     public int maxHealth;
     private float secDamageCooldown;
     public float secDamageTime;

     public Liftable nearestLiftableObj; //May be null.
     public Liftable carryingObj;

     public GameObject NormalPlayer;
     public GameObject MetalPlayer;
     
     public DefaultPlayerState defaultPlayerState = new();

     public DeadPlayerState deadPlayerState = new();

     public HeldPlayerState heldPlayerState = new();

     public ThrownPlayerState thrownPlayerState = new();
     //... add more states here.
     

     void Start()
     {
          rigidBody = GetComponent<Rigidbody2D>();
          animator2D = GetComponent<Animator2D.Animator2D>();
          boxCollider = GetComponent<BoxCollider2D>();
          currentState ??= defaultPlayerState;
          currentState.OnEnter(this);
          maxHealth = 3 * (SaveManager.NumOfHealthPacks + 1);
          health = maxHealth;
          //can also be simplified to the weird statement: currentState ??= defaultPlayerState;
     }

     void Update()
     {
          currentState.UpdateState(this);

          switch (currentState)
          {
               case DeadPlayerState:
                    debugState = DebugState.Dead;
                    break;
               case DefaultPlayerState:
                    debugState = DebugState.Default;
                    break;
               case ThrownPlayerState:
                    debugState = DebugState.Thrown;
                    break;
               case HeldPlayerState:       
                    debugState = DebugState.Held;        
                    break;
          }
          if(CanBreakBelow(out MetalBreakable breakable))
          {
               Destroy(breakable.gameObject);
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

     public void Lift()
     {
          if(nearestLiftableObj)
          {
               nearestLiftableObj.OnBeingLifted();
               nearestLiftableObj.transform.SetParent(transform.GetChild(0));
               carryingObj = nearestLiftableObj;
          }
     }

     public void Clone(bool metal)
     {
          print(metal);
          // create the clone at the "lift point" the single line that makes magic
          var newClone = Instantiate(metal ? MetalPlayer : NormalPlayer, transform.GetChild(0).position, transform.rotation);

          PlayerStateManager newClonePlayer = newClone.GetComponent<PlayerStateManager>();
          newClonePlayer.Init(); // init clone
          newClonePlayer.health = health;
          
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

     public void ThrowHeldObject(bool straight)
     {
          //Throw vector
          Vector2 ThrowVector = GetThrowVector(straight);

          bool isHeldPlayer = carryingObj.TryGetComponent(out PlayerStateManager a) && a.currentState == a.heldPlayerState;

          if(Input.GetAxis("Vertical") < 0) //down
          {
               carryingObj.transform.localPosition = Vector3.down*1;
          }

          //SPECIFIC ORDER TIME

          //This NEEDS to be BEFORE changing to thrown player state because thrown relies on checking if we are straight thrown.
          if(straight && !(IsGrounded() && Input.GetAxisRaw("Vertical") < 0))
          {
               carryingObj.StraightThrow(ThrowVector);    
          }

          if(isHeldPlayer)
          {
               //we die, clone lives!
               ChangeState(deadPlayerState);
               a.ChangeState(thrownPlayerState);
               a.direction = direction;
               FindFirstObjectByType<CloneManager>().currentlyControlledPlayer = a;
          }

          //Send object being thrown that we are throwing
          carryingObj.OnBeingThrown(gameObject);
          
          //This NEEDS to be AFTER changing to thrown state because exiting held state sets our rigidbody type back to dynamic from static.
          //Set velocity
          if(carryingObj.TryGetComponent(out Rigidbody2D rb))
          {
               rb.velocity = ThrowVector;   
          }

          

          //Done with object.
          carryingObj.transform.parent = null; //get rid of our parent
          carryingObj = null;
     }    

     public Vector2 GetThrowVector(bool straight)
     {
          float flip = transform.localScale.x; // this is so it matches the direction of the player. ([<-] -1 or 1 [->])
          if(Input.GetAxis("Vertical") > 0) // is holding up
          {

               return Vector2.up * 20 + 20 * Input.GetAxis("Horizontal") * Vector2.right;
          }
          else if(Input.GetAxis("Vertical") < 0) // is holding down
          {
               if(IsGrounded())
               {
                    return Vector2.down;
               }
               return Vector2.down * 20;
          }
          if(straight)
          {
               return new Vector2(flip, 0)*15;
          }
          return new Vector2(flip, 1)*15;      
     }
     
     public void UpdateHealth()
     {
          if(health > maxHealth)
          {
               health = maxHealth;
          }
          if(health <= 0)
          {
               Die();
          }
          if(secDamageCooldown > 0)
          {
               secDamageCooldown -= Time.deltaTime;
          }
     }

     // Called by enemies and hazards.
     public void TakeDamage(int amount, bool hitDirection)
     {
          if(secDamageCooldown <= 0)
          {
               health -= amount;
                //knockback jump       
               secDamageCooldown = secDamageTime;
          }
          rigidBody.AddForce(40f * Vector2.up, ForceMode2D.Impulse);

     }
     public void RegainHealth(int amount)
     {
          health += amount;
     }
     public void MaxOutHealth()
     {
          health = maxHealth;
     }
     public void Die()
     {
          rigidBody.AddForce(40f * Vector2.up , ForceMode2D.Impulse);
          if(carryingObj)
               ReleaseCuzDead();
          ChangeState(new DeadPlayerState());
     }
     public void ReleaseCuzDead()
     {
          print("hello!");
          bool isHeldPlayer = carryingObj.TryGetComponent(out PlayerStateManager a) && a.currentState == a.heldPlayerState;

          if(Input.GetAxis("Vertical") < 0) //down
          {
               carryingObj.transform.localPosition = Vector3.down*1;
          }

          if(isHeldPlayer)
          {
               //we die, clone lives!
               ChangeState(deadPlayerState);
               a.ChangeState(thrownPlayerState);
               a.direction = direction;
               FindFirstObjectByType<CloneManager>().currentlyControlledPlayer = a;
          }

          //Send object being thrown that we are throwing
          carryingObj.OnBeingThrown(gameObject);
          
          //This NEEDS to be AFTER changing to thrown state because exiting held state sets our rigidbody type back to dynamic from static.
          //Set velocity
          if(carryingObj.TryGetComponent(out Rigidbody2D rb))
          {
               rb.velocity = Vector2.zero;
          }

          //Done with object.
          carryingObj.transform.parent = null; //get rid of our parent
          carryingObj = null;
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
     public bool CanBreakBelow(out MetalBreakable breakable)
     {
          breakable = null;
          if(currentState != defaultPlayerState) return false;
          if(!TryGetComponent(out Conductive _)) return false;
          ContactFilter2D contactFilter2D = new ContactFilter2D
          {
               layerMask = ground,
               useTriggers = false,
               useLayerMask = true
               //Other settings that may come up later
          };
          List<RaycastHit2D> results = new(); //dont care
          var a = Physics2D.BoxCast(
               (Vector2)transform.position + boxCollider.offset, 
               boxCollider.bounds.size, 
               0,
               Vector2.down, 
               contactFilter2D, 
               results, 
               distanceCheckForCollision) 
               > 0;
          foreach (var item in results)
          {
               if(item.collider.TryGetComponent(out MetalBreakable b))
               {
                    breakable = b;
                    return true;
               }
          }
          return false;
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
          int amount = Physics2D.BoxCast((Vector2)transform.position + boxCollider.offset  + Vector2.up, collider.bounds.size, 0, Vector2.zero, contactFilter2D, results, 0);
          foreach (var item in results)
          {
               if(item.collider.gameObject == nearestLiftableObj.gameObject)
               {
                    amount--;
               }
          }
          return amount > 0;
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
