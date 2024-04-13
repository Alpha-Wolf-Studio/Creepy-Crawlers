using System.Collections;
using UnityEngine;
using TMPro;

namespace NotificationSystem
{
    public class NotificationComponent : MonoBehaviour
    {
        [SerializeField] protected TMP_Text text;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Transform visual;
        private NotificationSetting cfg;

        public bool InUse { get; protected set; }

        protected IEnumerator showing;

        protected void Awake ()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Set (string newText, NotificationSetting notificationSetting)
        {
            cfg = notificationSetting;

            StopNotification();
            text.text = newText;

            showing = Showing();
            StartCoroutine(showing);
        }

        public void StopNotification ()
        {
            InUse = false;
            if (showing != null)
            {
                StopCoroutine(showing);
                showing = null;
            }

            HideCompletely();
        }

        protected IEnumerator Showing ()
        {
            InUse = true;
            float currentTime = 0;

            while (currentTime < cfg.spawningTime)
            {
                currentTime += Time.deltaTime;
                Spawning(currentTime, cfg.animationSpawn);

                yield return null;
            }

            ShowCompletely();
            currentTime = 0;

            yield return new WaitForSeconds(cfg.duration);

            while (currentTime < cfg.despawningTime)
            {
                currentTime += Time.deltaTime;
                Despawning(currentTime, cfg.animationDespawn);

                yield return null;
            }

            StopNotification();
        }

        protected void Spawning (float currentTime, AnimationCurve animationCurve)
        {
            float lerp = currentTime / cfg.spawningTime;
            canvasGroup.alpha = lerp;

            Vector3 pos = Vector3.Lerp(cfg.hidePosition, cfg.showPosition, animationCurve.Evaluate(lerp));
            visual.localPosition = pos;
        }

        protected void Despawning (float currentTime, AnimationCurve animationCurve)
        {
            float lerp = currentTime / cfg.despawningTime;
            canvasGroup.alpha = 1 - lerp;

            Vector3 pos = Vector3.Lerp(cfg.showPosition, cfg.hidePosition, animationCurve.Evaluate(lerp));
            visual.localPosition = pos;
        }

        private void ShowCompletely ()
        {
            Spawning(cfg.despawningTime, cfg.animationDespawn);
        }

        private void HideCompletely ()
        {
            Despawning(cfg.despawningTime, cfg.animationDespawn);
        }
    }
}