using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MasashApp;

public partial class ModalMessage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public delegate void ResponceUser(string message);
    public event ResponceUser? ResponceUserEvent;

    public ModalMessage()
	{
		InitializeComponent();
        Loaded += Page_Loaded;
        BindingContext = this;
    }

    private string title_Message;
    public string Title_Message
    {
        get { return title_Message; }
        set { title_Message = value; OnPropertyChanged(); }
    }

    private string message;
    public string Message
    {
        get { return message; }
        set { message = value; OnPropertyChanged(); }
    }

    private string text_yes;
    public string Text_yes
    {
        get { return text_yes; }
        set { text_yes = value; OnPropertyChanged(); }
    }

    private string text_no;
    public string Text_no
    {
        get { return text_no; }
        set { text_no = value; OnPropertyChanged(); }
    }

    private bool isButtonVisible;
    public bool IsButtonVisible
    {
        get { return isButtonVisible; }
        set { isButtonVisible = value; OnPropertyChanged(); }
    }

    private async void Choice_Yes(object sender, TappedEventArgs e)
    {
        if(ResponceUserEvent != null)
            ResponceUserEvent.Invoke("Yes");
        await Close();
    }

    private async void Choice_No(object sender, TappedEventArgs e)
    {
        if (ResponceUserEvent != null)
            ResponceUserEvent.Invoke("No");
        await Close();
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

    async Task Close()
    {
        // Wait for the animation to complete
        await PoppingOut();
        // Navigate away without the default animation
        await Navigation.PopModalAsync(animated: false);
    }
}