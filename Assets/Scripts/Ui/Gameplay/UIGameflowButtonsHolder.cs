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
        public event Action GoToMenuButtonPressedEvent = delegate { };

        [Header("Buttons")]
        [SerializeField] private Button pauseGameButton;
        [SerializeField] private Button resetGameButton;
        [SerializeField] private Button goToMenuButton;

        private TextMeshProUGUI _pauseButtonText;

        private void Awake()
        {
            _pauseButtonText = pauseGameButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            pauseGameButton.onClick.AddListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.AddListener(CallResetGameButtonEvent);
            goToMenuButton.onClick.AddListener(CallGoToMenuButtonEvent);
        }

        private void OnDestroy()
        {
            pauseGameButton.onClick.RemoveListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.RemoveListener(CallResetGameButtonEvent);
            goToMenuButton.onClick.RemoveListener(CallGoToMenuButtonEvent);
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

        private void CallGoToMenuButtonEvent()
        {
            GoToMenuButtonPressedEvent?.Invoke();
        }
    }
}