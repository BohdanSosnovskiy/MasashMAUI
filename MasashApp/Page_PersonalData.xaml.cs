using MasashApp.Models;

namespace MasashApp;

public partial class Page_PersonalData : ContentPage
{
	public Page_PersonalData()
	{
		InitializeComponent();
        Loaded += Page_PersonalData_Loaded;
	}

    private void Page_PersonalData_Loaded(object? sender, EventArgs e)
    {
        if(StaticData.isAuth)
        {
            Entry_Phone.Text = StaticData.User.Phone;
            Entry_Email.Text = StaticData.User.Email;
            Entry_Name.Text = StaticData.User.Name;
            Date_Birthday.Date = StaticData.User.Date_birthday;
        }
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