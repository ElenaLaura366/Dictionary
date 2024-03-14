using System.Windows;
using System.IO;
using Microsoft.Win32;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for AddWordWindow.xaml
    /// </summary>
    public partial class AddWordWindow : Window
    {
        public AddWordWindow()
        {
            InitializeComponent();
            this.DataContext = new AddWordViewModel();
        }
    }
}
