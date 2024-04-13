using System;
using UnityEngine;

namespace Gnome
{
    public class Gnome : MonoBehaviour
    {
        private static readonly int IsAlive = Animator.StringToHash("IsAlive");

        [SerializeField] private GnomeStats gnomeStats = new GnomeStats();
        [SerializeField] private Direction direction = Direction.Left;
        [SerializeField] private LayerMask layerMaskCollision;
        [SerializeField] private Animator animator;

        public bool IsFalling { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public GnomeStats GnomeStats => gnomeStats;

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            CheckForwardCollision();
            CheckFallCollision();
        }

        private void FixedUpdate()
        {
            Move();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">: -1: Left | 0: None | 1: Right</param>
        public void Set(int direction)
        {
            this.direction = (Direction)direction;
        }

        public void AddExternalVelocity(Vector2 forceToAdd, ForceMode2D forceMode2D)
        {
            Rigidbody2D.AddForce(forceToAdd, forceMode2D);
        }

        public void Kill()
        {
            direction = Direction.None;
            animator.SetBool(IsAlive, false);
        }

        private void Move()
        {
            float currentVel = Mathf.Abs(Rigidbody2D.velocity.x);
            if (currentVel < gnomeStats.maxVelocity)
            {
                float forceToAdd = gnomeStats.forceMovement;
                if (currentVel + forceToAdd > gnomeStats.maxVelocity)
                {
                    forceToAdd = gnomeStats.maxVelocity - currentVel;
                }

                Rigidbody2D.AddForce(new Vector2(forceToAdd * (int)direction, 0), ForceMode2D.Impulse);
                Debug.Log("Push");
            }
        }

        private void CheckForwardCollision()
        {
            float raycastDistance = 0.1f;
            RaycastHit2D hit =
                    Physics2D.Raycast(transform.position, transform.right, raycastDistance, layerMaskCollision);

            if (hit.collider != null)
            {
                if (direction == Direction.Left) direction = Direction.Right;
                else if (direction == Direction.Right) direction = Direction.Left;
            }
        }

        private void CheckFallCollision()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Pickable pickable = other.GetComponent<Pickable>();
            if (pickable != null)
            {
                pickable.PickUp();
                switch (pickable.pickableType)
                {
                    case PickableType.Key:
                        GnomeStats.keyAmount++;
                        break;
                    case PickableType.Star:
                        GnomeStats.starAmount++;
                        break;
                    default:
                        Debug.LogWarning("Este pickable no posee tipo");
                        break;
                }
            }
        }
    }

    public enum Direction
    {
        Left = -1,
        None,
        Right
    }

    public enum State
    {
        Idle,
        Move,
        Fall,
        Fly,
        Death
    }
}