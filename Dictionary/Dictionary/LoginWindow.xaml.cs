using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System;

namespace Dictionary
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(); // Setează DataContext pentru a folosi LoginViewModel
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerViewModel = new RegisterViewModel(); // Crează un nou ViewModel pentru înregistrare
            var registerWindow = new RegisterWindow(); // Crează o nouă fereastră pentru înregistrare
            registerWindow.DataContext = registerViewModel; // Setează DataContext-ul pentru fereastra de înregistrare
            registerWindow.Show(); // Afișează fereastra de înregistrare
            Close(); // Închide fereastra de autentificare
        }
    }
}
