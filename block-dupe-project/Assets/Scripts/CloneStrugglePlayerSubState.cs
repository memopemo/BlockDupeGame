using UnityEngine;
public class CloneStrugglePlayerSubstate : IPlayerSubstate
{
    bool metalCloneActivated;
    bool straightShotActivated;
    float secsHoldingCloneButton; //since we are in this state by holding the clone button, we can use a float that counts up.
    void IPlayerSubstate.EnterSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        metalCloneActivated = false;
        secsHoldingCloneButton = 0;
    }
    
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        secsHoldingCloneButton += Time.deltaTime;
        
        if (Input.GetButtonUp("Fire1"))
        {
            if(manager.carryingObj)
            {
                substateManager.ChangeSubstate(substateManager.normalPlayerSubstate,manager);
                manager.normalBox.SetCollisionBox(manager.boxCollider);
                manager.ThrowHeldObject(straightShotActivated);
                secsHoldingCloneButton = 0;
            }
            else
            {
                if(manager.nearestLiftableObj)
                {
                    manager.Lift();
                }
                else
                {
                    Debug.Log(secsHoldingCloneButton);
                    manager.Clone(metalCloneActivated);
                    secsHoldingCloneButton = 0;
                }
                manager.carryBox.SetCollisionBox(manager.boxCollider);
                substateManager.ChangeSubstate(substateManager.normalPlayerSubstate, manager);
            }
        }

        if(!Input.GetKey(KeyCode.Z))
        {
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate,manager);
        }

        metalCloneActivated = PowerupStatus.Metal && secsHoldingCloneButton > manager.timeForActivatingPowerup;

        straightShotActivated = PowerupStatus.Straight && secsHoldingCloneButton > manager.timeForActivatingPowerup;
        

        
        substateManager.SetAnimation(PlayerStateManager.Animations.CloneStruggleIdle, PlayerStateManager.Animations.ThrowStruggleIdle, manager);
    }

    
}