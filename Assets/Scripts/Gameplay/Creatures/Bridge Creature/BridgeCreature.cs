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

            if(leftHandPosition.x < rightHandPosition.x) 
            {
                LeftHandVisual.transform.position = leftHandPosition;
                rightHandVisual.transform.position = rightHandPosition;
            }
            else 
            {
                LeftHandVisual.transform.position = rightHandPosition;
                rightHandVisual.transform.position = leftHandPosition;
            }



            float size = Vector2.Distance(leftHandPosition, rightHandPosition);
            _collider2D.size = new Vector2(size, _collider2D.size.y);
            bridgeArmsMask.transform.localScale = new Vector3(size + maskRemovalOffset, bridgeArmsMask.transform.localScale.y, bridgeArmsMask.transform.localScale.z);

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