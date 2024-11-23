using Foundation;
using UIKit;

namespace MasashApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (hander, view) =>
            {
#if IOS
                hander.PlatformView.BorderStyle = UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("MyCustomization", (hander, view) =>
            {
#if IOS
                hander.PlatformView.BorderStyle = UITextBorderStyle.None;
#endif
            });

            return MauiProgram.CreateMauiApp();
        }
    }
}
