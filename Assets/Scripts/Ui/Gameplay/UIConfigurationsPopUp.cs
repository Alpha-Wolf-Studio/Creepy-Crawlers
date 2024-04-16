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

        private readonly string VolumenGeneralMixerKey = "VolumeGeneral";
        private readonly string MusicGeneralMixerKey = "VolumeMusic";
        private readonly string SFXGeneralMixerKey = "VolumeEffect";


        private void Awake()
        {
            sliderVolumeGeneral.onValueChanged.AddListener(OnSliderVolumeGeneralChanged);
            sliderVolumeMusic.onValueChanged.AddListener(OnSliderVolumeMusicChanged);
            sliderVolumeEffect.onValueChanged.AddListener(OnSliderVolumeEffectChanged);
            goToMenuButton.onClick.AddListener(CallMenuButtonEvent);
        }

        void Start()
        {
            SetSliderStartValue(sliderVolumeGeneral, VolumenGeneralMixerKey);
            SetSliderStartValue(sliderVolumeMusic, MusicGeneralMixerKey);
            SetSliderStartValue(sliderVolumeEffect, SFXGeneralMixerKey);
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
            audioMixer.SetFloat(VolumenGeneralMixerKey, newValue);
        }

        private void OnSliderVolumeMusicChanged(float volume)
        {
            float newValue = logarithm.Evaluate(volume) * maxVolume - maxVolume;
            audioMixer.SetFloat(MusicGeneralMixerKey, newValue);
        }

        private void OnSliderVolumeEffectChanged(float volume)
        {
            float newValue = logarithm.Evaluate(volume) * maxVolume - maxVolume;
            audioMixer.SetFloat(SFXGeneralMixerKey, newValue);
        }

        private void SetSliderStartValue(Slider slider, string mixerKey)
        {
            if (audioMixer.GetFloat(mixerKey, out float effectsValue))
            {
                float sliderValue = logarithm.InverseEvaluate((effectsValue + maxVolume) / maxVolume);
                slider.SetValueWithoutNotify(sliderValue);
            }
        }

        private void CallMenuButtonEvent() 
        {
            GoToMenuButtonPressedEvent?.Invoke();
        }
    }
}
