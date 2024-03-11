using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace Dictionary
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
        private string GetFilePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Acesta este directorul bin\Debug sau bin\Release
            string relativePath = @"..\..\Data\users.json"; // Ajustează aceasta conform structurii proiectului tău
            string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
            return fullPath;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;
            string passwordVerify = passPasswordVerify.Password;

            if (password != passwordVerify)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            var usersList = ReadUsersFromFile();
            if (usersList.Any(u => u.username == username))
            {
                MessageBox.Show("Username already exists!");
                return;
            }

            usersList.Add(new User { username = username, password = password, isAdmin = false });
            WriteUsersToFile(usersList);

            MessageBox.Show("User registered successfully!");
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

        private void WriteUsersToFile(List<User> usersList)
        {
            try
            {
                string filePath = GetFilePath();
                var users = new UsersList { users = usersList };
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write to file: {ex.Message}");
            }
        }
    }
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
    }

    public class UsersList
    {
        public List<User> users { get; set; }
    }
}
