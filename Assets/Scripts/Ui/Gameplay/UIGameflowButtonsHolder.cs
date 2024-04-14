using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UIGameflowButtonsHolder : MonoBehaviour
    {
        public event Action PauseButtonPressedEvent = delegate { };
        public event Action ResetGameButtonPressedEvent = delegate { };
        public event Action ConfigurationsButtonPressedEvent = delegate { };

        [Header("Buttons")]
        [SerializeField] private Button pauseGameButton;
        [SerializeField] private Button resetGameButton;
        [SerializeField] private Button configurationsButton;

        private TextMeshProUGUI _pauseButtonText;

        private void Awake()
        {
            _pauseButtonText = pauseGameButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            pauseGameButton.onClick.AddListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.AddListener(CallResetGameButtonEvent);
            configurationsButton.onClick.AddListener(CallConfigurationsButtonEvent);
        }

        private void OnDestroy()
        {
            pauseGameButton.onClick.RemoveListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.RemoveListener(CallResetGameButtonEvent);
            configurationsButton.onClick.RemoveListener(CallConfigurationsButtonEvent);
        }

        private void CallPauseGameButtonEvent() 
        {
            PauseButtonPressedEvent?.Invoke();
            _pauseButtonText.text = Time.timeScale == 1 ? "Pause" : "Unpause";
        }

        private void CallResetGameButtonEvent()
        {
            ResetGameButtonPressedEvent?.Invoke();
        }

        private void CallConfigurationsButtonEvent() 
        {
            ConfigurationsButtonPressedEvent?.Invoke();
        }
    }
}