using System.Windows;

namespace Dictionary
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerViewModel = new RegisterViewModel();
            var registerWindow = new RegisterWindow();
            registerWindow.DataContext = registerViewModel;
            registerWindow.Show();
            Close();
        }
    }
}
