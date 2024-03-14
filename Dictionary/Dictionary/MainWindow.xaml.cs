using System.Windows;

namespace Dictionary
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
            this.DataContext = new MainViewModel();
        }
    }
}
