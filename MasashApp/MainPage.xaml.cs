using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MasashApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string address;
        public string Address 
        {
            get { return address; }
            set { address = value; OnPropertyChanged(); } 
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Address = "просп. Героев Харькова, 272 Д";
        }

        public async void SelectAuth_Touch(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Page_Auth(), false);
        }

        public async void SelectDateTime_Touch(object sender, TappedEventArgs e)
        {
            
        }

        public async void SelectMaster_Touch(object sender, TappedEventArgs e)
        {

        }

        public async void SelectService_Touch(object sender, TappedEventArgs e)
        {

        }
    }

}
