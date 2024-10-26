using Android.Widget;
using Microsoft.Maui.Handlers;

namespace MasashApp.Platforms.Android
{
    public class MySliderHandler : SliderHandler
    {
        protected override void ConnectHandler(SeekBar platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SetOnTouchListener(new SliderListener());
        }
    }
}
