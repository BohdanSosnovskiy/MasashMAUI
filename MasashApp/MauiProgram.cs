using CommunityToolkit.Maui;
using MasashApp.Notifications;
using Microsoft.Extensions.Logging;

namespace MasashApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    handlers.AddHandler(typeof(Slider), typeof(Platforms.Android.MySliderHandler));
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

#if ANDROID
            builder.Services.AddTransient<INotificationManagerService, MasashApp.Platforms.Android.NotificationManagerService>();
#elif IOS
            builder.Services.AddTransient<INotificationManagerService, MasashApp.Platforms.iOS.NotificationManagerService>();
#endif

            return builder.Build();
        }
    }
}
