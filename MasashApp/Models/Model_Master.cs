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
    public class Model_Master : INotifyPropertyChanged
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

        private ObservableCollection<bool> stars;
        public ObservableCollection<bool> Stars
        {
            get
            {
                return stars;
            }
            set
            {
                stars = value;
                OnPropertyChanged();
            }
        }

        public void Stars_OnChanged()
        {
            Stars.CollectionChanged += Stars_CollectionChanged;
        }

        private void Stars_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Stars.CollectionChanged -= Stars_CollectionChanged;
            OnPropertyChanged("Stars");
        }


    }
}
