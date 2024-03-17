using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using System.IO;

namespace Dictionary
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _passwordVerify;
        private readonly ObservableCollection<User> _users;
        private readonly RelayCommand _registerCommand;
        private readonly RelayCommand _loginCommand;

        public RegisterViewModel()
        {
            _users = new ObservableCollection<User>();
            _registerCommand = new RelayCommand(Register, CanRegister);
            _loginCommand = new RelayCommand(Login);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string PasswordVerify
        {
            get => _passwordVerify;
            set
            {
                _passwordVerify = value;
                OnPropertyChanged(nameof(PasswordVerify));
            }
        }

        public ICommand RegisterCommand => _registerCommand;

        public ICommand LoginCommand => _loginCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) &&
                   Password == PasswordVerify;
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
        private void Register(object parameter)
        {
            if (_users.Any(u => u.username == Username))
            {
                MessageBox.Show("Username already exists!");
                return;
            }

            _users.Add(new User { username = Username, password = Password, isAdmin = false });

            // Load existing users from JSON file
            List<User> existingUsers = ReadUsersFromFile();

            // Add the new user to the existing users
            existingUsers.AddRange(_users);

            // Serialize the updated list to JSON
            string json = JsonConvert.SerializeObject(new UsersList { users = existingUsers }, Formatting.Indented);

            try
            {
                string filePath = GetFilePath();
                File.WriteAllText(filePath, json);
                MessageBox.Show("User registered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the user: {ex.Message}");
            }
        }

        private string GetFilePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"..\..\Data\users.json";
            string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
            return fullPath;
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

        private void Login(object parameter)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is RegisterWindow)
                {
                    window.Close();
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
