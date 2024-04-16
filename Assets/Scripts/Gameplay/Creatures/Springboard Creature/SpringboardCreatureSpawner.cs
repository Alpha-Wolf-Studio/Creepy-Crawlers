using UnityEngine;

namespace Gameplay.Creatures
{
    public class SpringboardCreatureSpawner : CreatureSpawner
    {
        [Header("Spawner Configurations")]
        [SerializeField] private SpringboardCreature springboardCreature;
        [SerializeField] private GameObject spawnPreviewPrefab;

        private GameObject _spawnPreview;
        private SpawnState _currentSpawnState;

        private void Start()
        {
            if (!_spawnPreview)
                _spawnPreview = Instantiate(spawnPreviewPrefab);

            _currentSpawnState = SpawnState.SelectingLocation;
        }

        private void Update()
        {
            if (_currentSpawnState == SpawnState.SelectingLocation)
            {
                _spawnPreview.transform.position = GetPointerPositionInWorldPosition();

                if (IsSpawnButtonDown())
                {
                    _currentSpawnState = SpawnState.SelectingRotation;
                }
            }
            else if (_currentSpawnState == SpawnState.SelectingRotation)
            {
                _spawnPreview.transform.up = (GetPointerPositionInWorldPosition() - _spawnPreview.transform.position).normalized;

                if (IsSpawnButtonDown())
                {
                    SpringboardCreature springBoardCreature = Instantiate(springboardCreature, _spawnPreview.transform.position, _spawnPreview.transform.rotation);
                    springBoardCreature.SetReboundDirection(_spawnPreview.transform.up);
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

        private enum SpawnState
        {
            SelectingLocation,
            SelectingRotation
        }
    }
}
