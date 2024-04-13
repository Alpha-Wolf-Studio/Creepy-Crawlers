using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using UnityEngine;
using UnityEngine.UI;

namespace CustomSceneSwitcher.Examples.Scripts
{
    public class ChangeSceneUI : MonoBehaviour
    {
        [Header("Ui references")] 
        [SerializeField] private Button changeSceneButton;
        [Space(10)]
        [SerializeField] private SceneChangeData sceneChangeData;

        private void Start()
        {
            changeSceneButton.onClick.AddListener(ChangeScene);
        }

        private void OnDestroy()
        {
            changeSceneButton.onClick.RemoveListener(ChangeScene);
        }

        private void ChangeScene()
        {
            SceneSwitcher.ChangeScene(sceneChangeData);
        }
    }
}
