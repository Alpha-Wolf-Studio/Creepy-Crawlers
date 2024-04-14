using System.Linq;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class BridgeCreatureSpawner : CreatureSpawner
    {
        [Header("Spawn Configuration")]
        [SerializeField] private BridgeCreature bridgeCreature;
        [SerializeField] private GameObject leftHandPreviewPrefab;
        [SerializeField] private GameObject rightHandPreviewPrefab;
        [SerializeField] private float bridgeMinSize;
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
                Vector3 newPossiblePosition = GetPointerPositionInWorldPosition();
                newPossiblePosition.y = _leftHandPreview.transform.position.y;

                float distance = Vector3.Distance(_leftHandPreview.transform.position, newPossiblePosition);
                if (distance > bridgeMaxSize)
                {
                    Vector3 directionFromFirstPiece = (newPossiblePosition - _leftHandPreview.transform.position).normalized * bridgeMaxSize;
                    newPossiblePosition = directionFromFirstPiece + _leftHandPreview.transform.position;
                }
                else if (distance < bridgeMinSize) 
                {
                    Vector3 directionFromFirstPiece = (newPossiblePosition - _leftHandPreview.transform.position).normalized * bridgeMinSize;
                    newPossiblePosition = directionFromFirstPiece + _leftHandPreview.transform.position;
                }

                _rightHandPreview.transform.position = newPossiblePosition;

                if (_rightHandPreview.transform.position.x < _leftHandPreview.transform.position.x)
                {
                    _leftHandPreview.transform.localScale = Vector3.one;
                    _rightHandPreview.transform.localScale = Vector3.one;
                }
                else 
                {
                    _leftHandPreview.transform.localScale = new Vector3(-1, 1, 1);
                    _rightHandPreview.transform.localScale = new Vector3(-1, 1, 1);
                }


                if (IsSpawnButtonDown())
                {
                    if (_possibleFirstCollider) 
                    {
                        if (!CheckSecondHandHook())
                            return;
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

        private bool CheckSecondHandHook() 
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(GetPointerPositionInWorldPosition(), solidBridgeTolerance, solidBridgeCheckMask);

            foreach (var hit in hits)
            {
                if (hit == _possibleFirstCollider) continue;

                _rightHandPreview.transform.position = new Vector3(_rightHandPreview.transform.position.x,
                    _leftHandPreview.transform.position.y, _rightHandPreview.transform.position.z);
                return true;
            }

            return hits.Count() == 0;
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