using UnityEngine;
public class CloneStrugglePlayerSubstate : IPlayerSubstate
{
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if(manager.carryingObj)
            {
                substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
                manager.normalBox.SetCollisionBox(manager.boxCollider);
                manager.ThrowHeldObject();
            }
            else
            {
                if(manager.nearestLiftableObj)
                {
                    manager.Lift();
                }
                else
                {
                    manager.Clone();
                }
                substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
                manager.carryBox.SetCollisionBox(manager.boxCollider);
            }
        }
        if(!Input.GetKey(KeyCode.Z))
        {
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
        }
        
        substateManager.SetAnimation(PlayerStateManager.Animations.CloneStruggleIdle, PlayerStateManager.Animations.ThrowStruggleIdle, manager);
    }
}