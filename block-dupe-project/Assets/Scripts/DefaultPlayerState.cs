using UnityEngine;
using Unity;
using Unity.VisualScripting;

public class DefaultPlayerState : IPlayerState
{
    int jumpHeight = 22;
    int runSpeed = 8;
    int maxSpeed = 22;
    int landingTimer; //negative = falling, positive = landing, 0 = not used.

    public enum SubStates { Normal, Carry, Duck, CloneStruggle, ThrowStruggle, Wall, LookUp, CarryDuck, CarryWall, CarryLookUp};
    public enum MovementState { Idle, Running, Jumping, Falling, Landing };
    public SubStates currentSubState;
    public MovementState movementState;
    public bool isThrowStruggleForStraightShot;


    public void OnEnter(PlayerStateManager manager){}

    public void OnExit(PlayerStateManager manager){}

    public void UpdateState(PlayerStateManager manager)
    {
        Vector2 joyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 velocity = manager.rigidBody.velocity;

        // Vertical Movement
        if (Input.GetKeyDown(KeyCode.Space) && manager.IsGrounded() && !(currentSubState == SubStates.Duck || currentSubState == SubStates.CarryDuck))
        {
            manager.rigidBody.AddForce(20f * jumpHeight * Vector2.up, ForceMode2D.Impulse);
        }

        // Cloning / Throwing
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!manager.carryingObj && !manager.HasSpaceToLift(manager.boxCollider))
            {
                currentSubState = SubStates.CloneStruggle;
            }
            else if (manager.carryingObj)
            {
                currentSubState = SubStates.ThrowStruggle;
            }
        }

        //direction
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            manager.transform.localScale = new Vector3(Mathf.Sign(Input.GetAxisRaw("Horizontal")), 1, 1);
            manager.direction = Input.GetAxisRaw("Horizontal") > 0;
        }

        //update carrying object if any
        if (manager.carryingObj)
        {
            manager.nearestLiftableObj = null;
            manager.carryingObj.transform.SetLocalPositionAndRotation(Vector2.zero, manager.transform.GetChild(0).rotation);
            manager.transform.GetChild(0).localPosition = manager.animator2D.animations[manager.animator2D.currentAnimation].HeldItemPosition;
        }

        //else look for a liftable object within a vicinity of the player.
        else if (!manager.nearestLiftableObj)
        {
            foreach (Collider2D x in Physics2D.OverlapBoxAll((Vector2)manager.transform.position + manager.boxCollider.offset, manager.boxCollider.bounds.size * 2f, 0))
            {
                if (x.GetComponent<Liftable>() != null)
                {
                    manager.nearestLiftableObj = x.GetComponent<Liftable>();
                    break;
                }
            }
        }

        //Get Movement State of Player
        if (manager.IsGrounded())
        {
            movementState = joyInput.x != 0 ? MovementState.Running : MovementState.Idle;
            //not working for some reason
            //Detect if landing timer is negative, then set landing timer to 1, causing it to increment for 30 frames while being in the landing state. 
            //Then, by setting to
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

        //dammit...
        bool isAgainstWall = IsAgainstWall(manager);

        switch (currentSubState)
        {
            case SubStates.Normal:
                if (joyInput.y < 0 && manager.IsGrounded())
                {
                    manager.duckBox?.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Duck;
                    break;
                }
                if (joyInput.x != 0 && isAgainstWall)
                {
                    currentSubState = SubStates.Wall;
                    break;
                }
                if (joyInput.y > 0 && movementState == MovementState.Idle)
                {
                    currentSubState = SubStates.LookUp;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.Idle + (int)movementState);
                break;

            case SubStates.Carry:
                if (joyInput.y < 0 && manager.IsGrounded())
                {
                    manager.carryDuckBox?.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.CarryDuck;
                    break;
                }
                if (joyInput.x != 0 && IsAgainstWall(manager))
                {
                    currentSubState = SubStates.CarryWall;
                    break;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryIdle + (int)movementState);
                break;
            case SubStates.CarryDuck:
                if (joyInput.y >= 0 && !manager.IsTouchingCeiling())
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
                if (joyInput.y >= 0 && !manager.IsTouchingCeiling())
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
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    currentSubState = SubStates.Carry;
                    manager.carryBox.SetCollisionBox(manager.boxCollider);
                    manager.Clone();
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CloneStruggleIdle + (int)movementState);
                break;
            case SubStates.ThrowStruggle:
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    manager.normalBox.SetCollisionBox(manager.boxCollider);
                    currentSubState = SubStates.Normal;
                    manager.ThrowHeldObject();
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.ThrowStruggleIdle + (int)movementState);
                break;
            case SubStates.Wall:
                if (!IsAgainstWall(manager))
                {
                    currentSubState = SubStates.Normal;
                }
                if (movementState == MovementState.Idle)
                {
                    currentSubState = SubStates.Normal;
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.WallRun + (manager.IsGrounded() ? 0 : 1));
                }
                break;
            case SubStates.LookUp:
                if (joyInput.y <= 0 || movementState != MovementState.Idle)
                {
                    currentSubState = SubStates.Normal;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.IdleLookUp);
                break;
            case SubStates.CarryWall:
                if (!IsAgainstWall(manager))
                {
                    currentSubState = SubStates.Carry;
                }
                if (movementState == MovementState.Idle)
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryIdle);
                }
                else
                {
                    manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryWallRun + (manager.IsGrounded() ? 0 : 1));
                }
                break;
            case SubStates.CarryLookUp:
                if (joyInput.y <= 0 || movementState != MovementState.Idle)
                {
                    currentSubState = SubStates.Carry;
                }
                manager.animator2D.SetAnimation(PlayerStateManager.Animations.CarryLookUpIdle);
                break;
        }
    }

    private static bool IsAgainstWall(PlayerStateManager manager)
    {
        return (manager.IsTouchingRightWall() && manager.direction) || (manager.IsTouchingLeftWall() && !manager.direction);
    }

    public void SetAnimation(PlayerStateManager manager, Vector2 joyInput)
    {
        switch (currentSubState)
        {
            case SubStates.Normal when joyInput.y < 0 && manager.IsGrounded():
            break;
            case SubStates.Normal when joyInput.x != 0 && IsAgainstWall(manager):
            break;
        }
    }
    public void ChangeStateLogic()
    {
        return;
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