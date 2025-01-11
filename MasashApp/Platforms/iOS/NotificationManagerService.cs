using Foundation;
using MasashApp.Notifications;
using UserNotifications;

namespace MasashApp.Platforms.iOS
{
    public class NotificationManagerService : INotificationManagerService
    {
        int messageId = 0;
        bool hasNotificationsPermission;

        public event EventHandler? NotificationReceived;

        public NotificationManagerService()
        {
            Console.WriteLine($"Инициализация NotificationManagerService");
            // Создайте UNUserNotificationCenterDelegate для обработки входящих сообщений.
            UNUserNotificationCenter.Current.Delegate = new NotificationReceiver();
            Console.WriteLine($"запрос на уведомления");
            //Запросите разрешение на использование локальных уведомлений.
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                hasNotificationsPermission = approved;
            });
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            Console.WriteLine($"SendNotification: Проверяем есть ли разрешение ({hasNotificationsPermission})");
            // У приложения нет разрешений.
            if (!hasNotificationsPermission)
                return;
            Console.WriteLine($"SendNotification: разрешение есть");
            messageId++;
            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1
            };
            Console.WriteLine($"SendNotification: создан контент");
            UNNotificationTrigger trigger;
            if (notifyTime != null)
            {
                // Создайте триггер на основе календаря.
                trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponents(notifyTime.Value), false);
                Console.WriteLine($"SendNotification: создан тригер на основе календаря");
            }    
            else
            {
                // Создайте триггер на основе времени, интервал указывается в секундах и должен быть больше 0.
                trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);
                Console.WriteLine($"SendNotification: создан тригер на 0.25 секунды");
            }
            Console.WriteLine($"SendNotification: вызов Current.AddNotificationRequest");
            var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                    throw new Exception($"Failed to schedule notification: {err}");
            });
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message
            };
            Console.WriteLine($"ReceiveNotification: {title} \n {message}");
            NotificationReceived?.Invoke(null, args);
            Console.WriteLine($"ReceiveNotification: вызван Invoke");
        }

        NSDateComponents GetNSDateComponents(DateTime dateTime)
        {
            return new NSDateComponents
            {
                Month = dateTime.Month,
                Day = dateTime.Day,
                Year = dateTime.Year,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }
    }
}
