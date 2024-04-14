using System.Collections.Generic;
using Gameplay.Creatures;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIControllerGameplay : MonoBehaviour
    {
        [SerializeField] private CreaturesManager creaturesManager;
        [SerializeField] private UICreaturesCard creaturesCardPrefab;
        [SerializeField] private UICreaturesCardHolder creaturesCardsHolder;

        List<CreatureData> _creaturesData = new();

        private void Start()
        {
            Initialize();
        }

        //TODO Connect with level specific data received from levels system
        public void Initialize()
        {
            _creaturesData = creaturesManager.CreatureDataList;

            foreach (var data in _creaturesData)
            {
                UICreaturesCard creaturesCard = Instantiate(creaturesCardPrefab);
                creaturesCard.SetCard(data);
                creaturesCardsHolder.AddCard(creaturesCard);
            }

            creaturesCardsHolder.OnCardSelected += CreateCreatureSpawner;
            creaturesManager.CreatureSpawnedEvent += OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent += OnCreatureSpawnCanceled;
        }

        private void OnDestroy()
        {
            creaturesCardsHolder.OnCardSelected -= CreateCreatureSpawner;
            creaturesManager.CreatureSpawnedEvent -= OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent -= OnCreatureSpawnCanceled;
        }

        private void CreateCreatureSpawner(CreatureData data) 
        {
            creaturesManager.CreateSpawner(data);
            creaturesCardsHolder.HideCards();
        }

        private void OnCreatureSpawned(CreatureData data) 
        {
            creaturesCardsHolder.ShowCards();
        }

        private void OnCreatureSpawnCanceled() 
        {
            creaturesCardsHolder.ShowCards();
        }
    }
}
