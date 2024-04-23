using UnityEngine;

namespace Gameplay.Creatures
{
    public class WallCreatureSpawner : CreatureSpawner
    {
        [Header("Spawn Configuration")]
        [SerializeField] private WallCreature wallCreature;
        [SerializeField] private CreaturePreviewController previewPrefab;

        [Header("Hook Configuration")]
        [SerializeField] private LayerMask collisionCheckMask;

        private CreaturePreviewController _spawnPreview = null;
        private Bounds _creatureColliderBounds;
        private bool _validSpawnPosition;

        private void Start()
        {
            if (!_spawnPreview)
                _spawnPreview = Instantiate(previewPrefab);

            _creatureColliderBounds = wallCreature.GetCreatureColliderBounds();
        }

        private void Update()
        {
            _spawnPreview.transform.position = GetPointerPositionInWorldPosition();

            SetValidSpawnBool();

            if (_validSpawnPosition && IsSpawnButtonDown())
            {
                Instantiate(wallCreature, _spawnPreview.transform.position, Quaternion.identity);
                CreatureSpawnedEvent?.Invoke(Data);
                Destroy(gameObject);
            }
        }

        private void SetValidSpawnBool() 
        {
            Collider2D[] collidersInTheCreatureBounds =
                Physics2D.OverlapBoxAll(_spawnPreview.transform.position + _creatureColliderBounds.center,
                _creatureColliderBounds.size, 0, collisionCheckMask);
            _validSpawnPosition = collidersInTheCreatureBounds.Length > 0;

            _spawnPreview.SetPreviewState(_validSpawnPosition ? CreaturePreviewController.PreviewState.Valid : CreaturePreviewController.PreviewState.Invalid);
        }

        private void OnDestroy()
        {
            if (_spawnPreview)
                Destroy(_spawnPreview.gameObject);
            _spawnPreview = null;
        }
    }
}