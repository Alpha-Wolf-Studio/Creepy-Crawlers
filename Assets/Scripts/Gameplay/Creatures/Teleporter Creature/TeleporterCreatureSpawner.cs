using UnityEngine;

namespace Gameplay.Creatures
{
    public class TeleporterCreatureSpawner : CreatureSpawner
    {
        [Header("Spawner Configurations")]
        [SerializeField] private float maxRangeBetweenCreatures;
        [SerializeField] private TeleporterCreature teleporterCreature;
        [SerializeField] private CreaturePreviewController previewTeleporterFromPrefab;
        [SerializeField] private CreaturePreviewController previewTeleporterToPrefab;
        [SerializeField] private LayerMask collisionCheckMask;

        private CreaturePreviewController _fromSpawnPreview = null;
        private CreaturePreviewController _toSpawnPreview = null;

        private SpawnState _currentSpawnState;
        private Bounds _creatureColliderBounds;

        private void Start()
        {
            if (!_fromSpawnPreview)
                _fromSpawnPreview = Instantiate(previewTeleporterFromPrefab);

            _currentSpawnState = SpawnState.SpawningFirstTeleporter;
            _creatureColliderBounds = teleporterCreature.GetCreatureColliderBounds();
        }

        private void Update()
        {
            if (_currentSpawnState == SpawnState.SpawningFirstTeleporter) 
            {
                _fromSpawnPreview.transform.position = GetPointerPositionInWorldPosition();

                bool validSpawn = IsValidSpawnPosition(_fromSpawnPreview);

                if (validSpawn && IsSpawnButtonDown())
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

                bool validSpawn = IsValidSpawnPosition(_toSpawnPreview);

                if (validSpawn && IsSpawnButtonDown())
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
        private bool IsValidSpawnPosition(CreaturePreviewController preview)
        {
            Collider2D[] collidersInTheCreatureBounds =
                Physics2D.OverlapBoxAll(preview.transform.position + _creatureColliderBounds.center,
                _creatureColliderBounds.size, 0, collisionCheckMask);
            bool validSpawnPosition = collidersInTheCreatureBounds.Length == 0;

            preview.SetPreviewState(validSpawnPosition ? CreaturePreviewController.PreviewState.Valid : CreaturePreviewController.PreviewState.Invalid);

            return validSpawnPosition;
        }


        private void OnDestroy()
        {
            if (_fromSpawnPreview)
                Destroy(_fromSpawnPreview.gameObject);
            _fromSpawnPreview = null;

            if (_toSpawnPreview)
                Destroy(_toSpawnPreview.gameObject);
            _toSpawnPreview = null;
        }

        private enum SpawnState 
        {
            SpawningFirstTeleporter,
            SpawningSecondTeleporter
        }
    }
}