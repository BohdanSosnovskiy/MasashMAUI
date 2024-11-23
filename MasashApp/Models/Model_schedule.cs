using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_schedule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Model_schedule() 
        {
            Times = new List<Model_TimesItem>();
        }

        private DateTime date;
        /// <summary>
        /// Дата записи на прием
        /// </summary>
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

        private List<Model_TimesItem> times;
        /// <summary>
        /// Список доступного времени на прием
        /// </summary>
        public List<Model_TimesItem> Times
        {
            get
            {
                return times;
            }
            set
            {
                times = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Ворзвращает список времени на прием из строк
        /// </summary>
        /// <param name="startTime">Время начала работы</param>
        /// <param name="endTime">Время оканчания работы</param>
        /// <param name="interval">Интервал между приемами в минутах</param>
        /// <returns></returns>
        public List<Model_TimesItem> SetDiaposonTimes(string startTime, string endTime, int interval)
        {
            try
            {
                var _startTime = startTime.Split(':');
                int startHours = Convert.ToInt32(_startTime[0]);
                int startMinutes = Convert.ToInt32(_startTime[1]);

                var _endTime = endTime.Split(':');
                int endHours = Convert.ToInt32(_endTime[0]);
                int endMinutes = Convert.ToInt32(_endTime[1]);

                SetDiaposonTimes(new DateTime(Date.Year, Date.Month, Date.Day, startHours,startMinutes,0), new DateTime(Date.Year, Date.Month, Date.Day, endHours, endMinutes, 0), interval);
                return times;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
                return new List<Model_TimesItem>();
            }
            
        }
        /// <summary>
        /// Ворзвращает список времени на прием из DateTime
        /// </summary>
        /// <param name="startTime">Время начала работы</param>
        /// <param name="endTime">Время оканчания работы</param>
        /// <param name="interval">Интервал между приемами в минутах</param>
        /// <returns></returns>
        public void SetDiaposonTimes(DateTime startTime, DateTime endTime, int interval)
        {
            Times = new List<Model_TimesItem>();
            DateTime current = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute,0);
            bool isNext = true;
            while (isNext)
            {
                var r = endTime - current;
                if (r.TotalSeconds >= 0)
                {
                    Times.Add(new Model_TimesItem()
                    {
                        Time = current,
                        IsBrone = false,
                    });
                    current = current.AddMinutes(interval);
                }
                else
                {
                    isNext = false;
                }
            }
        }
    }
}
