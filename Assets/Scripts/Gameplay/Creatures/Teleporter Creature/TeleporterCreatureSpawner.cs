using UnityEngine;

namespace Gameplay.Creatures
{
    public class TeleporterCreatureSpawner : CreatureSpawner
    {
        [Header("Spawner Configurations")]
        [SerializeField] private float maxRangeBetweenCreatures;
        [SerializeField] private TeleporterCreature teleporterCreature;
        [SerializeField] private GameObject previewTeleporterFromPrefab;
        [SerializeField] private GameObject previewTeleporterToPrefab;

        private GameObject _fromSpawnPreview = null;
        private GameObject _toSpawnPreview = null;

        private SpawnState _currentSpawnState;

        private void Start()
        {
            if (!_fromSpawnPreview)
                _fromSpawnPreview = Instantiate(previewTeleporterFromPrefab);

            _currentSpawnState = SpawnState.SpawningFirstTeleporter;
        }

        private void Update()
        {
            if (_currentSpawnState == SpawnState.SpawningFirstTeleporter) 
            {
                _fromSpawnPreview.transform.position = GetPointerPositionInWorldPosition();

                if (IsSpawnButtonDown())
                {
                    _toSpawnPreview = Instantiate(previewTeleporterToPrefab);
                    _currentSpawnState = SpawnState.SpawningSecondTeleporter;
                }
            }
            else if (_currentSpawnState == SpawnState.SpawningSecondTeleporter)
            {
                Vector3 newPossiblePosition = GetPointerPositionInWorldPosition();

                if (Vector3.Distance(_fromSpawnPreview.transform.position, newPossiblePosition) > maxRangeBetweenCreatures) 
                {
                    Vector3 directionFromFirstPiece = (newPossiblePosition - _fromSpawnPreview.transform.position).normalized * maxRangeBetweenCreatures;
                    newPossiblePosition = directionFromFirstPiece + _fromSpawnPreview.transform.position;
                }

                _toSpawnPreview.transform.position = newPossiblePosition;


                if (IsSpawnButtonDown())
                {
                    TeleporterCreature firstTeleporter = Instantiate(teleporterCreature, _fromSpawnPreview.transform.position, Quaternion.identity);
                    TeleporterCreature secondTeleporter = Instantiate(teleporterCreature, _toSpawnPreview.transform.position, Quaternion.identity);

                    firstTeleporter.SetTeleport(secondTeleporter, TeleporterCreature.TeleporterType.From);
                    secondTeleporter.SetTeleport(firstTeleporter, TeleporterCreature.TeleporterType.To);

                    CreatureSpawnedEvent?.Invoke(Data);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (_fromSpawnPreview)
                Destroy(_fromSpawnPreview);
            _fromSpawnPreview = null;

            if (_toSpawnPreview)
                Destroy(_toSpawnPreview);
            _toSpawnPreview = null;
        }

        private enum SpawnState 
        {
            SpawningFirstTeleporter,
            SpawningSecondTeleporter
        }
    }
}