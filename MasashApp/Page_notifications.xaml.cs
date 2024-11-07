namespace MasashApp;

public partial class Page_notifications : ContentPage
{
	public Page_notifications()
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