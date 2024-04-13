using System.Collections.Generic;
using UnityEngine;
using NotificationSystem;

public class Notification : MonoBehaviour
{
    [SerializeField] private List<NotificationContent> notificationsContents = new List<NotificationContent>();

    public void Show (string newText, float duration, NotificationType notificationType = NotificationType.Default)
    {
        NotificationContent notificationContent = GetContent(notificationType);
        notificationContent.ShowNotification(newText, duration);
    }

    private NotificationContent GetContent (NotificationType notificationType) => notificationsContents.Find(available => available.NotificationType == notificationType);
}