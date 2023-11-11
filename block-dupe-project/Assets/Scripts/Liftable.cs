using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Liftable : MonoBehaviour 
{
    bool lifted;
    Vector2 StraightVector;
    Rigidbody2D  rb;
    float originalGravity;
    GameObject throwerObject; //this is so we dont collide with our thrower
    Collider2D col;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        col = GetComponent<Collider2D>();
    }
    public virtual void Update()
    {
        if(lifted) UpdateLifted();
        if(IsStraightThrown()) UpdateStraight();
    }
    public virtual void UpdateStraight()
    {
        float distancePerStep = 20 * Time.deltaTime;
        Vector2 nextPosition = distancePerStep * StraightVector;

        ContactFilter2D contactFilter2D = new()
        {
            useTriggers = false,
            useLayerMask = true,
            layerMask = LayerMask.NameToLayer("Ground")
            
        };
        
        Collider2D[] AllColliders = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0);

        
        foreach (var collision in AllColliders)
        {
            
            //this is for when we get to 
            /* if(collision.transform.TryGetComponent(out new Breakable breakable))
            {
                breakable.OnBreak();
            }*/
            
            if(collision.gameObject == col.gameObject){continue;}
            if(collision.isTrigger){continue;}
            if(collision.gameObject != throwerObject)//any other type of solid thats not the player
            {
                print(collision);
                //stop moving
                StraightVector = Vector2.zero;
                rb.gravityScale = originalGravity;
                return;
            }
        }
        transform.position += (Vector3)nextPosition;
        //check if hitting something
    }

    public virtual void OnBeingLifted()
    {
        lifted = true;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = true;
        }
    }
    public virtual void OnBeingThrown(GameObject thrower)
    {
        lifted = false;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = false;
        }
        throwerObject = thrower;
    }
    // Disables Gravity.
    public virtual void StraightThrow(Vector2 StraightThrowVector)
    {
        StraightVector = StraightThrowVector.normalized;
        rb.gravityScale = 0;
        rb.position += Vector2.up;
    }
    public virtual void UpdateLifted()
    {
        // Add some code here.
        // maybe like if its an enemy, a squirming animation
    }
    public bool IsStraightThrown() => StraightVector != Vector2Int.zero;


}
