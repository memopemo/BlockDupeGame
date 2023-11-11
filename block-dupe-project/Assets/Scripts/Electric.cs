
//Tag for an object that generates electricity.
using UnityEngine;
public class Electric : MonoBehaviour
{
    [SerializeField] GameObject sparks;
    void Start()
    {
        Instantiate(sparks, transform);
    }
}