using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiControllerLevelSelector : MonoBehaviour
{
    public event Action<UiLevel> onLevelSelected;
    public event Action onCloseButton;
    public CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;
    private List<UiLevel> levels = new List<UiLevel>();

    private void Awake()
    {
        UiLevel.OnLevelClicked += UiLevel_OnLevelClicked;
        levels = transform.GetComponentsInChildren<UiLevel>().ToList();
        closeButton.onClick.AddListener(OnCloseButton);
    }

    private void Start()
    {
        SaveGame save = SaveAndLoad.LoadAll();
        int index = 0;

        for (index = 0; index < save.level.Count; index++)
            levels[index].Set(save.level[index].stars, false, "Scene" + (index + 1));

        levels[index].Set(0, false, "Scene" + (index + 1));
        index++;

        for (int i = index; i < levels.Count; i++)
            levels[i].Set(0, true, "Scene" + (i + 1));
    }

    private void OnDestroy()
    {
        UiLevel.OnLevelClicked -= UiLevel_OnLevelClicked;
    }

    private void OnCloseButton() => onCloseButton?.Invoke();

    private void UiLevel_OnLevelClicked(UiLevel obj) => onLevelSelected?.Invoke(obj);
}