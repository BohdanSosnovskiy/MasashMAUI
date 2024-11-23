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
        public static MainPage linkMainPage {  get; set; }

        public static ObservableCollection<Model_service_catogory> Catergory_Sevice = new ObservableCollection<Model_service_catogory>();
        public static ObservableCollection<Model_service_item> SelectedCatergory_Item_Sevice = new ObservableCollection<Model_service_item>();
    }
}
