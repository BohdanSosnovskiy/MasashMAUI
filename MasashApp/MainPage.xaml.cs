using CommunityToolkit.Maui.Behaviors;
using MasashApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private Model_Master selectedMaster;
        public Model_Master? SelectedMaster
        {
            get
            {
                return selectedMaster;
            }
            set
            {
                selectedMaster = value;
                if(value != null)
                {
                    IsNullMaster = false;
                    IsSelectedMaster = true;
                }
                else
                {
                    IsNullMaster = true;
                    IsSelectedMaster = false;
                }
                OnPropertyChanged();
            }
        }
        private bool isSelectedMaster;
        public bool IsSelectedMaster
        {
            get
            {
                return isSelectedMaster;
            }
            set
            {
                isSelectedMaster = value;
                OnPropertyChanged();
            }
        }

        private string? pathImg;
        public string PathImg
        {
            get
            {
                if (pathImg == null)
                {
                    ImgUser.Behaviors.Clear();
                    ImgUser.Behaviors.Add(new IconTintColorBehavior { TintColor = Colors.White });
                    return "icon_user.png";
                }

                ImgUser.Behaviors.Clear();
                return pathImg;
            }
            set
            {
                pathImg = value;
                OnPropertyChanged();
            }
        }

        private string? titntColorImg;
        public string TintColorImg
        {
            get
            {
                return titntColorImg;
            }
            set
            {
                titntColorImg = value;
                OnPropertyChanged();
            }
        }

        private bool isNullMaster;
        public bool IsNullMaster
        {
            get
            {
                return isNullMaster;
            }
            set
            {
                isNullMaster = value;
                OnPropertyChanged();
            }
        }

        private bool isSelectedServise;
        public bool IsSelectedServise
        {
            get
            {
                return isSelectedServise;
            }
            set
            {
                isSelectedServise = value;
                OnPropertyChanged();
            }
        }

        private bool isNullServise;
        public bool IsNullServise
        {
            get
            {
                return isNullServise;
            }
            set
            {
                isNullServise = value;
                OnPropertyChanged();
            }
        }

        private bool isViewImageServise;
        public bool IsViewImageServise
        {
            get
            {
                return isViewImageServise;
            }
            set
            {
                isViewImageServise = value;
                OnPropertyChanged();
            }
        }

        private bool isHideDefaultImageServise;
        public bool IsHideDefaultImageServise
        {
            get
            {
                return isHideDefaultImageServise;
            }
            set
            {
                isHideDefaultImageServise = value;
                OnPropertyChanged();
            }
        }

        private DateTime? selectedTime;
        public DateTime? SelectedTime
        {
            get
            {
                return selectedTime;
            }
            set
            {
                selectedTime = value;
                SelectedTimeString = value.ToString();
                OnPropertyChanged();
            }
        }

        private string selectedTimeString;
        public string SelectedTimeString
        {
            get
            {
                return selectedTimeString;
            }
            set
            {
                var data = value.Split(' ');
                string dataDate = data[0];
                string dataTime = data[1];
                var Date = dataDate.Split('.');
                var Time = dataTime.Split(":");

                selectedTimeString = $"{Date[0]} {GetMounth(Date[1])} {Date[2]} в {Time[0]}:{Time[1]}";
                OnPropertyChanged();
            }
        }

        public string GetMounth(string date)
        {
            switch (Convert.ToInt32(date))
            {
                case 1: return "Января";
                case 2: return "Февраля";
                case 3: return "Марта";
                case 4: return "Апреля";
                case 5: return "Майя";
                case 6: return "Июня";
                case 7: return "Июля";
                case 8: return "Августа";
                case 9: return "Сентября";
                case 10: return "Октября";
                case 11: return "Ноября";
                case 12: return "Декабря";
                default: return "";
            }
        }

        private bool isHideTime;
        public bool IsHideTime
        {
            get
            {
                return isHideTime;
            }
            set
            {
                isHideTime = value;
                OnPropertyChanged();
            }
        }

        private bool isVisibleTime;
        public bool IsVisibleTime
        {
            get
            {
                return isVisibleTime;
            }
            set
            {
                isVisibleTime = value;
                OnPropertyChanged();
            }
        }

        private string pathImgService;
        public string PathImgService
        {
            get
            {
                return pathImgService;
            }
            set
            {
                pathImgService = value;
                OnPropertyChanged();
            }
        }

        private double totalMinsServises;
        public string TotalMinsServises
        {
            get
            {
                string result = "";

                var time = TimeSpan.FromMinutes(totalMinsServises);


                double hours = 0;
                double min = 0;

                if(totalMinsServises > 60)
                {
                    hours = time.Hours;
                    min = time.Minutes;

                    switch (hours)
                    {
                        case 1:
                            {
                                result = $"{hours} час {min} минут";
                                break;
                            }
                        case 2:
                            {
                                result = $"{hours} часа {min} минут";
                                break;
                            }
                        case 3:
                            {
                                result = $"{hours} часа {min} минут";
                                break;
                            }
                        case 4:
                            {
                                result = $"{hours} часа {min} минут";
                                break;
                            }
                        default:
                            {
                                result = $"{hours} часов {min} минут";
                                break;
                            }
                    }
                }
                else
                {
                    min = totalMinsServises;
                    result = $"{min} мин";
                }
                return result;
            }
            set
            {
                totalMinsServises = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }
        private double totalPriceServises;
        public double TotalPriceServises
        {
            get
            {
                return totalPriceServises;
            }
            set
            {
                totalPriceServises = value;
                OnPropertyChanged();
            }
        }

        public LoadData loadData { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Address = "просп. Героев Харькова, 272 Д";
            IsNullMaster = true;
            IsSelectedMaster = false;
            IsNullServise = true;
            IsSelectedServise = false;
            Loaded += MainPage_Loaded;
            InitStaticData();
            StaticData.linkMainPage = this;
            Grid_SelectedCategorysItems.BindingContext = this;
            HeightRequestSelectedCategorysItems = 40;
            IsVisibleTime = false;
            IsHideTime = true;
            try
            {
                StaticData.API = new Connection_API();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            loadData = new LoadData();
        }

        private double heightRequest_SelectedCategorysItems;
        public double HeightRequestSelectedCategorysItems
        {
            get
            {
                return heightRequest_SelectedCategorysItems;
            }
            set
            {
                heightRequest_SelectedCategorysItems = value;
                OnPropertyChanged();
            }
        }


        private void MainPage_Loaded(object? sender, EventArgs e)
        {
            UpdateListService();
            UpdateImgUser();
        }

        public void UpdateImgUser()
        {
            if(StaticData.User != null)
            {
                PathImg = StaticData.User.PathImg;
            }
        }

        public void UpdateListService()
        {
            StackLayout_main.BatchBegin();
            ListView_SelectedCategorysItems.BeginRefresh();
            CheckTotalTimeCash();
            CheckViewImgService();
            HeightRequestSelectedCategorysItems = ListView_SelectedCategorysItems.RowHeight * StaticData.SelectedCatergory_Item_Sevice.Count;
            ListView_SelectedCategorysItems.HeightRequest = ListView_SelectedCategorysItems.RowHeight * StaticData.SelectedCatergory_Item_Sevice.Count;
            ListView_SelectedCategorysItems.BindingContext = StaticData.SelectedCatergory_Item_Sevice;
            ListView_SelectedCategorysItems.ItemsSource = StaticData.SelectedCatergory_Item_Sevice;
            ListView_SelectedCategorysItems.EndRefresh();
            StackLayout_main.BatchCommit();
        }

        public Dictionary<string, int> CheckTotalTimeCash()
        {
            int cash = 0;
            int time = 0;

            if (StaticData.SelectedCatergory_Item_Sevice.Count > 0)
            {
                for (int i = 0; i < StaticData.SelectedCatergory_Item_Sevice.Count; i++)
                {
                    cash += StaticData.SelectedCatergory_Item_Sevice[i].Price;
                    time += StaticData.SelectedCatergory_Item_Sevice[i].Time;
                }
                TotalMinsServises = time.ToString();
                TotalPriceServises = cash;
            }

            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("Time", time);
            result.Add("Cash", cash);

            return result;
        }

        public void InitStaticData()
        {
            Model_Master master = new Model_Master();
            master.Name = "Мастер Виктория";
            master.PathImg = "http://192.168.88.200:4362/delkasswfuemmsscktdy.jpeg";

            //Отзывы
            Model_review review = new Model_review()
            {
                Name = "Наталья",
                Description = "Очень хорошая работа",
                Star = 5,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            review = new Model_review()
            {
                Name = "Глеб",
                Description = "Очень дорого",
                Star = 2,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            review = new Model_review()
            {
                Name = "Мария",
                Description = "Не плохо",
                Star = 4,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            //Записи на приём
            Model_appointment_date appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 15),
                Master = master,
                User = new Model_user()
                {
                    Name = "Таня"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 13),
                Master = master,
                User = new Model_user()
                {
                    Name = "Надя"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 21),
                Master = master,
                User = new Model_user()
                {
                    Name = "Кристина"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            //Расписаниие
            Model_schedule schedule = new Model_schedule();

            //21.11.2024
            schedule.Date = new DateTime(2024, 11, 21);
            schedule.SetDiaposonTimes("10:00", "20:00", 30);
            master.Schedules.Add(schedule);

            //22.11.2024
            schedule = new Model_schedule();
            schedule.Date = new DateTime(2024, 11, 22);
            schedule.SetDiaposonTimes("12:00", "15:30", 30);
            master.Schedules.Add(schedule);

            StaticData.Masters.Add(master);

            master = new Model_Master();
            master.Name = "Мастер Марина";

            //Отзывы
            review = new Model_review()
            {
                Name = "Наталья",
                Description = "Очень хорошая работа",
                Star = 4,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            review = new Model_review()
            {
                Name = "Глеб",
                Description = "Очень дорого",
                Star = 3,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            review = new Model_review()
            {
                Name = "Мария",
                Description = "Не плохо",
                Star = 2,
                Date = DateTime.Now,
            };
            master.Reviews.Add(review);

            //Записи на приём
            appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 15),
                Master = master,
                User = new Model_user()
                {
                    Name = "Таня"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 17),
                Master = master,
                User = new Model_user()
                {
                    Name = "Надя"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            appointment_Date = new Model_appointment_date()
            {
                Date = new DateTime(2024, 11, 21),
                Master = master,
                User = new Model_user()
                {
                    Name = "Кристина"
                }
            };
            master.Appointment_dates.Add(appointment_Date);

            StaticData.Masters.Add(master);
        }

        private void Remove_Servise(object sender, ItemTappedEventArgs e)
        {
            Model_service_item item = (Model_service_item)e.Item;

            for (int i = 0; i < StaticData.SelectedCatergory_Item_Sevice.Count; i++)
            {
                if (StaticData.SelectedCatergory_Item_Sevice[i].Name == item.Name)
                {
                    StaticData.SelectedCatergory_Item_Sevice.RemoveAt(i);
                    break;
                }
            }


            CheckViewImgService();
            UpdateListService();
        }

        public void CheckViewImgService()
        {
            if (StaticData.SelectedCatergory_Item_Sevice.Count == 0)
            {
                IsNullServise = true;
                IsSelectedServise = false;
                IsViewImageServise = false;
                IsHideDefaultImageServise = true;
            }
            else if (StaticData.SelectedCatergory_Item_Sevice.Count == 1)
            {
                PathImgService = StaticData.SelectedCatergory_Item_Sevice[0].PathImg;
                IsNullServise = false;
                IsSelectedServise = true;
                IsViewImageServise = true;
                IsHideDefaultImageServise = false;
            }
            else
            {
                IsNullServise = false;
                IsSelectedServise = true;
                IsViewImageServise = false;
                IsHideDefaultImageServise = true;
            }
        }

        private async void OpenSideBar(object sender, SwipedEventArgs e)
        {
            await Navigation.PushModalAsync(new SideBar(), false);
        }

        public async void SelectAuth_Touch(object sender, TappedEventArgs e)
        {
            if(!StaticData.isAuth)
            {
                await Navigation.PushModalAsync(new Page_Auth(), false);
            }
            else
            {
                await Navigation.PushModalAsync(new PageInfoAccaunt(), false);
            }
            
        }

        public async void SelectDateTime_Touch(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Page_selectDateTime(), true);
        }

        public async void SelectMaster_Touch(object sender, TappedEventArgs e)
        {
            Console.WriteLine("Переход на выбор мастера");
            await Navigation.PushModalAsync(new Page_selectMaster(), true);
        }

        public void Clear_selectedMaster(object sender, TappedEventArgs e)
        {
            SelectedMaster = null;
            IsNullMaster = true;
            IsSelectedMaster = false;
        }

        public void Clear_selectedTime(object sender, TappedEventArgs e)
        {
            selectedTime = null;
            IsVisibleTime = false;
            IsHideTime = true;
        }

        public async void SelectService_Touch(object sender, TappedEventArgs e)
        {
            await Navigation.PushModalAsync(new Page_selectService(), true);
        }

        public async void CreateSeans_Touch(object sender, TappedEventArgs e)
        {
            //if(StaticData.API.isConnect)
            //{
            //    await StaticData.API.SendMessageAsync("Привет|Ha");
            //}

            //LoadDataMasters();
            
            await loadData.LoadDataMasters();
        }
    }

}
