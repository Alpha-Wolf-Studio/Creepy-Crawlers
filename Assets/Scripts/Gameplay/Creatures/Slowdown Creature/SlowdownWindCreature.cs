using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class SlowdownWindCreature : MonoBehaviour
    {
        [SerializeField] private Bounds slowDownbound;
        [SerializeField, Range(0, 1)] private float slowDownFactor;

        private readonly List<Collider2D> _affectedColliders = new List<Collider2D>();

        void Update()
        {
            List<Collider2D> collidersInTheSlowdownZone = Physics2D.OverlapBoxAll(transform.position + slowDownbound.center, slowDownbound.size, 0).ToList();

            foreach (var collider in collidersInTheSlowdownZone)
            {
                if (!_affectedColliders.Contains(collider)) 
                {
                    ChangeGravitiyScale(collider, slowDownFactor);
                    _affectedColliders.Add(collider);
                }
            }

            foreach (var collider in _affectedColliders)
            {
                if (!collidersInTheSlowdownZone.Contains(collider)) 
                {
                    ChangeGravitiyScale(collider, 1);
                }
            }

            _affectedColliders.RemoveAll(collider => !collidersInTheSlowdownZone.Contains(collider));
        }

        private void OnDestroy()
        {
            foreach (var collider in _affectedColliders)
            {
                ChangeGravitiyScale(collider, 1);
            }
        }

        private void ChangeGravitiyScale(Collider2D collider, float gravityScale) 
        {
            if (collider.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.gravityScale = gravityScale;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + slowDownbound.center, slowDownbound.size);
        }
    }
}