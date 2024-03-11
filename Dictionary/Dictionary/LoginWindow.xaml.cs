using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System;

namespace Dictionary
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
        private string GetFilePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Acesta este directorul bin\Debug sau bin\Release
            string relativePath = @"..\..\Data\users.json"; // Ajustează aceasta conform structurii proiectului tău
            string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
            return fullPath;
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;

            var usersList = ReadUsersFromFile();
            var user = usersList.FirstOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                MessageBox.Show("Login successful!");
                // Presupunând că ai o logică pentru setarea statutului de administrator al utilizatorului curent în MainWindow
                MainWindow.IsUserAdmin = user.isAdmin;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
