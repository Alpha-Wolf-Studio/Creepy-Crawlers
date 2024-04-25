using System.Collections.Generic;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using CustomSceneSwitcher.Switcher.External;
using Gameplay.Creatures;
using Gameplay.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Gameplay
{
    public class UIControllerGameplay : MonoBehaviour
    {
        [Header("Bottom Panel")]
        [SerializeField] private UICreaturesCard creaturesCardPrefab;
        [SerializeField] private UICreaturesCardHolder creaturesCardsHolder;
        [SerializeField] private SceneChangeData levelSceneChangeData;

        [Header("Top Panel")]
        [SerializeField] private UIGameflowButtonsHolder gameflowButtonsHolder;
        [SerializeField] private UIConfigurationsPopUp configurationsPopUp;
        [SerializeField] private SceneChangeData mainMenuSceneChangeData;

        [Header("Scenes Data")]
        [SerializeField] private SceneReference repeatLevelScene;
        [SerializeField] private SceneReference completeLevelScene;

        private CreaturesManager _creaturesManager;
        private bool _paused = false;
        private bool _configurationsPopUpOpen = false;

        private void OnEnable()
        {
            _creaturesManager = FindObjectOfType<CreaturesManager>();
            _creaturesManager.SceneCreaturesInitializedEvent += InitializeCreaturesCards;

            LevelSystem.OnGnomeAbsorbed += EnableFinishLevelButton;
            LevelSystem.OnLevelStarted += CheckFinishLevelButtonStartingState;
            LevelSystem.OnLevelCleared += GoToNextLevel;
            LevelSystem.OnLevelFailed += RepeatLevel;
        }

        private void Start()
        {
            Time.timeScale = 1;
            creaturesCardsHolder.OnCardSelected += CreateCreatureSpawner;
            _creaturesManager.CreatureSpawnedEvent += OnCreatureSpawned;
            _creaturesManager.CreatureSpawnCancelEvent += OnCreatureSpawnCanceled;

            gameflowButtonsHolder.PauseButtonPressedEvent += TogglePauseGame;
            gameflowButtonsHolder.ResetGameButtonPressedEvent += ResetGame;
            gameflowButtonsHolder.ConfigurationsButtonPressedEvent += ToggleConfigurationsMenu;

            configurationsPopUp.GoToMenuButtonPressedEvent += GoToMenu;

        }

        private void OnDisable()
        {
            _creaturesManager.SceneCreaturesInitializedEvent -= InitializeCreaturesCards;

            LevelSystem.OnGnomeAbsorbed -= EnableFinishLevelButton;
            LevelSystem.OnLevelStarted -= CheckFinishLevelButtonStartingState;
            LevelSystem.OnLevelCleared -= GoToNextLevel;
            LevelSystem.OnLevelFailed -= RepeatLevel;
        }

        private void OnDestroy()
        {            
            _creaturesManager.CreatureSpawnedEvent -= OnCreatureSpawned;
            _creaturesManager.CreatureSpawnCancelEvent -= OnCreatureSpawnCanceled;
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
                if (creatureSceneData.CreatureAmount == 0) continue;

                UICreaturesCard creaturesCard = Instantiate(creaturesCardPrefab);
                creaturesCard.SetCard(creatureSceneData);
                creaturesCardsHolder.AddCard(creaturesCard);
            }
            creaturesCardsHolder.UpdateCards();
        }

        private void CreateCreatureSpawner(CreatureSceneData data) 
        {
            _creaturesManager.CreateSpawner(data.CreatureData);
            creaturesCardsHolder.HideCardsCompletely();
            creaturesCardsHolder.LockCardSlot();
        }

        private void OnCreatureSpawned() 
        {
            creaturesCardsHolder.UpdateCards();
            creaturesCardsHolder.HideCardsPartially();
            creaturesCardsHolder.UnlockCardSlot();
        }

        private void OnCreatureSpawnCanceled() 
        {
            creaturesCardsHolder.HideCardsPartially();
            creaturesCardsHolder.UnlockCardSlot();
        }

        private void TogglePauseGame() 
        {
            _paused = !_paused;
            creaturesCardsHolder.ChangeCardsLockState(_paused);
            Time.timeScale = _paused ? 0 : 1;
        }

        private void ResetGame() 
        {
            Time.timeScale = 1;

            levelSceneChangeData.SetScene(repeatLevelScene);
            SceneSwitcher.ChangeScene(levelSceneChangeData);
        }

        private void GoToMenu() 
        {
            Time.timeScale = 1;
            SceneSwitcher.ChangeScene(mainMenuSceneChangeData);
        }

        private void ToggleConfigurationsMenu() 
        {
            _configurationsPopUpOpen = !_configurationsPopUpOpen;
            configurationsPopUp.gameObject.SetActive(_configurationsPopUpOpen);
        }

        private void EnableFinishLevelButton() => gameflowButtonsHolder.EnableFinishLevelButton();

        private void DisableFinishLevelButton() => gameflowButtonsHolder.DisableFinishLevelButton();

        private void CheckFinishLevelButtonStartingState(int currentLevel, int gnomesInLevel) 
        {
            if (SaveAndLoad.SaveGame != null) 
            {
                SaveGame saveGame = SaveAndLoad.SaveGame;
                if (saveGame.maxLevel >= currentLevel)
                    EnableFinishLevelButton();
                else
                    DisableFinishLevelButton();
            }
            else 
            {
                DisableFinishLevelButton();
            }
        }

        private void GoToNextLevel(int currentLevel) 
        {
            Time.timeScale = 1;
            levelSceneChangeData.SetScene(completeLevelScene);
            SceneSwitcher.ChangeScene(levelSceneChangeData);
        }

        private void RepeatLevel(int currentLevel) 
        {
            Time.timeScale = 1;
            levelSceneChangeData.SetScene(repeatLevelScene);
            SceneSwitcher.ChangeScene(levelSceneChangeData);
        }
    }
}
