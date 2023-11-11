using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Conductive : MonoBehaviour
{
    public bool Electrified { get; private set; }
    public UnityEvent UpdateElectrified;
    public UnityEvent StartElectrified;
    public UnityEvent EndElectrified;
    public GameObject sparks;

    public void Start()
    {
        InvokeRepeating(nameof(CheckForElectricity), 0.25f, 0.25f);
    }

    // Updates every 1/4th of a second because its not frame-important
    void CheckForElectricity()
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        Collider2D[] AllTouchingObjects = Physics2D.OverlapBoxAll((Vector2)transform.position + myCollider.offset, Vector2.one * 1.3f, 0);

        List<Collider2D> TouchingElectricObjects = new();

        foreach (var collider in AllTouchingObjects)
        {
            if (collider == myCollider) continue;
            if (collider.TryGetComponent(out Conductive metal) && metal.Electrified ||
                collider.TryGetComponent(out Electric _))
            {
                TouchingElectricObjects.Add(collider);
            }
        }
        if (TouchingElectricObjects.Count == 0)
        {
            if (Electrified)
            {
                Electrified = false;
                EndElectrified.Invoke();
                Destroy(transform.Find(sparks.name).gameObject);
            }
        }
        else
        {
            if (!Electrified)
            {
                StartElectrified.Invoke();
                Instantiate(sparks, transform).name = sparks.name;
            }
            Electrified = true;
            UpdateElectrified.Invoke();
        }
    }
}