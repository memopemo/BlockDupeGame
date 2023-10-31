using UnityEngine;
public class Liftable : MonoBehaviour 
{
    bool lifted;
    Vector2Int StraightVector;

    public void Start()
    {
    }
    public virtual void Update()
    {
        if(lifted) UpdateLifted();
        if(StraightVector != Vector2Int.zero) UpdateStraight();
    }
    public virtual void UpdateStraight()
    {
        transform.position += (Vector3)(Vector2)StraightVector;
    }

    public virtual void OnBeingLifted()
    {
        lifted = true;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = true;
        }
    }
    public virtual void OnBeingThrown()
    {
        lifted = false;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = false;
        }
    }
    public virtual void OnBeingStraightThrown(Vector2Int StraightVector)
    {
        lifted = false;
    }
    public virtual void UpdateLifted()
    {
        // Add some code here.
    }

}
