using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControllerSettings : MonoBehaviour
{
    private const float maxVolume = 80;

    public event Action onSettingsCloseButtonClicked;

    [SerializeField] private UnityEngine.Audio.AudioMixer audioMixer;
    [SerializeField] private AnimationCurve logarithm;

    public CanvasGroup canvasGroup;
    [SerializeField] private Slider sliderVolumeGeneral;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Slider sliderVolumeEffect;
    [SerializeField] private Button closeButton;

    private void Awake ()
    {
        sliderVolumeGeneral.onValueChanged.AddListener(OnSliderVolumeGeneralChanged);
        sliderVolumeMusic.onValueChanged.AddListener(OnSliderVolumeMusicChanged);
        sliderVolumeEffect.onValueChanged.AddListener(OnSliderVolumeEffectChanged);
        closeButton.onClick.AddListener(OnSettingsCloseButtonClicked);
    }

    private void OnDestroy ()
    {
        sliderVolumeGeneral.onValueChanged.RemoveAllListeners();
        sliderVolumeMusic.onValueChanged.RemoveAllListeners();
        sliderVolumeEffect.onValueChanged.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
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

    private void OnSettingsCloseButtonClicked () => onSettingsCloseButtonClicked?.Invoke();
}