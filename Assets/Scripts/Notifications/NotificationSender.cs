using UnityEngine;

public class NotificationSender : MonoBehaviour
{
    public void SendNotification()
    {
        NotificationManager.SendNotification("TEST");
    }
}
