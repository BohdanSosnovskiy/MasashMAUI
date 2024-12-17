using MasashApp.Models;

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

    public async void History_Visit_Toch(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_historyinfo(), false);
    }

    public async void Notification_Toch(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_notifications(), false);
    }

    public async void Descond_Toch(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_discond(), false);
    }

    public async void Settings_Toch(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Page_settings(), false);
    }

    public void Logout_Toch(object sender, TappedEventArgs e)
    {
        string mainDir = FileSystem.Current.AppDataDirectory;
        string path = mainDir + "/GUID.txt";
        File.Delete(path);
        StaticData.isAuth = false;
        StaticData.User = null;
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}