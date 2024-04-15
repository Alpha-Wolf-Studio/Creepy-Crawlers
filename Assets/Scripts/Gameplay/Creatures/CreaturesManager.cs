using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Creatures
{
    public class CreaturesManager : MonoBehaviour
    {
        public event System.Action CreatureSpawnedEvent = delegate { };
        public event System.Action CreatureSpawnCancelEvent = delegate { };
        public event System.Action<List<CreatureSceneData>> SceneCreaturesInitializedEvent = delegate { };

        [SerializeField] private List<CreatureSceneData> creatureDataList = new();

        private List<CreatureSceneData> _creatureDataList = new();

        private CreatureSpawner _currentSpawner;

        private void Start()
        {
            _creatureDataList = new();
            foreach (var creatureData in creatureDataList)
            {
                _creatureDataList.Add(creatureData);
            }
            SceneCreaturesInitializedEvent?.Invoke(_creatureDataList);
        }

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
            CreatureSceneData createdCreatureSceneData = creatureDataList.First(i => i.CreatureData == creatureData);
            createdCreatureSceneData.CreatureAmount--;

            if(createdCreatureSceneData.CreatureAmount <= 0)
                creatureDataList.Remove(createdCreatureSceneData);

            CreatureSpawnedEvent?.Invoke();
        }
    }
}