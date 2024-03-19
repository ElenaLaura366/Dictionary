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
            this.IsUserAdmin = SessionManager.Instance.IsUserAdmin;
        }
    }
    public class SessionManager
    {
        public bool IsUserAdmin { get; set; } = false;

        // Singleton instance
        private static SessionManager instance;
        public static SessionManager Instance => instance ?? (instance = new SessionManager());

        // Constructor privat pentru a preveni instanțierea externă
        private SessionManager() { }
    }

}
