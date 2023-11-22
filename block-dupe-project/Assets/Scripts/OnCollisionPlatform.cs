using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlatform : MonoBehaviour
{
    public float speed;         // Speed of the platform
    public int startingPoint;   // Starting index (position of the platform)
    public Transform[] points;  // An array of transform points (position where the platform needs to move)

    private int i;              // Index of the array
    private bool movingUp = true; // Flag to indicate the movement direction

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;    // Setting the position of the platform to
                                                                // the position of one of the points using index "startingPoint"
    }

    // Update is called once per frame
    void Update()
    {
        // Check distance of platform and point
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            // Immediately change direction upon collision with another platform
            if (ShouldChangeDirection())
            {
                movingUp = !movingUp;
                i = movingUp ? Mathf.Min(i + 1, points.Length - 1) : Mathf.Max(i - 1, 0);
            }
            else
            {
                // Update the index based on the movement direction
                i = movingUp ? (i + 1) % points.Length : (i - 1 + points.Length) % points.Length;
            }
        }

        // Move platform to point position i
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    // Check if the platform should change direction based on collisions with other platforms
    private bool ShouldChangeDirection()
    {
        // Implement your collision detection logic here
        // For example, you can use Physics2D.Raycast or Physics2D.OverlapBox

        // Placeholder logic: Change direction immediately when colliding with any other platform
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != gameObject && collider.CompareTag("Platform"))
            {
                return true;
            }
        }

        return false;
    }

    // Used for vertically moving platforms
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
