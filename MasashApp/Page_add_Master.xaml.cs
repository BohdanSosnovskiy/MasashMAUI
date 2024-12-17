using MasashApp.Models;

namespace MasashApp;

public partial class Page_add_Master : ContentPage
{
	public Page_add_Master()
	{
		InitializeComponent();
        Loaded += Page_Loaded;

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

    private async void Add_Master(object sender, TappedEventArgs e)
    {
        try
        {
            var phone_res = await StaticData.API.POST("/getUserInfoByPhone", new Dictionary<string, string>()
            {
                { "phone", Entry_Phone.Text },
            });

            if (phone_res != null && phone_res != "" && phone_res != "error")
            {
                string rand_name = StaticData.GetRandomName();
                string type = selectedImg.FileName.Split('.')[1];
                string path = rand_name + "." + type;
                await StaticData.API.SendMessageAsync($"StartSendFile|{path}");
                await StaticData.API.SendFileAsync(selectedImg);
                await StaticData.API.SendMessageAsync("EndSendFile");
                await StaticData.API.POST("/add_master", new Dictionary<string, string>()
                {
                    { "name",Entry_Name.Text },
                    { "pathImg", $"{StaticData.API.GetPathHttp("")}/{path}" },
                    { "phone", Entry_Phone.Text },
                });

                await StaticData.API.POST("/set_user_master", new Dictionary<string, string>()
                {
                    { "pathImg", $"{StaticData.API.GetPathHttp("")}/{path}" },
                    { "phone", Entry_Phone.Text },
                });

                Model_Master master = new Model_Master();
                master.Name = Entry_Name.Text;
                master.PathImg = $"{StaticData.API.GetPathHttp("")}/{path}";

                StaticData.Masters.Add(master);

                await Close();
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
            
        }
        catch(Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
        
    }

    private async void EndSendFile(object? state)
    {
        await StaticData.API.SendMessageAsync("EndSendFile");
    }

    async Task Close()
    {
        // Wait for the animation to complete
        await PoppingOut();
        // Navigate away without the default animation
        await Navigation.PopModalAsync(animated: false);
    }

    public FileResult? selectedImg { get; set; }
    public async void Select_Img(object sender, TappedEventArgs e)
    {
        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
        };

        selectedImg = await StaticData.PickAndShow(options, Img_master);
    }
}