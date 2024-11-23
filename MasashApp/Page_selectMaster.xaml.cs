using MasashApp.Models;

namespace MasashApp;

public partial class Page_selectMaster : ContentPage
{
	public Page_selectMaster()
	{
		InitializeComponent();
        Loaded += Page_selectMaster_Loaded;
	}

    private void Page_selectMaster_Loaded(object? sender, EventArgs e)
    {
        Loaded -= Page_selectMaster_Loaded;
        Carusel_Masters.BindingContext = StaticData.Masters;
        Carusel_Masters.ItemsSource = StaticData.Masters;
    }

    

    public void Back_Touch(object sender, TappedEventArgs e)
    {
        Close();
    }

    protected override bool OnBackButtonPressed()
    {
        Close();
        return true;
    }

    public async void Close()
    {
        await Navigation.PopModalAsync(animated: true);
    }

    private void Carusel_Masters_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Model_Master master = (Model_Master)e.Item;
        StaticData.linkMainPage.SelectedMaster = master;
        Close();
    }
}