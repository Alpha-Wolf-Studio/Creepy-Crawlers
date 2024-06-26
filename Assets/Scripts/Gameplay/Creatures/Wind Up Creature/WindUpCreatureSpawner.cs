using UnityEngine;

namespace Gameplay.Creatures
{
    public class WindUpCreatureSpawner : CreatureSpawner
    {
        [Header("Spawner Configurations")]
        [SerializeField] private WindUpCreature windUpCreature;
        [SerializeField] private GameObject spawnPreviewPrefab;

        private GameObject _spawnPreview = null;

        private void Start()
        {
            if(!_spawnPreview)
                _spawnPreview = Instantiate(spawnPreviewPrefab);
        }

        private void Update()
        {
            _spawnPreview.transform.position = GetPointerPositionInWorldPosition();

            if (IsSpawnButtonDown()) 
            {
                Instantiate(windUpCreature, _spawnPreview.transform.position, Quaternion.identity);
                CreatureSpawnedEvent?.Invoke(Data);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if(_spawnPreview)
                Destroy(_spawnPreview);
            _spawnPreview = null;
        }
    }
}