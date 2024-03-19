using System.Collections.Generic;
using System.Windows;

namespace Dictionary
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel();
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
