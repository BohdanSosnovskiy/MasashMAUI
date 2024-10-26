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