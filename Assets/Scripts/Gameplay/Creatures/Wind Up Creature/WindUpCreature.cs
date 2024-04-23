using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class WindUpCreature : MonoBehaviour
    {
        [SerializeField] private Bounds pushbound;
        [SerializeField] private float forceToAdd;
        [SerializeField] private LayerMask gnomesLayer;
        [SerializeField] private LayerMask blockLayer;

        List<Collider2D> _collidersToAffect = new List<Collider2D>();

        void Update()
        {
            Collider2D[] collidersInTheSlowdownZone = Physics2D.OverlapBoxAll(transform.position + pushbound.center, pushbound.size, 0, gnomesLayer);

            foreach (var collider in collidersInTheSlowdownZone)
            {
                float yDistance = collider.transform.position.y - transform.position.y - pushbound.min.y - .1f;
                if (!Physics2D.Raycast(collider.transform.position, Vector2.down, yDistance, blockLayer)) 
                {
                    if (!_collidersToAffect.Contains(collider))
                        _collidersToAffect.Add(collider);
                }
            }

            _collidersToAffect.RemoveAll(i => !collidersInTheSlowdownZone.Contains(i));
        }

        private void FixedUpdate()
        {
            foreach (var collider in _collidersToAffect)
            {
                if (collider && collider.TryGetComponent(out Rigidbody2D rigidbody2D))
                {
                    TryAddForce(rigidbody2D, transform.up * forceToAdd);
                }
            }
        }

        private void TryAddForce(Rigidbody2D rigidbody2D, Vector3 force)
        {
            rigidbody2D.AddForce(force);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + pushbound.center, pushbound.size);
        }
    }
}