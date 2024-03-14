using System;
using System.ComponentModel;
using System.IO;

namespace Dictionary
{
    public class WordDetailsViewModel : INotifyPropertyChanged
    {
        private WordItem wordItem;

        public WordItem WordItem
        {
            get => wordItem;
            set
            {
                wordItem = value;
                OnPropertyChanged(nameof(WordItem));
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public string ImagePath => WordItem != null ?
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images\\", WordItem.ImageName) :
            string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
