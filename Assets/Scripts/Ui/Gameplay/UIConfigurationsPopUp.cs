using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay 
{
    public class UIConfigurationsPopUp : MonoBehaviour
    {
        public event Action GoToMenuButtonPressedEvent = delegate { };

        [SerializeField] private UnityEngine.Audio.AudioMixer audioMixer;
        [SerializeField] private AnimationCurve logarithm;

        [SerializeField] private Slider sliderVolumeGeneral;
        [SerializeField] private Slider sliderVolumeEffect;
        [SerializeField] private Slider sliderVolumeMusic;
        [SerializeField] private Button goToMenuButton;

        private const float maxVolume = 80;

        void Start()
        {
            sliderVolumeGeneral.onValueChanged.AddListener(OnSliderVolumeGeneralChanged);
            sliderVolumeMusic.onValueChanged.AddListener(OnSliderVolumeMusicChanged);
            sliderVolumeEffect.onValueChanged.AddListener(OnSliderVolumeEffectChanged);
            goToMenuButton.onClick.AddListener(CallMenuButtonEvent);
        }

        private void OnDestroy()
        {
            sliderVolumeGeneral.onValueChanged.RemoveListener(OnSliderVolumeGeneralChanged);
            sliderVolumeMusic.onValueChanged.RemoveListener(OnSliderVolumeMusicChanged);
            sliderVolumeEffect.onValueChanged.RemoveListener(OnSliderVolumeEffectChanged);
            goToMenuButton.onClick.RemoveListener(CallMenuButtonEvent);
        }

        private void OnSliderVolumeGeneralChanged(float volume)
        {
            float newValue = logarithm.Evaluate(volume) * maxVolume - maxVolume;
            audioMixer.SetFloat("VolumeGeneral", newValue);
        }

        private void OnSliderVolumeMusicChanged(float volume)
        {
            float newValue = logarithm.Evaluate(volume) * maxVolume - maxVolume;
            audioMixer.SetFloat("VolumeMusic", newValue);
        }

        private void OnSliderVolumeEffectChanged(float volume)
        {
            float newValue = logarithm.Evaluate(volume) * maxVolume - maxVolume;
            audioMixer.SetFloat("VolumeEffect", newValue);
        }

        private void CallMenuButtonEvent() 
        {
            GoToMenuButtonPressedEvent?.Invoke();
        }
    }
}
