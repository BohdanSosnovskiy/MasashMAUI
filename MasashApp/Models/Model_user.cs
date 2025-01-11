using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_user : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Dictionary<string, string> ToPOST()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result["id"] = StaticData.GUID_User;
            result["phone"] = Phone;
            result["date_reg"] = Date_reg.ToString();
            result["date_birthday"] = Date_birthday.ToString();
            result["email"] = Email;
            result["isAdmin"] = isAdmin.ToString();
            result["isMaster"] = isMaster.ToString();

            return result;
        }

        public string _id { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
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

        private string pathImg;
        public string PathImg
        {
            get
            {
                return pathImg;
            }
            set
            {
                pathImg = value;
                OnPropertyChanged();
            }
        }

        private string phone;
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }

            set 
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }

            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        private bool isMaster;
        public bool IsMaster
        {
            get
            {
                return isMaster;
            }

            set
            {
                isMaster = value;
                OnPropertyChanged();
            }
        }

        private DateTime date_birthday;

        public DateTime Date_birthday
        {
            get
            {
                return date_birthday;
            }
            set
            {
                date_birthday = value;
                OnPropertyChanged();
            }
        }

        private DateTime date_reg;

        public DateTime Date_reg
        {
            get
            {
                return date_reg;
            }
            set
            {
                date_reg = value;
                OnPropertyChanged();
            }
        }
    }
}
