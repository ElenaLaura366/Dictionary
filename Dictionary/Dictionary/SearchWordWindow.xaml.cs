using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Dictionary
{
    /// <summary>
    /// Interaction logic for SearchWordWindow.xaml
    /// </summary>
    public partial class SearchWordWindow : Window
    {
        public SearchWordWindow()
        {
            InitializeComponent();
            this.DataContext = new SearchWordViewModel();
        }
    }
}
