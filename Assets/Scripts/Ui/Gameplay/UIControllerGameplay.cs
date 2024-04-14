using System.Collections.Generic;
using Gameplay.Creatures;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIControllerGameplay : MonoBehaviour
    {

        [SerializeField] private CreaturesManager creaturesManager;
        [SerializeField] private UICreaturesCard creaturesCardPrefab;
        [SerializeField] private GameObject creaturesCardsHolder;

        List<UICreaturesCard> _uiCreaturesCards = new();
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
                UICreaturesCard creaturesCard = Instantiate(creaturesCardPrefab, creaturesCardsHolder.transform);
                creaturesCard.SetCard(data);
                creaturesCard.OnCardSelected += CreateCreatureSpawner;
                _uiCreaturesCards.Add(creaturesCard);
            }

            creaturesManager.CreatureSpawnedEvent += OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent += OnCreatureSpawnCanceled;
        }

        private void OnDestroy()
        {
            foreach (var creaturesCard in _uiCreaturesCards)
            {
                creaturesCard.OnCardSelected -= CreateCreatureSpawner;
            }

            creaturesManager.CreatureSpawnedEvent -= OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent -= OnCreatureSpawnCanceled;
        }

        private void CreateCreatureSpawner(CreatureData data) 
        {
            creaturesManager.CreateSpawner(data);
            creaturesCardsHolder.SetActive(false);
        }

        private void OnCreatureSpawned(CreatureData data) 
        {
            creaturesCardsHolder.SetActive(true);
        }

        private void OnCreatureSpawnCanceled() 
        {
            creaturesCardsHolder.SetActive(true);
        }
    }
}
