using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Liftable : MonoBehaviour 
{
    bool lifted;
    Vector2Int StraightVector;
    Collider2D collider2D;
    public void Start()
    {
        collider2D = GetComponent<Collider2D>();
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
        collider2D.isTrigger = true;
    }
    public virtual void OnBeingThrown()
    {
        lifted = false;
        collider2D.isTrigger = false;
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
