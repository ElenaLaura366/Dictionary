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

            usersList.Add(new User { username = username, password = password });
            WriteUsersToFile(usersList);

            MessageBox.Show("User registered successfully!");
        }

        private List<User> ReadUsersFromFile()
        {
            string filePath = "users.json";
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }

            string json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<UsersList>(json);
            return users.users;
        }

        private void WriteUsersToFile(List<User> usersList)
        {
            try
            {
                var users = new UsersList { users = usersList };
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText("users.json", json);
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
    }

    public class UsersList
    {
        public List<User> users { get; set; }
    }
}
