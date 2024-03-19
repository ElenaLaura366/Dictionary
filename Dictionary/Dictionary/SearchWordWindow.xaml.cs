using System.Windows;

namespace Dictionary
{
    public partial class SearchWordWindow : Window
    {
        public SearchWordWindow()
        {
            InitializeComponent();
            this.DataContext = new SearchWordViewModel();
        }
    }
}
