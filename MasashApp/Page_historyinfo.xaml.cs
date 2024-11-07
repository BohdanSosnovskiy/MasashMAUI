namespace MasashApp;

public partial class Page_historyinfo : ContentPage
{
	public Page_historyinfo()
	{
		InitializeComponent();
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
        await Navigation.PopModalAsync(animated: false);
    }
}