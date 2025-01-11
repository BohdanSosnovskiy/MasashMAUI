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

        public string _id {  get; set; }

        private string name;
        /// <summary>
        /// Имя мастера
        /// </summary>
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
        /// <summary>
        /// Номер телефона мастера
        /// </summary>
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

        private string pathImg;
        /// <summary>
        /// Путь к его фотографии
        /// </summary>
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

        private ObservableCollection<bool> stars;
        /// <summary>
        /// Список для отображения звезд в интерфейсе
        /// </summary>
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

        private double stars_double;
        /// <summary>
        /// Числовое значение звезд у мастера
        /// </summary>
        public double Stars_double
        {
            get
            {
                return stars_double;
            }
            set
            {
                stars_double = value;
                OnPropertyChanged();
            }
        }

        private double countReviews;
        /// <summary>
        /// Количество отзывов
        /// </summary>
        public double CountReviews
        {
            get
            {
                return countReviews;
            }
            set
            {
                countReviews = value;
                OnPropertyChanged();
            }
        }

        public void Stars_OnChanged()
        {
            Stars.CollectionChanged += Stars_CollectionChanged;
        }

        private void Stars_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Stars");
        }

        private ObservableCollection<Model_review> reviews;
        /// <summary>
        /// Список отзывов на мастера
        /// </summary>
        public ObservableCollection<Model_review> Reviews
        {
            get
            {
                return reviews;
            }
            set
            {
                reviews = value;
                OnPropertyChanged();
                ReChangeStars();
            }
        }

        public void Reviews_OnChanged()
        {
            Reviews.CollectionChanged += Reviews_CollectionChanged;
        }

        private void Reviews_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ReChangeStars();
            CountReviews = Reviews.Count;
        }
        /// <summary>
        /// Подсчет среднего значения звезд из представленых отзывов
        /// </summary>
        public void ReChangeStars()
        {
            double star = 0;
            for (int i = 0; i < reviews.Count; i++)
            {
                star += reviews[i].Star;
            }
            if(reviews.Count > 0)
            {
                stars_double = Math.Round(star / reviews.Count, 1);
                star = Math.Round(star / reviews.Count);
                if (star > 5)
                {
                    star = 5;
                }

                bool[] _stars = [false, false, false, false, false];
                for (int i = 0; i < star; i++)
                {
                    _stars[i] = true;
                }

                Stars = new ObservableCollection<bool>(_stars);
            }
        }

        private ObservableCollection<Model_appointment_date> appointment_dates;
        /// <summary>
        /// Список записей на прием
        /// </summary>
        public ObservableCollection<Model_appointment_date> Appointment_dates
        {
            get
            {
                return appointment_dates;
            }
            set
            {
                appointment_dates = value;
                OnPropertyChanged();
                GetNextAppointment();
            }
        }

        private ObservableCollection<Model_schedule> schedules;
        /// <summary>
        /// Расписание для записи на прием
        /// </summary>
        public ObservableCollection<Model_schedule> Schedules
        {
            get
            {
                return schedules;
            }
            set
            {
                schedules = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Проверка есть ли на эту дату расписание
        /// </summary>
        /// <param name="date">Дата которую нужно проверить</param>
        /// <returns>Возвращает расписание</returns>
        public Model_schedule? IsFoundDateInSchedules(DateTime date)
        {
            for (int i = 0; i < schedules.Count; i++) 
            {
                if(date.Year == schedules[i].Date.Year && date.Month == schedules[i].Date.Month && date.Day == schedules[i].Date.Day)
                {
                    return schedules[i];
                }
            }
            return null;
        }

        public void Appointment_dates_OnChanged()
        {
            Appointment_dates.CollectionChanged += Appointment_dates_CollectionChanged;
        }

        private void Appointment_dates_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GetNextAppointment();
        }
        /// <summary>
        /// Получение ближайшей следующей даты записи
        /// </summary>
        /// <returns></returns>
        public DateTime GetNextAppointment()
        {
            List<DateTime> j = new List<DateTime>();

            for (int i = 0; i < Appointment_dates.Count; i++)
            {
                j.Add(Appointment_dates[i].Date);
            }

            var nowDate = DateTime.Now;
            DateTime firstdate = j.OrderBy(date => Math.Abs((nowDate - date).TotalSeconds)).FirstOrDefault();

            BligDay = firstdate.Day.ToString();
            BligMounth = firstdate.Month.ToString();
            
            return firstdate;
        }

        private string blig_day;
        public string BligDay
        {
            get
            {
                return blig_day;
            }
            set
            {
                blig_day = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Model_service_catogory> catergory_Sevice;
        public ObservableCollection<Model_service_catogory> Catergory_Sevice
        {
            get
            {
                return catergory_Sevice;
            }
            set
            {
                catergory_Sevice = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Model_service_item> selectedCatergory_Item_Sevice;
        public ObservableCollection<Model_service_item> SelectedCatergory_Item_Sevice
        {
            get
            {
                return selectedCatergory_Item_Sevice;
            }
            set
            {
                selectedCatergory_Item_Sevice = value;
                OnPropertyChanged();
            }
        }

        private string blig_mounth;
        public string BligMounth
        {
            get
            {
                switch(blig_mounth)
                {
                    case "1": return "Января";
                    case "2": return "Февраля";
                    case "3": return "Марта";
                    case "4": return "Апреля";
                    case "5": return "Мая";
                    case "6": return "Июня";
                    case "7": return "Июля";
                    case "8": return "Августа";
                    case "9": return "Сентября";
                    case "10": return "Октября";
                    case "11": return "Ноября";
                    case "12": return "Декабря";
                }

                return blig_mounth;
            }
            set
            {
                blig_mounth = value;
                OnPropertyChanged();
            }
        }

        public Model_Master()
        {
            Catergory_Sevice = new ObservableCollection<Model_service_catogory>();
            SelectedCatergory_Item_Sevice = new ObservableCollection<Model_service_item>();
            Reviews = new ObservableCollection<Model_review>();
            Stars = new ObservableCollection<bool>();
            Appointment_dates = new ObservableCollection<Model_appointment_date>();
            pathImg = "icon_user.png";
            Schedules = new ObservableCollection<Model_schedule>();
            Reviews_OnChanged();
            Stars_OnChanged();
            Appointment_dates_OnChanged();
        }
    }
}
