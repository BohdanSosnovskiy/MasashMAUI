namespace MasashApp.Notifications
{
    public interface INotificationManagerService
    {
        public event EventHandler NotificationReceived;
        public void SendNotification(string title, string message, DateTime? notifyTime = null);
        public void ReceiveNotification(string title, string message);
    }
}
