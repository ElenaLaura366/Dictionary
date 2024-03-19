using System;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.ComponentModel;

namespace Dictionary
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        private string username;
        private string password;
        private bool isUserAdmin;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool IsUserAdmin
        {
            get => isUserAdmin;
            set
            {
                if (value != isUserAdmin)
                {
                    isUserAdmin = value;
                    OnPropertyChanged(nameof(IsUserAdmin));
                }
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Login()
        {
            var usersList = ReadUsersFromFile();
            var user = usersList.FirstOrDefault(u => u.username == Username && u.password == Password);

            if (user != null)
            {
                MessageBox.Show("Login successful!");
                IsUserAdmin = user.isAdmin;
                SessionManager.Instance.IsUserAdmin = user.isAdmin; // Modificare aici
                ShowMainWindow();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void ShowMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel { IsUserAdmin = this.IsUserAdmin }; // Transfer the user's admin status to the MainViewModel
            mainWindow.Show();
            CloseLoginWindow();
        }

        private void CloseLoginWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                }
            }
        }

        private void Register()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                }
            }
        }

        private List<User> ReadUsersFromFile()
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            string json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<UsersList>(json);
            return users?.users ?? new List<User>();
        }

        private string GetFilePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // This is the bin\Debug or bin\Release directory
            string relativePath = @"..\..\Data\users.json";
            string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
            return fullPath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
