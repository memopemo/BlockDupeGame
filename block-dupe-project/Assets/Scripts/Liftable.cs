using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class Liftable : MonoBehaviour 
{
    bool lifted;
    Vector2 StraightVector;
    Rigidbody2D  rb;
    float originalGravity;
    GameObject throwerObject; //this is so we dont collide with our thrower
    Collider2D col;
    TrailRenderer straightShotTrail;
    AudioSource audioSource;
    public AudioClip Thrown;
    public AudioClip StraightThrown;
    public AudioClip Smash;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        col = GetComponent<Collider2D>();
        straightShotTrail = GetComponent<TrailRenderer>();
        straightShotTrail.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }
    public virtual void Update()
    {
        if(lifted) UpdateLifted();
        if(IsStraightThrown()) UpdateStraight();
    }
    public virtual void UpdateStraight()
    {
        float distancePerStep = 30 * Time.deltaTime;
        Vector2 nextPosition = distancePerStep * StraightVector;

        ContactFilter2D contactFilter2D = new()
        {
            useTriggers = false,
            useLayerMask = true,
            layerMask = LayerMask.NameToLayer("Ground")
            
        };
        
        Collider2D[] AllColliders = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0);

        //check if hitting something
        foreach (var collision in AllColliders)
        {
            
            //this is for when we get to 
            if(collision.transform.TryGetComponent(out MetalBreakable breakable) && TryGetComponent(out Conductive _))
            {
                Destroy(breakable.gameObject);
                audioSource.PlayOneShot(Smash);
                continue;
            }
            
            if(collision.gameObject == col.gameObject){continue;}
            if(collision.isTrigger){continue;}
            if(collision.gameObject != throwerObject)//any other type of solid thats not the player
            {
                print(collision);
                //stop moving
                StraightVector = Vector2.zero;
                rb.gravityScale = originalGravity;
                straightShotTrail.enabled = false;
                if(audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(Smash);
                }
                return;
            }
        }
        transform.position += (Vector3)nextPosition;
        
        
    }

    public virtual void OnBeingLifted()
    {
        lifted = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = true;
        }
    }
    public virtual void OnBeingThrown(GameObject thrower)
    {
        lifted = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        foreach(var c in GetComponents<Collider2D>())
        {
            c.isTrigger = false;
        }
        throwerObject = thrower;
        audioSource.PlayOneShot(Thrown);
    }
    // Disables Gravity.
    public virtual void StraightThrow(Vector2 StraightThrowVector)
    {
        StraightVector = StraightThrowVector.normalized;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.position += Vector2.up;
        straightShotTrail.Clear();
        straightShotTrail.enabled = true;
        audioSource.PlayOneShot(StraightThrown);
    }
    public virtual void UpdateLifted()
    {
        // Add some code here.
        // maybe like if its an enemy, a squirming animation
    }
    public bool IsStraightThrown() => StraightVector != Vector2Int.zero;


}
