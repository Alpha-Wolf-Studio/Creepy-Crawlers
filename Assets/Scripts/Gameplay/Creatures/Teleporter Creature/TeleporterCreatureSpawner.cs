using UnityEngine;

namespace Gameplay.Creatures
{
    public class TeleporterCreatureSpawner : CreatureSpawner
    {
        [Header("Spawner Configurations")]
        [SerializeField] private float maxRangeBetweenCreatures;
        [SerializeField] private TeleporterCreature teleporterCreature;
        [SerializeField] private GameObject spawnPreviewPrefab;

        private GameObject _firstSpawnPreview = null;
        private GameObject _secondSpawnPreview = null;

        private SpawnState _currentSpawnState;

        private void Start()
        {
            if (!_firstSpawnPreview)
                _firstSpawnPreview = Instantiate(spawnPreviewPrefab);

            _currentSpawnState = SpawnState.SpawningFirstTeleporter;
        }

        private void Update()
        {
            if (_currentSpawnState == SpawnState.SpawningFirstTeleporter) 
            {
                _firstSpawnPreview.transform.position = GetPointerPositionInWorldPosition();

                if (IsSpawnButtonDown())
                {
                    _secondSpawnPreview = Instantiate(spawnPreviewPrefab);
                    _currentSpawnState = SpawnState.SpawningSecondTeleporter;
                }
            }
            else if (_currentSpawnState == SpawnState.SpawningSecondTeleporter)
            {
                Vector3 newPossiblePosition = GetPointerPositionInWorldPosition();

                if (Vector3.Distance(_firstSpawnPreview.transform.position, newPossiblePosition) > maxRangeBetweenCreatures) 
                {
                    Vector3 directionFromFirstPiece = (newPossiblePosition - _firstSpawnPreview.transform.position).normalized * maxRangeBetweenCreatures;
                    newPossiblePosition = directionFromFirstPiece + _firstSpawnPreview.transform.position;
                }

                _secondSpawnPreview.transform.position = newPossiblePosition;


                if (IsSpawnButtonDown())
                {
                    TeleporterCreature firstTeleporter = Instantiate(teleporterCreature, _firstSpawnPreview.transform.position, Quaternion.identity);
                    TeleporterCreature secondTeleporter = Instantiate(teleporterCreature, _secondSpawnPreview.transform.position, Quaternion.identity);

                    firstTeleporter.SetTeleport(secondTeleporter, TeleporterCreature.TeleporterType.Connected);
                    secondTeleporter.SetTeleport(firstTeleporter, TeleporterCreature.TeleporterType.Connected);

                    CreatureSpawnedEvent?.Invoke(Data);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (_firstSpawnPreview)
                Destroy(_firstSpawnPreview);
            _firstSpawnPreview = null;

            if (_secondSpawnPreview)
                Destroy(_secondSpawnPreview);
            _secondSpawnPreview = null;
        }

        private enum SpawnState 
        {
            SpawningFirstTeleporter,
            SpawningSecondTeleporter
        }
    }
}