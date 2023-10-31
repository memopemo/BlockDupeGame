using Unity.VisualScripting;
using UnityEngine;
public class PhysicsObject : MonoBehaviour
{
    void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            // add opposing force to our velocity until its zero.
            GetComponent<Rigidbody2D>().AddForce(10 * GetComponent<Rigidbody2D>().velocity.x * Vector2.left);
        }
    }
}