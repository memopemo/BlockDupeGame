using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;         // Speed of the platform
    public int startingPoint;   // Starting index (position of the platform)
    public Transform[] points;  // An array of transform points (position where the platform needs to move)

    private int i;              // Index of the array


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
                i++;

                // Check if platform was on the last point after index increase
                if (i == points.Length)
                {
                    i = 0;
                }
            }

            // Move platform to point position i
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        
    }

    /*
        //Used for horizontally moving platforms
        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            collision.transform.SetParent(null);
        }
    */
}
