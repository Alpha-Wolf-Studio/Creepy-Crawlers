using UnityEngine;

namespace Gameplay.Creatures
{
    public class SpringboardCreature : MonoBehaviour
    {
        [SerializeField] private float reboundStrenght;

        private Vector3 _reboundDirection;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.AddForce(_reboundDirection * reboundStrenght, ForceMode2D.Impulse);
            }
        }

        public void SetReboundDirection(Vector3 direction) => _reboundDirection = direction;
    }
}
