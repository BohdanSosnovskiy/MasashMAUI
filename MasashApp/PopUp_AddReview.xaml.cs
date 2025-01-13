using MasashApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasashApp;

public partial class PopUp_AddReview : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public Model_appointment_date appointment_Date { get; set; }
    public PopUp_AddReview(Model_appointment_date appointment)
	{
		InitializeComponent();
        appointment_Date = appointment;
        Loaded += Page_Loaded;
        BindingContext = this;
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

        Stars = [false, false, false, false, false];
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

    async Task Close()
    {
        // Wait for the animation to complete
        await PoppingOut();
        // Navigate away without the default animation
        await Navigation.PopModalAsync(animated: false);
    }

    public bool[] Stars { get; set; }
    public int SelStars { get; set; }

    public void SetStars(int count_star)
    {
        for(int i = 0; i < Stars.Length; i++)
        {
            if(i < count_star)
            {
                Stars[i] = true;
            }
            else
            {
                Stars[i] = false;
            }
            OnPropertyChanged("Stars");
        }
        SelStars = count_star;
    }

    public void Set_1_Star(object sender, TappedEventArgs e)
    {
        SetStars(1);
    }
    public void Set_2_Star(object sender, TappedEventArgs e)
    {
        SetStars(2);
    }
    public void Set_3_Star(object sender, TappedEventArgs e)
    {
        SetStars(3);
    }
    public void Set_4_Star(object sender, TappedEventArgs e)
    {
        SetStars(4);
    }
    public void Set_5_Star(object sender, TappedEventArgs e)
    {
        SetStars(5);
    }

    public async void Add_review_Toch(object sender, TappedEventArgs e)
    {
        string id_review = await StaticData.API.POST("/add_review", new Dictionary<string, string>()
        {
            { "name", StaticData.User.Name},
            { "description",Entry_Description.Text },
            { "masterId", appointment_Date.Master._id},
            { "date", DateTime.Now.ToString() },
            { "star", SelStars.ToString()},
            { "appointmentID", appointment_Date._id}
        });

        Model_review review = new Model_review();
        review._id = id_review;
        review.Name = StaticData.User.Name;
        review.Description = Entry_Description.Text;
        review.Date = DateTime.Now;
        review.AppointmentId = appointment_Date._id;
        review.Star = SelStars;

        appointment_Date.Master.Reviews.Add(review);
        await Close();
    }
}