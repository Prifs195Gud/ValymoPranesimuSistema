using Unity.Notifications.Android;

public static class NotificationManager
{
    readonly static string notificationChannelID = "channel_id";

    static NotificationManager()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Id = notificationChannelID,
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        SendNotification();
    }

    public static void SendNotification()
    {
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Your Title",
            Text = "Your Text",
            FireTime = System.DateTime.Now
        };

        AndroidNotificationCenter.SendNotification(notification, notificationChannelID);
    }
}
