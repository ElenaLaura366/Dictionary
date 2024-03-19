using System.Windows;

namespace Dictionary
{
    public partial class AddWordWindow : Window
    {
        public AddWordWindow()
        {
            InitializeComponent();
            this.DataContext = new AddWordViewModel();
        }
    }
}
