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
