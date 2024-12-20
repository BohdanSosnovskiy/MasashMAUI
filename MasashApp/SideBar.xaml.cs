namespace MasashApp;

public partial class SideBar : ContentPage
{
	public SideBar()
	{
		InitializeComponent();
        Loaded += Page_Loaded;

    }

    void Page_Loaded(object sender, EventArgs e)
    {
        Loaded -= Page_Loaded;
        PoppingIn();
    }

    protected override bool OnBackButtonPressed()
    {
        Close();
        return true;
    }


    public void PoppingIn()
    {
        double Anim_start = -600;
        // Начните с перевода контента под экраном или за его пределами.
        this.Content.TranslationX = Anim_start;


        // Также анимируйте контент, скользящий вверх из-под экрана.
        this.Animate("Content",
            callback: v => this.Content.TranslationX = (int)(Anim_start + v),
            start: 0,
            end: 600,
            length: 500,
            easing: Easing.CubicInOut,
            finished: (v, k) => this.Content.TranslationX = 0);
    }

    public Task PoppingOut()
    {
        var done = new TaskCompletionSource();

        var windowWidth = 0;

        // Start sliding the content down below the bottom of the screen
        this.Animate("Content",
            callback: v => this.Content.TranslationX = (int)(windowWidth - v),
            start: 0,
            end: 600,
            length: 500,
            easing: Easing.CubicInOut,
            finished: (v, k) =>
            {
                this.Content.TranslationX = -600;
                // Important: Set our completion source to done!
                done.TrySetResult();
            });

        // We return the task so we can wait for the animation to finish
        return done.Task;
    }

    async Task Close()
    {
        // Wait for the animation to complete
        await PoppingOut();
        // Navigate away without the default animation
        await Navigation.PopModalAsync(animated: false);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Close();
    }

    private async void Selcet_MagerMaster(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_ManageMasters(), false);
    }

    private async void Selcet_AddService(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_add_servicess(), false);
    }

    private async void Selecet_Schedule(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ShaduleManager(), false);
    }

    private async void SwipeLeft(object sender, SwipedEventArgs e)
    {
        await Close();
    }
}