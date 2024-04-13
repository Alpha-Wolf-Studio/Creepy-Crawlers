using UnityEngine;

namespace Gameplay.Creatures
{
    public class BridgeCreatureSpawner : CreatureSpawner
    {
        [Header("Spawn Configuration")]
        [SerializeField] private BridgeCreature bridgeCreature;
        [SerializeField] private GameObject leftHandPreviewPrefab;
        [SerializeField] private GameObject rightHandPreviewPrefab;
        [SerializeField] private float bridgeMaxSize;

        [Header("Hook Configuration")]
        [SerializeField] private LayerMask solidBridgeCheckMask;
        [SerializeField] private float hookDistanceFromTop;
        [SerializeField] private float solidBridgeTolerance;

        private GameObject _leftHandPreview = null;
        private GameObject _rightHandPreview = null;

        private SpawnState _currentSpawnState;

        private Collider2D _possibleFirstCollider;

        private bool _solidBridge;

        private void Start()
        {
            if (!_leftHandPreview)
                _leftHandPreview = Instantiate(leftHandPreviewPrefab);

            _currentSpawnState = SpawnState.SpawningLeftHand;
        }

        private void Update()
        {
            if (_currentSpawnState == SpawnState.SpawningLeftHand)
            {
                _leftHandPreview.transform.position = GetPointerPositionInWorldPosition();

                if (IsSpawnButtonDown())
                {
                    _rightHandPreview = Instantiate(rightHandPreviewPrefab);
                    _currentSpawnState = SpawnState.SpawningRightHand;
                    CheckFirstHandHook();
                }
            }
            else if (_currentSpawnState == SpawnState.SpawningRightHand)
            {
                _rightHandPreview.transform.position = GetPointerPositionInWorldPosition();

                if (IsSpawnButtonDown())
                {
                    if (_possibleFirstCollider) 
                    {
                        CheckSecondHandHook();
                    }

                    BridgeCreature bridge = Instantiate(bridgeCreature);
                    bridge.SetCreature(_solidBridge, _leftHandPreview.transform.position, _rightHandPreview.transform.position);

                    CreatureSpawnedEvent?.Invoke(Data);
                    Destroy(gameObject);
                }
            }
        }

        private void CheckFirstHandHook() 
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(GetPointerPositionInWorldPosition(), solidBridgeTolerance, solidBridgeCheckMask);
            
            _solidBridge = hits.Length > 0;

            foreach (var hit in hits)
            {
                _possibleFirstCollider = hit;
                float hookYPosition = hit.bounds.max.y - hookDistanceFromTop;
                _leftHandPreview.transform.position = 
                    new Vector3(_leftHandPreview.transform.position.x, hookYPosition, _leftHandPreview.transform.position.z);
                return;
            }
        }

        private void CheckSecondHandHook() 
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(GetPointerPositionInWorldPosition(), solidBridgeTolerance, solidBridgeCheckMask);

            foreach (var hit in hits)
            {
                if (hit == _possibleFirstCollider) continue;

                _rightHandPreview.transform.position = new Vector3(_rightHandPreview.transform.position.x,
                    _leftHandPreview.transform.position.y, _rightHandPreview.transform.position.z);
                return;
            }
        }

        private void OnDestroy()
        {
            if (_leftHandPreview)
                Destroy(_leftHandPreview);
            _leftHandPreview = null;

            if (_rightHandPreview)
                Destroy(_rightHandPreview);
            _rightHandPreview = null;
        }

        private enum SpawnState
        {
            SpawningLeftHand,
            SpawningRightHand
        }

    }
}