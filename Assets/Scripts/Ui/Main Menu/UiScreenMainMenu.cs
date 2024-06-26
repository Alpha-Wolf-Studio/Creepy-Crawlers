using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using CustomSceneSwitcher.Switcher.External;

public class UiScreenMainMenu : MonoBehaviour
{
    [SerializeField] private SceneChangeData sceneChangeData;
    [SerializeField] private List<SceneReference> allLevels = new List<SceneReference>();
    [SerializeField] private UiControllerMainMenu controllerMainMenu;
    [SerializeField] private UiControllerLevelSelector controllerLevelSelector;
    [SerializeField] private UiControllerSettings controllerSettings;
    [SerializeField] private UiControllerCredits controllerCredits;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float durationFade = 1.0f;
    private IEnumerator switchPanel;

    private void Awake ()
    {
        controllerMainMenu.onPlayButtonClicked += ControllerMainMenu_onPlayButtonClicked;
        controllerMainMenu.onSettingsButtonClicked += ControllerMainMenu_onSettingsButtonClicked;
        controllerMainMenu.onCreditsButtonClicked += ControllerMainMenu_onCreditsButtonClicked;
        controllerSettings.onSettingsCloseButtonClicked += ControllerSettings_onSettingsCloseButtonClicked;
        controllerCredits.onCreditsCloseButtonClicked += ControllerCredits_onCreditsCloseButtonClicked;
        controllerLevelSelector.onLevelSelected += ControllerLevelSelector_onLevelSelected;
        controllerLevelSelector.onCloseButton += ControllerLevelSelector_onCloseButton;
    }

    private void Start ()
    {
        SetPanel(controllerMainMenu.canvasGroup, true);
        SetPanel(controllerLevelSelector.canvasGroup, false);
        SetPanel(controllerSettings.canvasGroup, false);
        SetPanel(controllerCredits.canvasGroup, false);
    }

    private void OnDestroy ()
    {
        controllerMainMenu.onPlayButtonClicked -= ControllerMainMenu_onPlayButtonClicked;
        controllerMainMenu.onSettingsButtonClicked -= ControllerMainMenu_onSettingsButtonClicked;
        controllerMainMenu.onCreditsButtonClicked -= ControllerMainMenu_onCreditsButtonClicked;
        controllerSettings.onSettingsCloseButtonClicked -= ControllerSettings_onSettingsCloseButtonClicked;
        controllerCredits.onCreditsCloseButtonClicked -= ControllerCredits_onCreditsCloseButtonClicked;
        controllerLevelSelector.onLevelSelected -= ControllerLevelSelector_onLevelSelected;
    }

    private void ControllerMainMenu_onPlayButtonClicked () => SwitchController(durationFade, controllerLevelSelector.canvasGroup, controllerMainMenu.canvasGroup);

    private void ControllerMainMenu_onSettingsButtonClicked () => SwitchController(durationFade, controllerSettings.canvasGroup, controllerMainMenu.canvasGroup);

    private void ControllerMainMenu_onCreditsButtonClicked () => SwitchController(durationFade, controllerCredits.canvasGroup, controllerMainMenu.canvasGroup);

    private void ControllerMainMenu_onExitButtonClicked ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Debug.Log("No Exit");
#else
        Application.Quit();
#endif
    }

    private void ControllerCredits_onCreditsCloseButtonClicked () => SwitchController(durationFade, controllerMainMenu.canvasGroup, controllerCredits.canvasGroup);

    private void ControllerSettings_onSettingsCloseButtonClicked () => SwitchController(durationFade, controllerMainMenu.canvasGroup, controllerSettings.canvasGroup);

    private void ControllerLevelSelector_onCloseButton() => SwitchController(durationFade, controllerMainMenu.canvasGroup, controllerLevelSelector.canvasGroup);

    private void SwitchController (float duration, CanvasGroup on, CanvasGroup off)
    {
        if (switchPanel != null)
            StopCoroutine(switchPanel);

        switchPanel = SwitchPanel(duration, on, off);
        StartCoroutine(switchPanel);
    }

    private IEnumerator SwitchPanel (float duration, CanvasGroup on, CanvasGroup off)
    {
        SetPanel(off, true);
        SetPanel(on, false);

        off.blocksRaycasts = false;
        off.interactable = false;

        float currentDuration = 0;
        while (currentDuration < duration)
        {
            currentDuration += Time.unscaledDeltaTime;
            float fade = animationCurve.Evaluate(currentDuration / duration);
            on.alpha = fade;
            off.alpha = 1 - fade;
            yield return null;
        }

        SetPanel(on, true);
    }

    private void SetPanel (CanvasGroup canvasGroup, bool isEnable)
    {
        canvasGroup.alpha = isEnable ? 1 : 0;
        canvasGroup.blocksRaycasts = isEnable;
        canvasGroup.interactable = isEnable;
    }

    private void ControllerLevelSelector_onLevelSelected(UiLevel obj)
    {
        SceneReference sceneToLoad = allLevels.Find(s => s.ScenePath.Split("/")[^1].Split(".")[0] == obj.nameScene);
        sceneChangeData.SetScene(sceneToLoad);
        SceneSwitcher.ChangeScene(sceneChangeData);
    }
}