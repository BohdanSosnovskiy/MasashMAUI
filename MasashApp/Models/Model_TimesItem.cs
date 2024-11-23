using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasashApp.Models
{
    public class Model_TimesItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DateTime time;
        /// <summary>
        /// Время записи на прем
        /// </summary>
        public DateTime Time
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

        private bool isBrone;
        /// <summary>
        /// Есть ли на данное время запись
        /// </summary>
        public bool IsBrone
        {
            get
            {
                return isBrone;
            }
            set
            {
                isBrone = value;
                OnPropertyChanged();
            }
        }

        private Frame frame_layout;
        public Frame Frame_layout
        {
            get
            {
                return frame_layout;
            }
            set
            {
                frame_layout = value;
                OnPropertyChanged();
            }
        }

        public Model_TimesItem()
        {
            Frame_layout = new Frame();
        }
    }
}
