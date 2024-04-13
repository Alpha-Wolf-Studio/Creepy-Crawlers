using UnityEngine;
using UnityEngine.UI;

namespace NotificationSystem
{
    public class Nofitication_Test : MonoBehaviour
    {
        [SerializeField] private Button buttonNotificationDefault;
        [SerializeField] private Button buttonNotificationKill;
        [SerializeField] private Notification notification;

        private void Awake ()
        {
            buttonNotificationDefault.onClick.AddListener(OnDefaultClick);
            buttonNotificationKill.onClick.AddListener(OnKillClick);
        }

        private void OnDestroy()
        {
            buttonNotificationDefault.onClick.RemoveAllListeners();
            buttonNotificationKill.onClick.RemoveAllListeners();
        }

        private void OnDefaultClick ()
        {
            notification.Show("Has Conseguido un logro de valor.", 5, NotificationType.Default);
        }

        private void OnKillClick ()
        {
            notification.Show("ElGranPepo -> Mengueche", 5, NotificationType.Kill);
        }
    }
}