namespace MasashApp;

public partial class PageInfoAccaunt : ContentPage
{
	public PageInfoAccaunt()
	{
		InitializeComponent();
	}
    public void Back_Touch(object sender, TappedEventArgs e)
    {
        Close();
    }

    public async void personal_data_toch(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_PersonalData(), false);
    }

    public void History_Visit_Toch(object sender, TappedEventArgs e)
    {

    }

    public void Notification_Toch(object sender, TappedEventArgs e)
    {

    }

    public void Descond_Toch(object sender, TappedEventArgs e)
    {

    }

    public void Settings_Toch(object sender, TappedEventArgs e)
    {

    }

    public void Logout_Toch(object sender, TappedEventArgs e)
    {

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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}