using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Liftable : MonoBehaviour 
{
    bool lifted;
    public virtual void Update()
    {
        if(lifted) UpdateLifted();
    }

    public virtual void OnBeingLifted()
    {
        lifted = true;
        GetComponent<Collider2D>().isTrigger = true;
    }
    public virtual void OnBeingThrown()
    {
        lifted = false;
        GetComponent<Collider2D>().isTrigger = false;
    }
    public virtual void UpdateLifted()
    {
        // Add some code here.
    }

}
