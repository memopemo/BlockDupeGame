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
        NotAliveYet=30, DeadObj=31, DeadClone=32;
        };
    
    public enum _State:int {Idle, Run, Skid, 
        Jump, Fall, Land, 
        Duck, DuckIdle,DuckWalk, UnDuck, 
        Lift, Throw, 
        CarryIdle, CarryRun, CarrySkid,
        CarryJump, CarryFall, CarryLand, CarryBounce,
        CarryDuck, CarryUnDuck, Duplicate, MidairDuplicate, MidairThrow,
        WallRun, MidairWall, CarryWallRun, CarryMidairWall, 
        SpeedThrown, MetalThrown, 
        NotAliveYet, DeadObj, DeadClone}
    
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(Vector2.up * jumpHeight * 20f, ForceMode2D.Impulse);
        }
        if(Input.GetKeyUp(KeyCode.Space) && rigidBody.velocity.y > 3)
        {
            rigidBody.AddForce(Vector2.down * jumpHeight * 20f/ 9, ForceMode2D.Impulse);
        }

        //Animation, (Yes, I should probably make a state machine)
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            animator2D.SetAnimation(State.Run);
        }
        else
        {
            animator2D.SetAnimation(State.Idle);
        }
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
}
