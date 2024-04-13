using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomPlayFabAPI
{
    public class CustomPlayFabLoadingScreen : MonoBehaviour
    {

        [SerializeField] private CustomPlayFabSingleton customPlayFabSingleton;
        [Space(10)]
        [Header("General Loading Screen Configurations")]
        [SerializeField] private CanvasGroup generalCanvasGroup;
        
        [Header("Display Name Configurations")]
        [SerializeField] private CanvasGroup displayNameCanvasGroup;
        [SerializeField] private int displayMinLenght = 3;
        [SerializeField] private TMPro.TMP_InputField displayNameInputField;
        [SerializeField] private Button displayNameAcceptButton;

        private void Awake()
        {
            customPlayFabSingleton.OnRequestDisplayName += OnRequestDisplayName;
            customPlayFabSingleton.OnClientDataGetResult += OnClientDataGet;
            displayNameAcceptButton.onClick.AddListener(OnAcceptDisplayName);
            displayNameInputField.onSubmit.AddListener(OnSubmitDisplayName);
        }

        private void Start()
        {
            InstantShowCanvasGroup(generalCanvasGroup);
            InstantHideCanvasGroup(displayNameCanvasGroup);
        }

        private void OnDestroy()
        {
            customPlayFabSingleton.OnRequestDisplayName -= OnRequestDisplayName;
            customPlayFabSingleton.OnClientDataGetResult -= OnClientDataGet;
            displayNameAcceptButton.onClick.RemoveListener(OnAcceptDisplayName);
            displayNameInputField.onSubmit.RemoveListener(OnSubmitDisplayName);
        }

        private void OnRequestDisplayName()
        {
            FadeShowCanvasGroup(displayNameCanvasGroup);
        }

        private void OnClientDataGet(bool result)
        {
            FadeHideCanvasGroup(generalCanvasGroup, true);
        }

        private void OnAcceptDisplayName()
        {
            if (displayNameInputField.text.Length >= displayMinLenght)
            {
                customPlayFabSingleton.SetDisplayName(displayNameInputField.text);
                FadeHideCanvasGroup(displayNameCanvasGroup);
            }
        }

        private void OnSubmitDisplayName(string displayName)
        {
            if (displayName.Length >= displayMinLenght)
            {
                customPlayFabSingleton.SetDisplayName(displayName);
                FadeHideCanvasGroup(displayNameCanvasGroup);
            }
        }
        
        #region CANVAS_GROUP_LOGIC

        private void InstantShowCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void InstantHideCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
        private void FadeShowCanvasGroup(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadingShowCanvasGroup(canvasGroup));
        }

        private IEnumerator FadingShowCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            
            float t = 0;
            while (t < 1)
            {
                canvasGroup.alpha = t;
                t += Time.deltaTime;
                yield return null;
            }

            InstantShowCanvasGroup(canvasGroup);
        }
        
        private void FadeHideCanvasGroup(CanvasGroup canvasGroup, bool destroyOnComplete = false)
        {
            StartCoroutine(FadingHideCanvasGroup(canvasGroup, destroyOnComplete));
        }
        
        private IEnumerator FadingHideCanvasGroup(CanvasGroup canvasGroup, bool destroyOnComplete)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            float t = 1;
            while (t > 0)
            {
                canvasGroup.alpha = t;
                t -= Time.deltaTime;
                yield return null;
            }
            
            InstantHideCanvasGroup(canvasGroup);
            
            if(destroyOnComplete)
                Destroy(gameObject);
        }
        

        #endregion
        
    }
}
