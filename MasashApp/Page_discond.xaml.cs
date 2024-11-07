namespace MasashApp;

public partial class Page_discond : ContentPage
{
	public Page_discond()
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