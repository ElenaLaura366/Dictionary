using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;

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
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = passPassword.Password;

            var usersList = ReadUsersFromFile();
            var user = usersList.FirstOrDefault(u => u.username == username && u.password == password);

            if (user != null)
            {
                MessageBox.Show("Login successful!");
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
