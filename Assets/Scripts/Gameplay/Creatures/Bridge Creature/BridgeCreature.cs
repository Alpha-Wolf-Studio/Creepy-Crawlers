using UnityEngine;

namespace Gameplay.Creatures
{
    public class BridgeCreature : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private GameObject LeftHandVisual;
        [SerializeField] private GameObject leftArmVisual;
        [SerializeField] private GameObject centerVisual;
        [SerializeField] private GameObject rightArmVisual;
        [SerializeField] private GameObject rightHandVisual;

        private Rigidbody2D _body2D;
        private BoxCollider2D _collider2D;

        private void Awake()
        {
            _body2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<BoxCollider2D>();
        }

        public void SetCreature(bool solidBridge, Vector3 leftHandPosition, Vector3 rightHandPosition) 
        {
            Vector3 centerPosition = Vector3.Lerp(leftHandPosition, rightHandPosition, .5f);
            transform.position = centerPosition;

            LeftHandVisual.transform.position = leftHandPosition;
            rightHandVisual.transform.position = rightHandPosition;

            _collider2D.size = new Vector2(Vector2.Distance(leftHandPosition, rightHandPosition), _collider2D.size.y);

            if (solidBridge) 
            {
                _body2D.bodyType = RigidbodyType2D.Kinematic;
            }
            else 
            {
                _body2D.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}