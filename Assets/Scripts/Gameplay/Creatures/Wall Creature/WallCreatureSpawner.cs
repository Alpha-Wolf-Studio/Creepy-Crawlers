using UnityEngine;

namespace Gameplay.Creatures
{
    public class WallCreatureSpawner : CreatureSpawner
    {
        [Header("Spawn Configuration")]
        [SerializeField] private WallCreature wallCreature;
        [SerializeField] private GameObject previewPrefab;

        [Header("Hook Configuration")]
        [SerializeField] private LayerMask collisionCheckMask;

        private GameObject _spawnPreview = null;
        private Bounds _creatureColliderBounds;

        private void Start()
        {
            if (!_spawnPreview)
                _spawnPreview = Instantiate(previewPrefab);

            _creatureColliderBounds = wallCreature.GetCreatureColliderBounds();
        }

        private void Update()
        {
            _spawnPreview.transform.position = GetPointerPositionInWorldPosition();

            if (IsSpawnButtonDown())
            {

                Collider2D[] collidersInTheCreatureBounds = 
                    Physics2D.OverlapBoxAll(_spawnPreview.transform.position + _creatureColliderBounds.center,
                    _creatureColliderBounds.size, 0, collisionCheckMask);

                if (collidersInTheCreatureBounds.Length > 0) 
                {
                    Instantiate(wallCreature, _spawnPreview.transform.position, Quaternion.identity);
                    CreatureSpawnedEvent?.Invoke(Data);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (_spawnPreview)
                Destroy(_spawnPreview);
            _spawnPreview = null;
        }
    }
}