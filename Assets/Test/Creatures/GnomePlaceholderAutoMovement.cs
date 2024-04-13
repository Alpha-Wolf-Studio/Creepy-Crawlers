using UnityEngine;

namespace Test.Creatures 
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class GnomePlaceholderAutoMovement : MonoBehaviour
    {
        [SerializeField] private Vector2 forceToAdd;
        [SerializeField] private float maxVelocity;

        private Rigidbody2D body2D;

        private void Awake()
        {
            body2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if(body2D.velocity.magnitude < maxVelocity) 
            {
                body2D.AddForce(forceToAdd, ForceMode2D.Impulse);
            }
        }
    }
}
