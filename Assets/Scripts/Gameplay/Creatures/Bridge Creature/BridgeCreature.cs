using UnityEngine;

namespace Gameplay.Creatures
{
    public class BridgeCreature : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private GameObject LeftHandVisual;
        [SerializeField] private GameObject centerVisual;
        [SerializeField] private GameObject rightHandVisual;
        [SerializeField] private GameObject bridgeArmsMask;
        [SerializeField] private float maskRemovalOffset;

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

            float size = Vector2.Distance(leftHandPosition, rightHandPosition);
            _collider2D.size = new Vector2(size, _collider2D.size.y);
            bridgeArmsMask.transform.localScale = new Vector3(size + maskRemovalOffset, bridgeArmsMask.transform.localScale.y, bridgeArmsMask.transform.localScale.z);

            LeftHandVisual.transform.localPosition = 
                new Vector3(-size / 2, bridgeArmsMask.transform.localPosition.y, LeftHandVisual.transform.position.z);
            rightHandVisual.transform.localPosition =
                new Vector3(size / 2, bridgeArmsMask.transform.localPosition.y, rightHandVisual.transform.position.z);

            if (solidBridge) 
            {
                _body2D.bodyType = RigidbodyType2D.Kinematic;
            }
            else 
            {
                _body2D.bodyType = RigidbodyType2D.Dynamic;
                _body2D.mass = 1000;
            }
        }
    }
}