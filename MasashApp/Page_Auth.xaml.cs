using MasashApp.Models;
using System.Diagnostics.Metrics;
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
        StaticData.NumberPhone = Entry_NumberPhone.Text;
        var phone_res = await StaticData.API.POST("/getUserInfoByPhone", new Dictionary<string, string>()
        {
            { "phone", Entry_NumberPhone.Text },
        });

        if (phone_res != "")
        {
            var parsing_phone_res = StaticData.API.ParsingListData(phone_res);
            for (int j = 0; j < parsing_phone_res.Count; j++)
            {
                var item_user = parsing_phone_res[j];
                StaticData.GUID_User = item_user["id"];
                string mainDir = FileSystem.Current.AppDataDirectory;
                string path = mainDir + "/GUID.txt";
                await StaticData.API.WriteTextFile(path, StaticData.GUID_User);

                StaticData.User = new Model_user()
                {
                    Name = item_user["name"],
                    Email = item_user["email"],
                    PathImg = item_user["pathImg"],
                    Date_birthday = StaticData.API.GetDateFromString(item_user["date_birthday"]),
                    Date_reg = StaticData.API.GetDateFromString(item_user["date_reg"]),
                    IsAdmin = Convert.ToBoolean(item_user["isAdmin"]),
                    IsMaster = Convert.ToBoolean(item_user["isMaster"]),
                    Phone = item_user["phone"]
                };
                StaticData.isAuth = true;
            }
            await Navigation.PushModalAsync(new PageInfoAccaunt(), false);
        }
        else
        {
            await Navigation.PushModalAsync(new PageCodePhone(), false);
        }
    }
}