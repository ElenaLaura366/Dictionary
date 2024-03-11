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
            LoadCategories();
        }

        private void chkIsPhrase_Checked(object sender, RoutedEventArgs e)
        {
            txtCategory.Visibility = Visibility.Visible; // Arată caseta de text
            lstCategories.Visibility = Visibility.Collapsed; // Ascunde lista
        }

        private void chkIsPhrase_Unchecked(object sender, RoutedEventArgs e)
        {
            txtCategory.Visibility = Visibility.Collapsed; // Ascunde caseta de text
            lstCategories.Visibility = Visibility.Visible; // Arată lista
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg", // Filtrează doar fișierele de imagine
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) // Deschide direct în biblioteca de imagini
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory; // Acesta este directorul bin\Debug sau bin\Release
                string relativePath = @"..\..\Images\"; // Ajustează aceasta conform structurii proiectului tău
                string targetPath = Path.Combine(projectDirectory, relativePath);
                string destFile = Path.Combine(targetPath, Path.GetFileName(fileName));

                // Asigură-te că folderul de destinație există
                Directory.CreateDirectory(targetPath);

                // Copiază fișierul în folderul de destinație
                File.Copy(fileName, destFile, true); // 'true' permite suprascrierea fișierului dacă acesta există deja

                // Actualizează numele fișierului pe buton sau într-un textblock
                txtImageName.Text = Path.GetFileName(fileName);
            }
        }
        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            // Verifică dacă toate câmpurile obligatorii sunt completate
            string selectedCategory = chkIsPhrase.IsChecked == true ? txtCategory.Text : (lstCategories.SelectedItem as ListBoxItem)?.Content.ToString();
            if (!string.IsNullOrWhiteSpace(txtWord.Text) && !string.IsNullOrWhiteSpace(selectedCategory) && !string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                // Citește fișierul JSON existent
                string filePath = @"..\..\Data\words.json";
                List<dynamic> words = new List<dynamic>();
                bool wordExists = false;
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    words = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();

                    // Verifică dacă cuvântul deja există
                    foreach (var word in words)
                    {
                        if (word.Word.ToString().ToLower() == txtWord.Text.ToLower())
                        {
                            wordExists = true;
                            break;
                        }
                    }
                }

                if (!wordExists)
                {
                    // Crează un obiect pentru a reprezenta noul cuvânt și adaugă-l în listă
                    var newWord = new
                    {
                        Word = txtWord.Text,
                        Category = selectedCategory,
                        Description = txtDescription.Text,
                        ImageName = string.IsNullOrWhiteSpace(txtImageName.Text) ? "default.png" : txtImageName.Text
                    };
                    words.Add(newWord);

                    // Scrie lista actualizată înapoi în fișierul JSON
                    string newJson = JsonConvert.SerializeObject(words, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);

                    // Reîncarcă categoriile pentru a include noi adăugări
                    LoadCategories();

                    // Resetează controalele UI după adăugarea cuvântului
                    txtWord.Clear();
                    chkIsPhrase.IsChecked = false;
                    txtCategory.Clear();
                    txtDescription.Clear();
                    txtImageName.Text = "";
                    lstCategories.UnselectAll();

                    MessageBox.Show("Word added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("The word already exists in the dictionary.", "Duplicate Word", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please ensure that all required fields (word, category, and description) are provided.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadCategories()
        {
            string filePath = @"..\..\Data\words.json";
            HashSet<string> categories = new HashSet<string>(); // Folosim un HashSet pentru a evita duplicatele

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var words = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();
                foreach (var word in words)
                {
                    categories.Add(word.Category.ToString());
                }
            }

            // Sortează categoriile în ordine alfabetică
            var sortedCategories = categories.OrderBy(c => c).ToList();

            // Actualizează ListBox-ul cu categoriile
            lstCategories.Items.Clear();
            foreach (var category in sortedCategories)
            {
                lstCategories.Items.Add(new ListBoxItem { Content = category });
            }
        }
    }
}
