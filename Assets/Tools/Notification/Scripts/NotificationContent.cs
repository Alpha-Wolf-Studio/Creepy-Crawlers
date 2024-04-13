using System.Collections.Generic;
using UnityEngine;

namespace NotificationSystem
{
    public class NotificationContent : MonoBehaviour
    {
        [SerializeField] protected NotificationType notificationType;
        [SerializeField] protected List<NotificationComponent> notifications = new List<NotificationComponent>();
        [SerializeField] protected NotificationComponent prefabNotificationComponent;
        [SerializeField] private NotificationSetting notificationSetting;

        public NotificationType NotificationType => notificationType;

        public virtual void ShowNotification (string newText, float duration)
        {
            NotificationComponent notification = GetFreeNotification(out int index);
            if (notification == null)
            {
                notification = Instantiate(prefabNotificationComponent, transform);
                notifications.Add(notification);
                notification = GetFreeNotification(out index);
            }

            notification.transform.SetSiblingIndex(index);
            notification.Set(newText, notificationSetting);
        }

        protected NotificationComponent GetFreeNotification (out int index)
        {
            index = 0;

            if (notifications.Count < 1)
                return null;

            NotificationComponent notification = null;
            for (int i = 0; i < notifications.Count; i++)
            {
                if (!notifications[i].InUse)
                {
                    index = i;
                    notification = notifications[i];

                    break;
                }
            }

            return notification;
        }
    }
}