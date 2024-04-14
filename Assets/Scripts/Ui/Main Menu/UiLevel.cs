using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevel : MonoBehaviour
{
    [SerializeField] private List<Image> starsImages = new List<Image>();
    [SerializeField] private Image imgLocked;
    [SerializeField] private Button button;

    public static event Action<UiLevel> OnLevelClicked;
    public string nameScene = "";
    public int stars = 0;
    public bool isLock = true;

    private void Awake()
    {
        button.onClick.AddListener(onLevelClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void Set(int starsAmount, bool isLocked, string sceneName)
    {
        nameScene = sceneName;
        isLock = isLocked;
        stars = starsAmount;
        button.interactable = !isLocked;
        imgLocked.gameObject.SetActive(isLocked);
        for (int i = 0; i < starsImages.Count; i++)
        {
            if (stars > i)
            {
                starsImages[i].color = Color.yellow;
            }
        }
    }

    private void onLevelClicked()
    {
        OnLevelClicked?.Invoke(this);
    }
}