using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dictionary
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isUserAdmin;

        public bool IsUserAdmin
        {
            get => isUserAdmin;
            set
            {
                if (isUserAdmin != value)
                {
                    isUserAdmin = value;
                    OnPropertyChanged(nameof(IsUserAdmin));
                }
            }
        }

        public ICommand AddWordCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }
        public ICommand SearchWordCommand { get; private set; }

        public MainViewModel()
        {
            AddWordCommand = new RelayCommand(() => ExecuteAddWordCommand()); // Presupunem că există o metodă CanExecuteAddWordCommand
            PlayCommand = new RelayCommand(() => ExecutePlayCommand()); // Presupunând că aceste metode există
            LogOutCommand = new RelayCommand(() => ExecuteLogOutCommand());
            SearchWordCommand = new RelayCommand(() => ExecuteSearchWordCommand());
        }

        private void ExecuteAddWordCommand()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddWordWindow addWordWindow = new AddWordWindow();
                addWordWindow.Show();
            });
        }

        private void ExecutePlayCommand()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                PlayWindow playWindow = new PlayWindow();
                playWindow.Show();
            });
        }

        private void ExecuteLogOutCommand()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Application.Current.MainWindow.Close();
            });
        }

        private void ExecuteSearchWordCommand()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SearchWordWindow searchWordWindow = new SearchWordWindow();
                searchWordWindow.Show();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
