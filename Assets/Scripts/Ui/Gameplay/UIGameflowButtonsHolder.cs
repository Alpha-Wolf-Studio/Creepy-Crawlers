using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UIGameflowButtonsHolder : MonoBehaviour
    {
        public static event Action onFinishLevelPressed;
        public event Action PauseButtonPressedEvent = delegate { };
        public event Action ResetGameButtonPressedEvent = delegate { };
        public event Action ConfigurationsButtonPressedEvent = delegate { };

        [Header("Buttons")]
        [SerializeField] private Button finishLevelButton;
        [SerializeField] private Button pauseGameButton;
        [SerializeField] private Button resetGameButton;
        [SerializeField] private Button configurationsButton;

        [Header("Audio Config")]
        [SerializeField] private AudioClip audioClip = null;
        [SerializeField] private UnityEngine.Audio.AudioMixerGroup audioMixerGroup = null;
        private AudioSource audioSource = null;
        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.loop = false;
        }

        void Start()
        {
            finishLevelButton.onClick.AddListener(OnFinishLevelButton);
            pauseGameButton.onClick.AddListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.AddListener(CallResetGameButtonEvent);
            configurationsButton.onClick.AddListener(CallConfigurationsButtonEvent);

            pauseGameButton.onClick.AddListener(CallButtonSound);
            resetGameButton.onClick.AddListener(CallButtonSound);
            configurationsButton.onClick.AddListener(CallButtonSound);
        }

        private void OnDestroy()
        {
            finishLevelButton.onClick.RemoveAllListeners();
            pauseGameButton.onClick.RemoveListener(CallPauseGameButtonEvent);
            resetGameButton.onClick.RemoveListener(CallResetGameButtonEvent);
            configurationsButton.onClick.RemoveListener(CallConfigurationsButtonEvent);

            pauseGameButton.onClick.RemoveListener(CallButtonSound);
            resetGameButton.onClick.RemoveListener(CallButtonSound);
            configurationsButton.onClick.RemoveListener(CallButtonSound);
        }

        public void DisableFinishLevelButton() => finishLevelButton.interactable = false;
        public void EnableFinishLevelButton() => finishLevelButton.interactable = true;

        private void OnFinishLevelButton() => onFinishLevelPressed?.Invoke();

        private void CallPauseGameButtonEvent() => PauseButtonPressedEvent?.Invoke();

        private void CallResetGameButtonEvent() => ResetGameButtonPressedEvent?.Invoke();

        private void CallConfigurationsButtonEvent() => ConfigurationsButtonPressedEvent?.Invoke();

        private void CallButtonSound() => audioSource.Play();
    }
}