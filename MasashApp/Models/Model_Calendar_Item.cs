using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_Calendar_Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Frame frame_control;
        public Frame Frame_control
        {
            get
            {
                return frame_control;
            }
            set
            {
                frame_control = value;
                OnPropertyChanged();
            }
        }

        private Label label_control;
        public Label Label_control
        {
            get
            {
                return label_control;
            }
            set
            {
                label_control = value;
                OnPropertyChanged();
            }
        }

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

        private bool isCurrentDay;
        public bool IsCurrentDay
        {
            get
            {
                return isCurrentDay;
            }
            set
            {
                isCurrentDay = value;
                OnPropertyChanged();
            }
        }

        private bool isSelectedDay;
        public bool IsSelectedDay
        {
            get
            {
                return isSelectedDay;
            }
            set
            {
                isSelectedDay = value;
                OnPropertyChanged();
            }
        }

        private bool isFoudTime;
        public bool IsFoudTime
        {
            get
            {
                return isFoudTime;
            }
            set
            {
                isFoudTime = value;
                OnPropertyChanged();
            }
        }

        private Model_schedule? schedule;
        public Model_schedule? Schedule
        {
            get
            {
                return schedule;
            }
            set
            {
                schedule = value;
                OnPropertyChanged();
            }
        }
    }
}
