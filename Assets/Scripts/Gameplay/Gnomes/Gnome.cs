using System;
using System.Collections;
using UnityEngine;

namespace Gnomes
{
    public class Gnome : MonoBehaviour
    {
        [SerializeField] private int MaxHeightFall = -9;
        private static readonly int States = Animator.StringToHash("States");

        [SerializeField] private GnomeStats gnomeStats = new GnomeStats();
        [SerializeField] private Animator animator;
        [SerializeField] private CollisionDetect gnomeForward;
        [SerializeField] private CollisionDetect gnomeFloor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Direction direction = Direction.Right;
        private bool isAlive = true;
        private float lastFallSpeed = 0;

        public bool IsFalling { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public GnomeStats GnomeStats => gnomeStats;

        public static Action OnKeyPickUp;

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            AddAllListeners();
        }

        private void Update()
        {
            if (isAlive)
                CheckFall();
        }

        private void CheckFall()
        {
            if (lastFallSpeed > MaxHeightFall && Rigidbody2D.velocity.y < MaxHeightFall)
            {
                spriteRenderer.color = Color.magenta;
            }

            lastFallSpeed = Rigidbody2D.velocity.y;
        }

        private void FixedUpdate()
        {
            if (isAlive)
                Move();
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
                        OnKeyPickUp?.Invoke();
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

        private void OnDestroy()
        {
            RemoveAllListeners();
        }

        private void AddAllListeners()
        {
            gnomeForward.onCollisionEnter += GnomeForward_onCollisionEnter;
            gnomeFloor.onCollisionEnter += GnomeFloor_onCollisionEnter;
            gnomeFloor.onEmptyCollisions += GnomeFloor_onEmptyCollisions;
        }

        private void RemoveAllListeners()
        {
            gnomeForward.onCollisionEnter -= GnomeForward_onCollisionEnter;
            gnomeFloor.onCollisionEnter -= GnomeFloor_onCollisionEnter;
            gnomeFloor.onEmptyCollisions -= GnomeFloor_onEmptyCollisions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">: -1: Left | 0: None | 1: Right</param>
        public void Set(int direction)
        {
            this.direction = (Direction)direction;
            if (this.direction == Direction.Left)
            {
                transform.Rotate(Vector3.up, 180f);
                Rigidbody2D.velocity = Vector2.zero;
            }
        }

        public void AddExternalVelocity(Vector2 forceToAdd, ForceMode2D forceMode2D)
        {
            Rigidbody2D.AddForce(forceToAdd, forceMode2D);
        }

        public void Kill()
        {
            spriteRenderer.color = Color.red;
            direction = Direction.None;
            animator.SetInteger(States, (int)State.Death);
            Rigidbody2D.velocity = Vector2.zero;
            isAlive = false;
            StartCoroutine(Dying());
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
            }
        }

        private void GnomeFloor_onCollisionEnter()
        {
            if (Rigidbody2D.velocity.y < MaxHeightFall)
            {
                Kill();
                return;
            }

            animator.SetInteger(States, (int)State.Move);
            spriteRenderer.color = Color.white;
        }

        private void GnomeFloor_onEmptyCollisions()
        {
            IsFalling = true;
            animator.SetInteger(States, (int)State.Fall);
            spriteRenderer.color = Color.cyan;
        }

        private void GnomeForward_onCollisionEnter()
        {
            transform.Rotate(Vector3.up, 180f);
            Rigidbody2D.velocity = Vector2.zero;

            if (direction == Direction.Left)
                direction = Direction.Right;
            else if (direction == Direction.Right)
                direction = Direction.Left;
        }

        private IEnumerator Dying()
        {
            yield return new WaitForSeconds(1);
            float onTime = 0;
            float maxTime = 1;
            Color from = spriteRenderer.color;
            Color to = Color.clear;

            while (onTime < maxTime)
            {
                onTime += Time.deltaTime;
                spriteRenderer.color = Color.Lerp(from, to, onTime);
                yield return null;
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