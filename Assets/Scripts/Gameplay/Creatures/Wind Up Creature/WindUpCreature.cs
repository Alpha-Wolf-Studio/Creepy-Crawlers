using UnityEngine;

namespace Gameplay.Creatures
{
    public class WindUpCreature : MonoBehaviour
    {
        [SerializeField] private Bounds pushbound;
        [SerializeField] private float forceToAdd;
        [SerializeField] private LayerMask gnomesLayer;
        [SerializeField] private LayerMask blockLayer;

        void Update()
        {
            Collider2D[] collidersInTheSlowdownZone = Physics2D.OverlapBoxAll(transform.position + pushbound.center, pushbound.size, 0, gnomesLayer);

            foreach (var collider in collidersInTheSlowdownZone)
            {
                float yDistance = collider.transform.position.y - transform.position.y - pushbound.min.y - .1f;
                if (!Physics2D.Raycast(collider.transform.position, Vector2.down, yDistance, blockLayer)) 
                {
                    TryAddForce(collider, 1);
                }
            }
        }

        private void TryAddForce(Collider2D collider, float gravityScale)
        {
            if (collider.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.AddForce(transform.up * forceToAdd);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + pushbound.center, pushbound.size);
        }
    }
}