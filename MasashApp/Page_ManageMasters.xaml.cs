using MasashApp.Models;

namespace MasashApp;

public partial class Page_ManageMasters : ContentPage
{
	public Page_ManageMasters()
	{
		InitializeComponent();
        Loaded += Page_ManageMasters_Loaded;
	}

    private void Page_ManageMasters_Loaded(object? sender, EventArgs e)
    {
        List_Masters.BindingContext = StaticData.Masters;
        List_Masters.ItemsSource = StaticData.Masters;
    }

    public void Back_Touch(object sender, TappedEventArgs e)
    {
        Close();
    }

    public async void Add_Master(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_add_Master(), false);
    }

    protected override bool OnBackButtonPressed()
    {
        Close();
        return true;
    }

    public async void Close()
    {
        await Navigation.PopModalAsync(animated: false);
    }
}