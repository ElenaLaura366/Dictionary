using System.Windows;

namespace Dictionary
{
    public partial class WordDetailsWindow : Window
    {
        public WordDetailsWindow()
        {
            InitializeComponent();
        }

        public WordDetailsWindow(WordItem wordItem)
        {
            InitializeComponent();
            var viewModel = new WordDetailsViewModel { WordItem = wordItem };
            DataContext = viewModel;
        }
    }
}
