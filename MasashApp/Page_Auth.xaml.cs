using System.Windows.Input;

namespace MasashApp;

public partial class Page_Auth : ContentPage
{
	public Page_Auth()
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

    private async void CheckCode_Tap(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new PageCodePhone(), false);
    }
}