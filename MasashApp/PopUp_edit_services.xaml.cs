using MasashApp.Models;

namespace MasashApp;

public partial class PopUp_edit_services : ContentPage
{
    public Model_service_item selectedService {  get; set; }
	public PopUp_edit_services(Model_service_item item_service)
	{
		InitializeComponent();
        Loaded += Page_Loaded;
        selectedService = item_service;
        isChangeImg = false;
        BindingContext = selectedService;
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

    public FileResult? selectedImg { get; set; }
    public async void Select_Img(object sender, TappedEventArgs e)
    {
        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
        };

        selectedImg = await StaticData.PickAndShow(options, Img_service);
    }
    public bool isChangeImg { get; set; }
    public async void Edit_service(object sender, TappedEventArgs e)
    {
        if(isChangeImg)
        {
            string rand_name = StaticData.GetRandomName();
            string type = selectedImg.FileName.Split('.')[1];
            string path = rand_name + "." + type;
            await StaticData.API.SendMessageAsync($"StartSendFile|{path}");
            await StaticData.API.SendFileAsync(selectedImg);
            await StaticData.API.SendMessageAsync("EndSendFile");
            selectedService.PathImg = path;
        }
        
        string id_service = await StaticData.API.POST("/edit_service", new Dictionary<string, string>()
        {
            { "id", selectedService._id },
            { "isChangeImg", isChangeImg.ToString().ToLower() },
            { "name",Entry_Name.Text },
            { "price", Entry_Price.Text },
            { "time",  Entry_Time.Text },
            { "pathImg", selectedService.PathImg },
            { "idMaster", StaticData.User.Master._id },
            { "idCategory", StaticData.selectedCategoryService._id}
        });

        selectedService.Name = Entry_Name.Text;
        selectedService.Price = Convert.ToInt32(Entry_Price.Text);
        selectedService.Time = Convert.ToInt32(Entry_Time.Text);

        await Close();
    }
}