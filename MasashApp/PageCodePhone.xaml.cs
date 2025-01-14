using MasashApp.Models;
using System.Text.Json;

namespace MasashApp;

public partial class PageCodePhone : ContentPage
{
	public PageCodePhone()
	{
		InitializeComponent();
        Entry_Code_1.PropertyChanged += Entry_Code_1_PropertyChanged;
        Entry_Code_2.PropertyChanged += Entry_Code_2_PropertyChanged;
        Entry_Code_3.PropertyChanged += Entry_Code_3_PropertyChanged;
        Entry_Code_4.PropertyChanged += Entry_Code_4_PropertyChanged;
	}

    private void Entry_Code_4_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(Entry_Code_4.Text != "" && Entry_Code_4.Text != null)
        {

        }
        else
        {
            Entry_Code_3.Focus();
        }
    }

    private void Entry_Code_3_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (Entry_Code_3.Text != "" && Entry_Code_3.Text != null)
        {
            Entry_Code_4.Focus();
        }
        else
        {
            Entry_Code_2.Focus();
        }
    }

    private void Entry_Code_2_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (Entry_Code_2.Text != "" && Entry_Code_2.Text != null)
        {
            Entry_Code_3.Focus();
        }
        else
        {
            Entry_Code_1.Focus();
        }
    }

    private void Entry_Code_1_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (Entry_Code_1.Text != "" && Entry_Code_1.Text != null)
        {
            Entry_Code_2.Focus();
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

    public async void SendCode_Tap(object sender, TappedEventArgs e)
    {
        string Code = Entry_Code_1.Text + Entry_Code_2.Text + Entry_Code_3.Text + Entry_Code_4.Text;
        string mainDir = FileSystem.Current.AppDataDirectory;
        string path = mainDir + "/GUID.txt";
        if (!File.Exists(path)) 
        {
            StaticData.API.InitGUID();
        }

        string result = await StaticData.API.POST("/auth", new Dictionary<string, string>
        {
            {"id", StaticData.GUID_User},
            {"phone", StaticData.NumberPhone},
            {"date_reg", DateTime.Now.ToString() }
        });



        if (result == "signin")
        {
            StaticData.User = new Model_user()
            {
                Phone = StaticData.NumberPhone
            };
        }
        else
        {
            Dictionary<string, string> parsing = StaticData.API.ParsingData(result);
            

            StaticData.User = new Model_user()
            {
                Name = parsing["name"],
                Email = parsing["email"],
                Date_birthday = StaticData.API.GetDateFromString(parsing["date_birthday"]),
                Date_reg = StaticData.API.GetDateFromString(parsing["date_reg"]),
                IsAdmin = Convert.ToBoolean(parsing["isAdmin"]),
                IsMaster = Convert.ToBoolean(parsing["isMaster"]),
                Phone = parsing["phone"]
            };
            StaticData.isAuth = true;
        }

        await Navigation.PushModalAsync(new PageInfoAccaunt(), false);
    }
}