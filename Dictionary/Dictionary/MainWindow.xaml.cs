using System.Windows;

namespace Dictionary
{
    public partial class MainWindow : Window
    {
        public static bool IsUserAdmin = false;
        public MainWindow()
        {
            InitializeComponent();
            AddWordButton.Visibility = IsUserAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            AddWordWindow addWordWindow = new AddWordWindow();
            addWordWindow.Show();
            this.Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

    }
}
