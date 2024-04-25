using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevel : MonoBehaviour
{
    [SerializeField] private List<Image> starsImages = new List<Image>();
    [SerializeField] private Image imgLocked;
    [SerializeField] private Image myImage;
    [SerializeField] private Button button;
    [SerializeField] private Sprite onStarSprite;
    [SerializeField] private Sprite offStarSprite;
    [SerializeField] private Sprite middleStarSpriteOn;
    [SerializeField] private Sprite middleStarSpriteHalfOn;
    [SerializeField] private Sprite middleStarSpriteOff;
    [SerializeField] private Sprite onSprite2;
    [SerializeField] private Sprite offSprite2;

    public static event Action<UiLevel> OnLevelClicked;
    public string nameScene = "";
    public int stars = 0;
    public bool isLock = true;

    private UiButtonEffect buttonEffect;

    private void Awake()
    {
        buttonEffect = GetComponent<UiButtonEffect>();
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
        myImage.sprite = isLocked ? offSprite2 : onSprite2;

        for (int i = 0; i < starsImages.Count; i++)
        {
            starsImages[i].sprite = stars > i ? onStarSprite : offStarSprite;
            starsImages[i].color = isLocked ? Color.gray : Color.white;
        }

        if (starsAmount == 3)
            starsImages[1].sprite = middleStarSpriteOn;
        else if (starsAmount == 2) 
            starsImages[1].sprite = middleStarSpriteHalfOn;
        else
            starsImages[1].sprite = middleStarSpriteOff;

        buttonEffect.enabled = !isLocked;
    }

    private void onLevelClicked()
    {
        OnLevelClicked?.Invoke(this);
    }
}