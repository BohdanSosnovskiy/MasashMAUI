using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public static class StaticData
    {
        public static ObservableCollection<Model_Master> Masters = new ObservableCollection<Model_Master>();
        public static MainPage linkMainPage { get; set; }
        public static Connection_API API { get; set; }
        public static string? GUID_User { get; set; }
        public static string? NumberPhone { get; set; }
        public static bool isAuth { get; set; }
        public static Model_user? User { get; set; }
        public static Model_Master? SelectedEditMaster { get; set; }
        public static Model_service_catogory? selectedCategoryService { get; set; }

        public static ObservableCollection<Model_service_catogory> Catergory_Sevice = new ObservableCollection<Model_service_catogory>();
        public static ObservableCollection<Model_service_item> SelectedCatergory_Item_Sevice = new ObservableCollection<Model_service_item>();

        public static string GetRandomName()
        {
            string result = "";
            string alf = "qazwsxedcrfvtgbyhnujmikolp";
            for(int i = 0; i < 20; i++)
            {
                result += alf[new Random().Next(0,alf.Length-1)];
            }
            return result;
        }

        public static async Task<FileResult> PickAndShow(PickOptions options, Image GUI_Image)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var byteArray = File.ReadAllBytes(result.FullPath);

                        var image = ImageSource.FromStream(() => new MemoryStream(byteArray));

                        GUI_Image.Source = image;
                        var res = await GUI_Image.Source.GetPlatformImageAsync(linkMainPage.Handler.MauiContext);
                        if (res == null)
                        {
                            GUI_Image.Source = "close.png";
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                GUI_Image.Source = "close.png";
            }

            return null;
        }

        public static string GetTime(DateTime date)
        {
            string result = "";
            int Hours = date.Hour;
            int Minute = date.Minute;

            if (Hours < 10)
            {
                result += $"0{Hours}";
            }
            else
            {
                result += $"{Hours}";
            }

            result += ":";

            if (Minute < 10)
            {
                result += $"0{Minute}";
            }
            else
            {
                result += $"{Minute}";
            }
            return result;
        }
    }
}
