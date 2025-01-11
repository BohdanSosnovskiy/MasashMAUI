using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_appointment_date : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string _id { get; set; }

        private DateTime date;

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        private Model_user user;

        public Model_user User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        private Model_Master master;
        public Model_Master Master
        {
            get
            {
                return master;
            }
            set
            {
                master = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Model_service_item> Catergory_Item_Sevice = new ObservableCollection<Model_service_item>();

        public int GetTotalTime()
        {
            int total = 0;

            for(int i = 0; i < Catergory_Item_Sevice.Count; i++)
            {
                total += Catergory_Item_Sevice[i].Time;
            }

            return total;
        }
    }
}
