using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_service_item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private int price;
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        private int time;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        private bool isDecription;
        public bool IsDecription
        {
            get
            {
                return isDecription;
            }
            set
            {
                isDecription = value;
                OnPropertyChanged();
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged();
                LinkModelCategory.CountSelected = LinkModelCategory.CheckCountSelected();
            }
        }

        private string decription;
        public string Decription
        {
            get
            {
                return decription;
            }
            set
            {
                decription = value;
                if( decription.Trim() != "")
                {
                    IsDecription = true;
                }
                else
                {
                    IsDecription = false;
                }
                OnPropertyChanged();
            }
        }

        private Model_service_catogory linkModelCategory;
        public Model_service_catogory LinkModelCategory
        {
            get
            {
                return linkModelCategory;
            }
            set
            {
                linkModelCategory = value;
                OnPropertyChanged();
            }
        }
    }
}
