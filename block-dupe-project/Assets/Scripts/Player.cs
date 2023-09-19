using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Animator2D;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public static class State {
        public const byte Idle = 0, Run = 1, Skid = 2, 
        Jump = 3, Fall = 4, Land = 5, 
        Duck=6, DuckIdle=7,DuckWalk=8, UnDuck=9, 
        Lift=10, Throw=11, 
        CarryIdle=12, CarryRun=13, CarrySkid=14,
        CarryJump=15, CarryFall=16, CarryLand=17, CarryBounce=18,
        CarryDuck=19, CarryUnDuck=20, Duplicate=21, MidairDuplicate=22, MidairThrow=23,
        WallRun=24, MidairWall=25, CarryWallRun=26, CarryMidairWall=27, 
        SpeedThrown=28, MetalThrown=29, 
        NotAliveYet=30, DeadObj=31, DeadClone=32,
        IdleLookingUp=33, CarryIdleLookingUp=34;
        };
    
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wall;
    [SerializeField] int jumpHeight;
    [SerializeField] int runSpeed;
    [SerializeField] int maxSpeed;
    bool direction;
    Animator2D.Animator2D animator2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator2D = GetComponent<Animator2D.Animator2D>();
    }
    void Update(){
        // Vertical Movement
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpHeight * 20f, ForceMode2D.Impulse);
        }
        
        print(
            "B" + (IsGrounded() ? "1 " : "0 ") +
            "L" + (IsTouchingLeftWall() ? "1 " : "0 ") +
            "R" + (IsTouchingRightWall() ? "1 " : "0 ") +
            "T" + (IsTouchingCeiling() ? "1 " : "0 ") 
        );

        //Animation, (Yes, I should probably make a state machine)
        
        if((Input.GetAxisRaw("Vertical") < 0 && IsGrounded()) || (IsGrounded() && IsTouchingCeiling() && Mathf.Abs(rigidBody.velocity.y) < 0.2f ))
        {
            GetComponent<BoxCollider2D>().size = Vector2.one * 0.95f;
            GetComponent<BoxCollider2D>().offset = Vector2.down * 0.45f;
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                animator2D.SetAnimation(State.DuckWalk);
            }
            else if(animator2D.currentAnimation == State.DuckWalk)
            {
                animator2D.SetAnimation(State.DuckIdle,2);
            }
            else
            {
                animator2D.SetAnimation(State.DuckIdle);
            }
            return;
        }
        if(!IsGrounded())
        {
            if(IsTouchingRightWall() && direction || IsTouchingLeftWall() && !direction)
            {
                animator2D.SetAnimation(State.MidairWall);
            }
            else
            {
                if(rigidBody.velocity.y > 0)
                {
                    animator2D.SetAnimation(State.Jump);
                }
                else
                {
                    animator2D.SetAnimation(State.Fall);
                }
            }
            return;
        }
        if(Input.GetAxisRaw("Vertical") > 0 && IsGrounded() && Input.GetAxisRaw("Horizontal") == 0)
        {
            animator2D.SetAnimation(State.IdleLookingUp);
            
            return;
        }
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            if(IsTouchingRightWall() && direction || IsTouchingLeftWall() && !direction)
            {
                animator2D.SetAnimation(State.WallRun);
            }
            else
            {
                animator2D.SetAnimation(State.Run);
            }
            
        }
        else
        {
            animator2D.SetAnimation(State.Idle);
        }
        GetComponent<BoxCollider2D>().size = new Vector2(0.95f, 1.31f);
        GetComponent<BoxCollider2D>().offset = Vector2.down * 0.25f;
        
        

    }
    public void OnAnimationEnd()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Horizontal Movement
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            if(Mathf.Abs(rigidBody.velocity.x) < runSpeed)
            {
                rigidBody.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal") * 10 * runSpeed);
            }
        }
        else if(rigidBody.velocity.x != 0)
        {
            // add opposing force to our velocity until its zero.
            rigidBody.AddForce(Vector2.left * rigidBody.velocity.x * 10); 
        }

        if(Input.GetAxisRaw("Horizontal") != 0){
            transform.localScale = new Vector3 (Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
            direction = Input.GetAxisRaw("Horizontal") > 0;
        }

        
        // Horizontal Speed Capping
        if(Mathf.Abs(rigidBody.velocity.x) > maxSpeed || Mathf.Abs(rigidBody.velocity.y) > maxSpeed)
        {
            rigidBody.velocity = new Vector2(
                Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), 
                Mathf.Clamp(rigidBody.velocity.y, -maxSpeed, maxSpeed)
                );
        }
    }
    public bool IsGrounded()
    {
        var col = Physics2D.BoxCast((Vector2)transform.position + GetComponent<Collider2D>().offset, GetComponent<Collider2D>().bounds.size, 0, Vector2.down, 0.5f, ground);
        if(col) Debug.DrawLine((Vector2)transform.position + GetComponent<Collider2D>().offset, col.point, Color.magenta);
        return col;
    }
    public bool IsTouchingLeftWall()
    {
        return Physics2D.BoxCast((Vector2)transform.position + GetComponent<Collider2D>().offset, GetComponent<Collider2D>().bounds.size, 0, Vector2.left, 0.1f, wall);
    }
    public bool IsTouchingRightWall()
    {
        return Physics2D.BoxCast((Vector2)transform.position + GetComponent<Collider2D>().offset, GetComponent<Collider2D>().bounds.size, 0, Vector2.right, 0.1f, wall);
    }
    public bool IsTouchingCeiling()
    {
        return Physics2D.BoxCast((Vector2)transform.position + GetComponent<Collider2D>().offset, GetComponent<Collider2D>().bounds.size, 0, Vector2.up, 0.1f, wall);
    }
    public bool HasSpaceToLift(BoxCollider2D collider)
    {
        return Physics2D.BoxCast((Vector2)transform.position + GetComponent<Collider2D>().offset  + Vector2.up, collider.bounds.size, 0, Vector2.zero, 0, wall);
    }
 
    //Drawing Box Checks
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
