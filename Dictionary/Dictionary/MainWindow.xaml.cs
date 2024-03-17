using System.Windows;

namespace Dictionary
{
    public partial class MainWindow : Window
    {
        public bool IsUserAdmin { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            this.DataContext = new MainViewModel();
            this.IsUserAdmin = ApplicationState.IsUserAdmin; // Utilizează starea globală pentru a seta dacă utilizatorul este admin
        }

    }
    public static class ApplicationState
    {
        public static bool IsUserAdmin { get; set; } = false;
    }
}
