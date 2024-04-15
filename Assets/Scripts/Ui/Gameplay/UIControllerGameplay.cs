using System.Collections.Generic;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using Gameplay.Creatures;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIControllerGameplay : MonoBehaviour
    {
        [Header("Bottom Panel")]
        [SerializeField] private CreaturesManager creaturesManager;
        [SerializeField] private UICreaturesCard creaturesCardPrefab;
        [SerializeField] private UICreaturesCardHolder creaturesCardsHolder;
        [SerializeField] private SceneChangeData ownSceneChangeData;

        [Header("Top Panel")]
        [SerializeField] private UIGameflowButtonsHolder gameflowButtonsHolder;
        [SerializeField] private UIConfigurationsPopUp configurationsPopUp;
        [SerializeField] private SceneChangeData mainMenuSceneChangeData;

        private bool _paused = false;
        private bool _configurationsPopUpOpen = false;

        private void Start()
        {
            creaturesCardsHolder.OnCardSelected += CreateCreatureSpawner;
            creaturesManager.CreatureSpawnedEvent += OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent += OnCreatureSpawnCanceled;

            gameflowButtonsHolder.PauseButtonPressedEvent += TogglePauseGame;
            gameflowButtonsHolder.ResetGameButtonPressedEvent += ResetGame;
            gameflowButtonsHolder.ConfigurationsButtonPressedEvent += ToggleConfigurationsMenu;

            configurationsPopUp.GoToMenuButtonPressedEvent += GoToMenu;
        }

        private void OnEnable()
        {
            creaturesManager.SceneCreaturesInitializedEvent += InitializeCreaturesCards;
        }

        private void OnDisable()
        {
            creaturesManager.SceneCreaturesInitializedEvent -= InitializeCreaturesCards;
        }

        private void OnDestroy()
        {            
            creaturesManager.CreatureSpawnedEvent -= OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent -= OnCreatureSpawnCanceled;
            creaturesCardsHolder.OnCardSelected -= CreateCreatureSpawner;

            gameflowButtonsHolder.PauseButtonPressedEvent -= TogglePauseGame;
            gameflowButtonsHolder.ResetGameButtonPressedEvent -= ResetGame;
            gameflowButtonsHolder.ConfigurationsButtonPressedEvent -= ToggleConfigurationsMenu;

            configurationsPopUp.GoToMenuButtonPressedEvent -= GoToMenu;
        }

        public void InitializeCreaturesCards(List<CreatureSceneData> creaturesSceneData)
        {
            foreach (var creatureSceneData in creaturesSceneData)
            {
                UICreaturesCard creaturesCard = Instantiate(creaturesCardPrefab);
                creaturesCard.SetCard(creatureSceneData);
                creaturesCardsHolder.AddCard(creaturesCard);
            }
        }

        private void CreateCreatureSpawner(CreatureSceneData data) 
        {
            creaturesManager.CreateSpawner(data.CreatureData);
            creaturesCardsHolder.HideCards();
        }

        private void OnCreatureSpawned() 
        {
            creaturesCardsHolder.UpdateCardsData();
            creaturesCardsHolder.ShowCards();
        }

        private void OnCreatureSpawnCanceled() 
        {
            creaturesCardsHolder.ShowCards();
        }

        private void TogglePauseGame() 
        {
            _paused = !_paused;
            creaturesCardsHolder.ChangeCardsLockState(_paused);
            Time.timeScale = _paused ? 0 : 1;
        }

        private void ResetGame() 
        {
            SceneSwitcher.ChangeScene(ownSceneChangeData);
        }

        private void GoToMenu() 
        {
            SceneSwitcher.ChangeScene(mainMenuSceneChangeData);
        }

        private void ToggleConfigurationsMenu() 
        {
            _configurationsPopUpOpen = !_configurationsPopUpOpen;
            configurationsPopUp.gameObject.SetActive(_configurationsPopUpOpen);
        }
    }
}
