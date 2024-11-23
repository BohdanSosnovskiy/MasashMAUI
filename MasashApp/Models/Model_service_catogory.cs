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
    public class Model_service_catogory : INotifyPropertyChanged
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

        private bool isViewSelected;
        public bool IsViewSelected
        {
            get
            {
                return isViewSelected;
            }
            set
            {
                isViewSelected = value;
                OnPropertyChanged();
            }
        }

        private int countSelected;
        public int CountSelected
        {
            get
            {
                return countSelected;
            }
            set
            {
                countSelected = value;
                OnPropertyChanged();
            }
        }

        public int CheckCountSelected()
        {
            int count = 0;
            for(int i = 0; i < Items.Count; i++)
            {
                if (Items[i].IsSelected)
                {
                    count++;
                }
            }
            return count;
        }

        private ObservableCollection<Model_service_item> items;

        public ObservableCollection<Model_service_item> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        public Model_service_catogory()
        {
            Items = new ObservableCollection<Model_service_item>();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Items");
        }
    }
}
