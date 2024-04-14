using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class CreaturesManager : MonoBehaviour
    {
        public event System.Action<CreatureData> CreatureSpawnedEvent = delegate { };
        public event System.Action CreatureSpawnCancelEvent = delegate { };

        [SerializeField] private List<CreatureData> creatureDataList = new List<CreatureData>();

        public List<CreatureData> CreatureDataList => creatureDataList;

        private CreatureSpawner _currentSpawner;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                CancelCurrentSpawner();
            }
        }

        public void CreateSpawner(CreatureData creatureData) 
        {
            _currentSpawner = Instantiate(creatureData.creatureSpawnerPrefab);
            _currentSpawner.SetCreatureData(creatureData);
            _currentSpawner.CreatureSpawnedEvent += CallCreatureCreatedEvent;
        }

        public void CancelCurrentSpawner() 
        {
            if (_currentSpawner) 
            {
                _currentSpawner.Dispose();
                CreatureSpawnCancelEvent?.Invoke();
            }
        }

        private void CallCreatureCreatedEvent(CreatureData creatureData) 
        {
            CreatureSpawnedEvent?.Invoke(creatureData);
        }
    }
}