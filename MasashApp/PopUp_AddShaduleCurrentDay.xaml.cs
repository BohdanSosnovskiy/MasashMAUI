using MasashApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasashApp;

public partial class PopUp_AddShaduleCurrentDay : ContentPage
{
    public DateTime added_date { get; set; }

    public delegate void CreatedShedule(Model_schedule id_shedule);
    public event CreatedShedule? CreatedSheduleEvent;

    public PopUp_AddShaduleCurrentDay(DateTime date)
	{
		InitializeComponent();
        Loaded += Page_Loaded;
        BindingContext = this;
        added_date = date;
    }

    protected override bool OnBackButtonPressed()
    {
        Close();
        return true;
    }

    void Page_Loaded(object sender, EventArgs e)
    {
        // We only need this to fire once, so clean things up!
        this.Loaded -= Page_Loaded;

        // Call the animation
        PoppingIn();
    }

    public void PoppingIn()
    {
        var contentHeight = 400;

        // Начните с перевода контента под экраном или за его пределами.
        this.Content.TranslationY = contentHeight;

        // Также анимируйте контент, скользящий вверх из-под экрана.
        this.Animate("Content",
            callback: v => this.Content.TranslationY = (int)(contentHeight - v),
            start: 0,
            end: contentHeight,
            length: 500,
            easing: Easing.CubicInOut,
            finished: (v, k) => this.Content.TranslationY = 0);
    }

    public Task PoppingOut()
    {
        var done = new TaskCompletionSource();

        // Measure the content size so we know how much to translate
        var contentSize = this.Content.Measure(Window.Width, Window.Height, MeasureFlags.IncludeMargins);
        var windowHeight = contentSize.Request.Height;


        // Start sliding the content down below the bottom of the screen
        this.Animate("Content",
            callback: v => this.Content.TranslationY = (int)(windowHeight - v),
            start: windowHeight,
            end: 0,
            length: 500,
            easing: Easing.CubicInOut,
            finished: (v, k) =>
            {
                this.Content.TranslationY = windowHeight;
                // Important: Set our completion source to done!
                done.TrySetResult();
            });

        // We return the task so we can wait for the animation to finish
        return done.Task;
    }

    private async void TapGestureRecognizer_OnTapped(object sender, TappedEventArgs e)
    {
        await Close();
    }

    private async void Create_Shadule(object sender, TappedEventArgs e)
    {
        string fromTime = From_TimePicker.Time.ToString(@"hh\:mm");
        string toTime = To_TimePicker.Time.ToString(@"hh\:mm");

        if(CheckTimeValid(fromTime, toTime))
        {
            var result = await StaticData.API.POST("/add_schedule", new Dictionary<string, string>()
            {
                { "masterId", StaticData.User.Master._id },
                { "date", added_date.ToString()},
                { "fromTime", fromTime.ToString() },
                { "toTime", toTime.ToString() }
            });

            Model_schedule model_Schedule = new Model_schedule();
            model_Schedule._id = result;
            model_Schedule.Date = added_date;
            model_Schedule.SetDiaposonTimes(fromTime, toTime, 30);

            CreatedSheduleEvent.Invoke(model_Schedule);
            await Close();
        }
        else
        {
            var alert = new ModalMessage()
            {
                IsButtonVisible = false,
                Text_yes = "Удалить",
                Text_no = "Отмена",
                Title_Message = "Ошибка",
                Message = "Начальное время не должно быть больше конечного"
            };
            await Navigation.PushModalAsync(alert, false);
        }
    }

    public bool CheckTimeValid(string from,  string to)
    {
        var _startTime = from.Split(':');
        int startHours = Convert.ToInt32(_startTime[0]);
        int startMinutes = Convert.ToInt32(_startTime[1]);

        var _endTime = to.Split(':');
        int endHours = Convert.ToInt32(_endTime[0]);
        int endMinutes = Convert.ToInt32(_endTime[1]);

        if(startHours > endHours)
        {
            return false;
        }
        else if(startHours == endHours && startMinutes > endMinutes)
        {
            return false;
        }

        return true;
    }

    async Task Close()
    {
        // Wait for the animation to complete
        await PoppingOut();
        // Navigate away without the default animation
        await Navigation.PopModalAsync(animated: false);
    }
}