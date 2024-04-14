using System.Collections.Generic;
using Gameplay.Creatures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Gameplay
{
    public class UIControllerGameplay : MonoBehaviour
    {
        [Header("Bottom Panel")]
        [SerializeField] private CreaturesManager creaturesManager;
        [SerializeField] private UICreaturesCard creaturesCardPrefab;
        [SerializeField] private UICreaturesCardHolder creaturesCardsHolder;

        [Header("Top Panel")]
        [SerializeField] private UIGameflowButtonsHolder gameflowButtonsHolder;
        [SerializeField] private UIConfigurationsPopUp configurationsPopUp;

        List<CreatureData> _creaturesData = new();

        private bool _paused = false;
        private bool _configurationsPopUpOpen = false;

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

            gameflowButtonsHolder.PauseButtonPressedEvent += TogglePauseGame;
            gameflowButtonsHolder.ResetGameButtonPressedEvent += ResetGame;
            gameflowButtonsHolder.ConfigurationsButtonPressedEvent += ToggleConfigurationsMenu;

            configurationsPopUp.GoToMenuButtonPressedEvent += GoToMenu;
        }

        private void OnDestroy()
        {
            creaturesCardsHolder.OnCardSelected -= CreateCreatureSpawner;
            creaturesManager.CreatureSpawnedEvent -= OnCreatureSpawned;
            creaturesManager.CreatureSpawnCancelEvent -= OnCreatureSpawnCanceled;

            gameflowButtonsHolder.PauseButtonPressedEvent -= TogglePauseGame;
            gameflowButtonsHolder.ResetGameButtonPressedEvent -= ResetGame;
            gameflowButtonsHolder.ConfigurationsButtonPressedEvent -= ToggleConfigurationsMenu;

            configurationsPopUp.GoToMenuButtonPressedEvent -= GoToMenu;
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

        private void TogglePauseGame() 
        {
            _paused = !_paused;
            creaturesCardsHolder.ChangeCardsLockState(_paused);
            Time.timeScale = _paused ? 0 : 1;
        }

        private void ResetGame() 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void GoToMenu() 
        {
            //TODO call to menu scene
        }

        private void ToggleConfigurationsMenu() 
        {
            _configurationsPopUpOpen = !_configurationsPopUpOpen;
            configurationsPopUp.gameObject.SetActive(_configurationsPopUpOpen);
        }
    }
}
