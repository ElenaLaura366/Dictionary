using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dictionary
{
    public partial class PlayWindow : Window
    {
        public PlayWindow()
        {
            InitializeComponent();
            this.DataContext = new PlayWindowViewModel();
        }
    }
}
