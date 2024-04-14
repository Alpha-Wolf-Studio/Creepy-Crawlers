using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControllerCredits : MonoBehaviour
{
    public event Action onCreditsCloseButtonClicked;

    public CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;

    private void Awake ()
    {
        closeButton.onClick.AddListener(OnCreditsCloseButtonClicked);
    }

    private void OnDestroy ()
    {
        closeButton.onClick.RemoveAllListeners();
    }

    private void OnCreditsCloseButtonClicked () => onCreditsCloseButtonClicked?.Invoke();
}