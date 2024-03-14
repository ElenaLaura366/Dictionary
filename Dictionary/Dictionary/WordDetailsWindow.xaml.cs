using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for WordDetailsWindow.xaml
    /// </summary>
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
