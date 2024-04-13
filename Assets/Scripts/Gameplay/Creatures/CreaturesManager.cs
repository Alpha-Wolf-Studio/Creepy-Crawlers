using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class CreaturesManager : MonoBehaviour
    {
        public event System.Action<CreatureData> CreatureSpawnedEvent = delegate { };

        [SerializeField] private List<CreatureData> creatureDataList = new List<CreatureData>();

        public List<CreatureData> CreatureDataList => creatureDataList;

        private CreatureSpawner _currentSpawner;


        public void CreateSpawner(CreatureData creatureData) 
        {
            _currentSpawner = Instantiate(creatureData.creatureSpawnerPrefab);
            _currentSpawner.SetCreatureData(creatureData);
            _currentSpawner.CreatureSpawnedEvent += CallCreatureCreatedEvent;
        }

        public void CancelCurrentSpawner() 
        {
            _currentSpawner.Dispose();
        }

        private void CallCreatureCreatedEvent(CreatureData creatureData) 
        {
            CreatureSpawnedEvent?.Invoke(creatureData);
        }

        [ContextMenu("Create Random Spawner")]
        public void CreateRandomSpawner() 
        {
            int random = Random.Range(0, creatureDataList.Count);
            Instantiate(creatureDataList[random].creatureSpawnerPrefab);
        }
    }
}