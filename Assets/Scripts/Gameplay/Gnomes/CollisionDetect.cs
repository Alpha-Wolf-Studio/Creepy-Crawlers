using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    public event Action onCollisionEnter;
    public event Action onEmptyCollisions;
    private List<Collider2D> colliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            colliders.Add(other);
            onCollisionEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            colliders.Remove(other);
        }

        if (colliders.Count == 0)
        {
            onEmptyCollisions?.Invoke();
        }
    }
}