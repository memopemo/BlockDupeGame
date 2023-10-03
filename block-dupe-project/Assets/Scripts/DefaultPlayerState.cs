using UnityEngine;
using Unity;

public class DefaultPlayerState : IPlayerState
{
    int jumpHeight = 22;
    int runSpeed = 8;
    int maxSpeed = 22;
    float landingTimer;

    public enum SubStates { Normal, Carry, Duck, CloneStruggle, ThrowStruggle, Wall, LookUp, CarryDuck, CarryWall, CarryLookUp};
    public enum MovementState { Idle, Running, Jumping, Falling, Landing };
    public SubStates currentSubState;
    public MovementState movementState;
    public bool isThrowStruggleForStraightShot;


    public void OnEnter(PlayerStateManager manager)
    {

    }

    public void OnExit(PlayerStateManager manager)
    {
    }

    public void UpdateState(PlayerStateManager manager)
    {
        Vector2 joyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 velocity = manager.rigidBody.velocity;

        // Vertical Movement
        if (Input.GetKeyDown(KeyCode.Space) && manager.IsGrounded())
        {
            manager.rigidBody.AddForce(20f * jumpHeight * Vector2.up, ForceMode2D.Impulse);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(!manager.carryingObj)
            {
                currentSubState = SubStates.CloneStruggle;
            }
            else
            {
                currentSubState = SubStates.ThrowStruggle;
            }
        }



        //Get Movement State of Player

        if (manager.IsGrounded())
        {
            movementState = joyInput.x != 0 ? MovementState.Running : MovementState.Idle;

            if (landingTimer < 0)
            {
                movementState = MovementState.Landing;
                landingTimer = 1;
            }
            if (movementState == MovementState.Landing)
            {
                if (landingTimer > 30)
                {
                    landingTimer = 0; //setting to 0 allows other movement states to pass through.
                }
                else
                {
                    landingTimer += Time.deltaTime;
                }
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
                landingTimer -= Time.deltaTime;
            }
        }

        Debug.Log(movementState);
        Debug.Log(currentSubState);

        switch (currentSubState)
        {
            case SubStates.Normal:
                if (joyInput.y < 0)
                {
                    
                    manager.duckBox?.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Duck;
                    break;
                }
                if (joyInput.x != 0 && manager.IsTouchingRightWall() && manager.direction || manager.IsTouchingLeftWall() && !manager.direction)
                {
                    currentSubState = SubStates.Wall;
                    break;
                }
                if(joyInput.y > 0 && movementState == MovementState.Idle)
                {
                    currentSubState = SubStates.LookUp;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.Idle + (int)movementState);
                break;
            case SubStates.Carry:
                if (joyInput.y < 0)
                {
                    manager.carryDuckBox?.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.CarryDuck;
                    break;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryIdle + (int)movementState);
                break;
            case SubStates.CarryDuck:
                if (joyInput.y >= 0)
                {
                    manager.carryBox.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Carry;
                    break;
                }
                if (movementState == MovementState.Running)
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryDuckRun);
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryDuckIdle);
                }
                break;
            case SubStates.Duck:
                if (joyInput.y >= 0)
                {
                    manager.normalBox.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Normal;
                    break;
                }
                if (movementState == MovementState.Running)
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.DuckRun);
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.DuckIdle);
                }
                break;
            case SubStates.CloneStruggle:
                if(Input.GetKeyUp(KeyCode.Z))
                {
                    currentSubState = SubStates.Carry;
                    manager.carryBox.SetCollisionBox(manager.boxCollider);
                    manager.carryingObj = true;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CloneStruggleIdle + (int)movementState);
                break;
            case SubStates.ThrowStruggle:
                if(Input.GetKeyUp(KeyCode.Z))
                {
                    manager.normalBox.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Normal;
                    manager.carryingObj = false;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.ThrowStruggleIdle + (int)movementState);
                break;
            case SubStates.Wall:
                if ((!manager.IsTouchingRightWall() || !manager.direction) && (!manager.IsTouchingLeftWall() || manager.direction))
                {
                    currentSubState = SubStates.Normal;
                }
                if (movementState == MovementState.Idle) 
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.Idle);
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.WallRun + (manager.IsGrounded()? 0 : 1));
                }
                break;
            case SubStates.LookUp:
                if(joyInput.y <= 0 || movementState != MovementState.Idle)
                {
                    currentSubState = SubStates.Normal;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.IdleLookUp);
                break;
            case SubStates.CarryWall:
                if ((!manager.IsTouchingRightWall() || !manager.direction) && (!manager.IsTouchingLeftWall() || manager.direction))
                {
                    currentSubState = SubStates.Carry;
                }
                if (movementState == MovementState.Idle) 
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryIdle);
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryWallRun + (manager.IsGrounded()? 0 : 1));
                }
                break;
            case SubStates.CarryLookUp:
                if(joyInput.y <= 0 || movementState != MovementState.Idle)
                {
                    currentSubState = SubStates.Carry;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryLookUpIdle);
                break;
        }
    }

    // This deals with the physical movement of the player.
    public void FixedUpdateState(PlayerStateManager manager)
    {
        Vector2 joyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 velocity = manager.rigidBody.velocity;
        // Horizontal Movement
        if (joyInput.x != 0)
        {
            if (Mathf.Abs(velocity.x) < runSpeed)
            {
                manager.rigidBody.AddForce(10 * joyInput.x * runSpeed * Vector2.right);
            }
        }
        else if (velocity.x != 0)
        {
            // add opposing force to our velocity until its zero.
            manager.rigidBody.AddForce(10 * velocity.x * Vector2.left);
        }

        // Horizontal Speed Capping
        if (Mathf.Abs(velocity.x) > maxSpeed || Mathf.Abs(velocity.y) > maxSpeed)
        {
            manager.rigidBody.velocity  = new Vector2(
                Mathf.Clamp(manager.rigidBody.velocity.x, -maxSpeed, maxSpeed),
                Mathf.Clamp(manager.rigidBody.velocity.y, -maxSpeed, maxSpeed)
                );
        }
    }
}