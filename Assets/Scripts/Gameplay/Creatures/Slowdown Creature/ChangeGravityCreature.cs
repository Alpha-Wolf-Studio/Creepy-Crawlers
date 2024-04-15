using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gnomes;

namespace Gameplay.Creatures
{
    public class ChangeGravityCreature : MonoBehaviour
    {
        [SerializeField] private Bounds changeGravityBound;
        [SerializeField, Range(-10, 10)] private float newGravityFactor;
        [SerializeField, Range(0, 10)] private float gnomeMaxSpeedChange;
        [SerializeField, Range(0, 1)] private float gnomeSlowdownOnEnter;
        [SerializeField] private LayerMask gnomesLayer;

        private readonly List<Collider2D> _affectedColliders = new List<Collider2D>();
        private float? gnomeBaseSpeed = null;

        void Update()
        {
            List<Collider2D> collidersInTheGravityZone = Physics2D.OverlapBoxAll(transform.position + changeGravityBound.center, changeGravityBound.size, 0, gnomesLayer).ToList();

            foreach (var collider in collidersInTheGravityZone)
            {
                if (!_affectedColliders.Contains(collider)) 
                {
                    ChangeGravitiyScale(collider, newGravityFactor);
                    ReduceSpeed(collider, gnomeMaxSpeedChange);
                    _affectedColliders.Add(collider);
                }
            }

            foreach (var collider in _affectedColliders)
            {
                if (!collidersInTheGravityZone.Contains(collider)) 
                {
                    ChangeGravitiyScale(collider, 1);
                    RestartSpeed(collider, gnomeBaseSpeed.Value);
                }
            }

            _affectedColliders.RemoveAll(collider => !collidersInTheGravityZone.Contains(collider));
        }

        private void ChangeGravitiyScale(Collider2D collider, float gravityScale) 
        {
            if (collider.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.gravityScale = gravityScale;
            }
        }

        private void ReduceSpeed(Collider2D collider, float newSpeed) 
        {
            if (collider.TryGetComponent(out Gnome gnome))
            {
                if (!gnomeBaseSpeed.HasValue) 
                {
                    gnomeBaseSpeed = gnome.GnomeStats.maxVelocity;
                }

                gnome.GnomeStats.maxVelocity = newSpeed;

                Vector2 gnomePosition = gnome.transform.position;
                Vector2 ownPosition = new Vector3(transform.position.x, gnome.transform.position.y, transform.position.z);
                Vector2 pushDirection = (gnomePosition - ownPosition).normalized * gnomeSlowdownOnEnter;

                gnome.AddExternalVelocity(pushDirection, ForceMode2D.Impulse);
            }
        }

        private void RestartSpeed(Collider2D collider, float startSpeed) 
        {
            if (collider.TryGetComponent(out Gnome gnome))
            {
                gnome.GnomeStats.maxVelocity = startSpeed;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + changeGravityBound.center, changeGravityBound.size);
        }
    }
}