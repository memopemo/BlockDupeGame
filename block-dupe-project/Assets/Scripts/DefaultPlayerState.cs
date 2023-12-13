using UnityEngine;

public class DefaultPlayerState : IPlayerState
{
    int jumpHeight = 22;
    int runSpeed = 8;
    int maxSpeed = 22;
    int coyoteTime;
    bool coyoteTimeEnable;
    int landingTimer; //negative = falling, positive = landing, 0 = not used.
    int runTime;
    int stepRate = 13;

    public IPlayerSubstate currentSubState;
    public NormalPlayerSubstate normalPlayerSubstate;
    public DuckPlayerSubstate duckPlayerSubstate;
    public WallPlayerSubstate wallPlayerSubstate;
    public LookUpPlayerSubstate lookUpPlayerSubstate;
    public CloneStrugglePlayerSubstate cloneStruggleSubstate;

    public enum MovementState { Idle, Running, Jumping, Falling, Landing };
    public MovementState movementState;

    public bool isThrowStruggleForStraightShot;

    public void ChangeSubstate(IPlayerSubstate newSubstate, PlayerStateManager manager)
    {
        currentSubState = newSubstate;
        currentSubState.EnterSubstate(manager, this);
    }
    public void OnEnter(PlayerStateManager manager)
    {
        normalPlayerSubstate = new NormalPlayerSubstate();
        duckPlayerSubstate = new DuckPlayerSubstate();
        wallPlayerSubstate = new WallPlayerSubstate();
        lookUpPlayerSubstate = new LookUpPlayerSubstate();
        cloneStruggleSubstate = new CloneStrugglePlayerSubstate();
        currentSubState = normalPlayerSubstate;
    }

    public void OnExit(PlayerStateManager manager)
    {
        manager.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        //vars
        Vector2 joyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 velocity = manager.rigidBody.velocity;
        bool IsGrounded = manager.IsGrounded();
        MovementState previousMovementState = movementState;

        //grounded y position does not change.
        /* manager.rigidBody.constraints = IsGrounded
            ? RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY
            : RigidbodyConstraints2D.FreezeRotation; */

        //update coyoteTime
        if (coyoteTimeEnable)
        {
            coyoteTime += 1;
        }
        else
        {
            coyoteTime = 0;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && (IsGrounded || (coyoteTime > 0 && coyoteTime <= 15)))
        {
            if(!PowerupStatus.JumpHold && manager.carryingObj) return;
            //Debug.Log("Jump!");
            manager.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            manager.rigidBody.AddForce(20f * jumpHeight * Vector2.up, ForceMode2D.Impulse);
            manager.playerSounds.PlayJump();
        }

        // Cloning / Throwing
        if (Input.GetButtonDown("Fire1") && ((!manager.carryingObj && !manager.HasSpaceToLift(manager.boxCollider)) || manager.carryingObj))
        {
            if(!PowerupStatus.Clone) return;
            if(!PowerupStatus.Midair && !manager.IsGrounded() && !manager.carryingObj) return; //prevent midair duplication
            ChangeSubstate(cloneStruggleSubstate, manager);  
        }

        //direction
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            manager.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
            manager.direction = Input.GetAxisRaw("Horizontal") > 0;
        }

        

        manager.nearestLiftableObj = null;
        foreach (Collider2D x in Physics2D.OverlapBoxAll((Vector2)manager.transform.position + manager.boxCollider.offset, manager.boxCollider.bounds.size * 2f, 0))
        {
            if (x.GetComponent<Liftable>() != null && (!x.TryGetComponent(out PlayerStateManager a) || a != manager)) //is liftable but not us.=
            {
                manager.nearestLiftableObj = x.GetComponent<Liftable>();
                break;
            }
        }

