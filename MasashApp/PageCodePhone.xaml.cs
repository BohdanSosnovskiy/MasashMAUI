namespace MasashApp;

public partial class PageCodePhone : ContentPage
{
	public PageCodePhone()
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

    public async void SendCode_Tap(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new PageInfoAccaunt(), false);
    }
}