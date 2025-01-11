
using MasashApp.Notifications;
using UserNotifications;

namespace MasashApp.Platforms.iOS
{
    public class NotificationReceiver : UNUserNotificationCenterDelegate
    {
        // Вызывается, если приложение находится на переднем плане.
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Console.WriteLine($"WillPresentNotification: вызов ProcessNotification");
            ProcessNotification(notification);

            var presentationOptions = (OperatingSystem.IsIOSVersionAtLeast(14))
                ? UNNotificationPresentationOptions.Banner
                : UNNotificationPresentationOptions.Alert;

            completionHandler(presentationOptions);
        }

        // Вызывается, если приложение находится в фоновом режиме или в завершенном состоянии.
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            Console.WriteLine($"WillPresentNotification");
            if (response.IsDefaultAction)
                ProcessNotification(response.Notification);

            completionHandler();
        }

        void ProcessNotification(UNNotification notification)
        {
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;

            var service = IPlatformApplication.Current?.Services.GetService<INotificationManagerService>();
            if (service != null) 
            {
                service?.ReceiveNotification(title, message);
                Console.WriteLine("ProcessNotification: вызов ReceiveNotification");
            }
            else
            {
                Console.WriteLine("ProcessNotification: service пустой");
            }
        }
    }
}
