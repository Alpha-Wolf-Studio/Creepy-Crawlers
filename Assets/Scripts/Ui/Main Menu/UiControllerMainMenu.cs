using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControllerMainMenu : MonoBehaviour
{
    public event Action onPlayButtonClicked;
    public event Action onSettingsButtonClicked;
    public event Action onCreditsButtonClicked;

    public CanvasGroup canvasGroup;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button creditsButton;

    private void Awake ()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingButton.onClick.AddListener(OnSettingsButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
    }

    private void OnDestroy ()
    {
        playButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
    }

    private void OnPlayButtonClicked () => onPlayButtonClicked?.Invoke();
    private void OnSettingsButtonClicked () => onSettingsButtonClicked?.Invoke();
    private void OnCreditsButtonClicked () => onCreditsButtonClicked?.Invoke();
}