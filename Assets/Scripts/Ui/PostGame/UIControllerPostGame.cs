using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UIControllerPostGame : MonoBehaviour
    {
        [SerializeField] private SceneChangeData mainMenuSceneChangeData;
        [SerializeField] private Button goToMainMenuButton;

        private void Awake()
        {
            goToMainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        private void OnDestroy()
        {
            goToMainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }

        private void GoToMainMenu() 
        {
            SceneSwitcher.ChangeScene(mainMenuSceneChangeData);
        }
    }
}