        //update carrying object if any (also nulls nearest liftable object)
        if (manager.carryingObj)
        {
            manager.nearestLiftableObj = null;
            manager.carryingObj.transform.SetLocalPositionAndRotation(Vector2.zero, manager.transform.GetChild(0).rotation);
            manager.transform.GetChild(0).localPosition = manager.animator2D.animations[manager.animator2D.currentAnimation].HeldItemPosition;
        }

        //Get Movement State of Player
        if (IsGrounded)
        {
            movementState = joyInput.x != 0 ? MovementState.Running : MovementState.Idle;
            
            //Detect if landing timer is negative, then set landing timer to 1, causing it to increment for 10 frames while being in the landing state. 
            if (landingTimer != 0)
            {
                movementState = MovementState.Landing;
            }
            if (landingTimer < 0)
            {
                landingTimer = 1;
            }
            else if (landingTimer > 0)
            {
                landingTimer++;
            }
            if (movementState == MovementState.Landing && landingTimer == 10)
            {
                landingTimer = 0; //setting to 0 allows other movement states to pass through.
            }
        }
        else // airborn
        {
            if (velocity.y > 0)
            {
                movementState = MovementState.Jumping;
            }
            else
            {
                movementState = MovementState.Falling;
                landingTimer--;
            }
        }
        //Debug.Log(landingTimer);
        //Debug.Log(IsGrounded);

        //if we switch from running to falling or coyoteTime is already enabled...
        if ((previousMovementState == MovementState.Running && movementState == MovementState.Falling) || coyoteTimeEnable)
        {
            coyoteTimeEnable = true;
        }
        if ((coyoteTimeEnable && IsGrounded) || (coyoteTimeEnable && movementState == MovementState.Jumping))
        {
            coyoteTimeEnable = false;
        }

        //:D
        currentSubState.UpdateSubstate(manager, this);
        manager.UpdateHealth();

        if(landingTimer == 1)
        {
            manager.playerSounds.PlayLand();
        }
    }

    // This deals with the physical movement of the player.
    public void FixedUpdateState(PlayerStateManager manager)
    {
        Vector2 joyInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 velocity = manager.rigidBody.velocity;

        // Horizontal Movement
        if (joyInput.x != 0)
        {
            if (Mathf.Abs(velocity.x) < runSpeed)
            {
                manager.rigidBody.AddForce(10 * joyInput.x * runSpeed * Vector2.right);
            }
            runTime++;
        }
        else if (velocity.x != 0)
        {
            // add opposing force to our velocity until its zero.
            manager.rigidBody.AddForce(10 * velocity.x * Vector2.left);
            runTime = 0;
        }

        // Horizontal Speed Capping
        if (Mathf.Abs(velocity.x) > maxSpeed || Mathf.Abs(velocity.y) > maxSpeed)
        {
            manager.rigidBody.velocity = new Vector2(
                Mathf.Clamp(manager.rigidBody.velocity.x, -maxSpeed, maxSpeed),
                Mathf.Clamp(manager.rigidBody.velocity.y, -maxSpeed, maxSpeed)
                );
        }

        
        if(runTime % stepRate == stepRate-1 && manager.IsGrounded() && (currentSubState == normalPlayerSubstate || currentSubState == wallPlayerSubstate))
        {
            manager.playerSounds.PlayStep();
        }

        /* if (manager.IsGrounded() && manager.SnapToGround(out float result))
        {
            manager.rigidBody.position = new Vector2(manager.rigidBody.position.x, result);
        } */
    }
    public void SetAnimation(int animation, int animationCarry, PlayerStateManager manager)
    {
        //set animation depending on if carrying object or not
        if(!manager.carryingObj)
        {
            manager.animator2D.SetAnimation(animation + (int)movementState);
        }
        else
        {
            manager.animator2D.SetAnimation(animationCarry + (int)movementState);
        }
    }
}
public interface IPlayerSubstate
{
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager);
    public void EnterSubstate(PlayerStateManager manager, DefaultPlayerState substateManager);
}