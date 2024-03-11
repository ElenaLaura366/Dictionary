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
        private List<WordItem> wordItems;
        public SearchWordWindow()
        {
            InitializeComponent();
            LoadCategories(); // Inițializare categorii
            LoadWordItems(); // Inițializare cuvinte

            // Setare eveniment pentru căutarea cuvintelor în timp real
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;

            // Setare valoarea implicită pentru ComboBox
            CategoryComboBox.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            // Construiește calea relativă către fișierul JSON în proiect
            string relativePath = @"..\..\Data\words.json"; // Acesta este calea relativă de la directorul executabilului
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory; // Acesta este directorul bin\Debug sau bin\Release
            string jsonFilePath = Path.GetFullPath(Path.Combine(appDirectory, relativePath)); // Transformă calea relativă în cale absolută

            // Verifică dacă fișierul există
            if (File.Exists(jsonFilePath))
            {
                // Citire și deserializare JSON
                string jsonData = File.ReadAllText(jsonFilePath);
                var items = JsonConvert.DeserializeObject<List<WordItem>>(jsonData) ?? new List<WordItem>();

                // Extragere categorii unice și sortare
                var categories = items.Select(item => item.Category).Distinct().OrderBy(c => c);

                // Adăugare categorii în ComboBox
                CategoryComboBox.Items.Clear(); // Curăță orice elemente existente
                foreach (var category in categories)
                {
                    CategoryComboBox.Items.Add(category);
                }

                CategoryComboBox.Items.Insert(0, "Category (Optional)");
                CategoryComboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show($"JSON file not found in path: {jsonFilePath}");
            }
        }

        private void LoadWordItems()
        {
            string relativePath = @"..\..\Data\words.json";
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.GetFullPath(Path.Combine(appDirectory, relativePath));

            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                wordItems = JsonConvert.DeserializeObject<List<WordItem>>(jsonData) ?? new List<WordItem>();
            }
            else
            {
                MessageBox.Show($"JSON file not found in path: {jsonFilePath}");
                wordItems = new List<WordItem>();
            }
        }

        private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            string selectedCategory = CategoryComboBox.SelectedItem as string;

            IEnumerable<string> filteredWords;

            if (selectedCategory != null && selectedCategory != "Category (Optional)")
            {
                // Filtrare cuvinte după textul introdus și categoria selectată
                filteredWords = wordItems
                    .Where(item => item.Word.ToLower().StartsWith(searchText) && item.Category == selectedCategory)
                    .Select(item => item.Word);
            }
            else
            {
                // Filtrare cuvinte după textul introdus în toate categoriile
                filteredWords = wordItems
                    .Where(item => item.Word.ToLower().StartsWith(searchText))
                    .Select(item => item.Word);
            }

            ResultsListBox.ItemsSource = filteredWords;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedWord = (string)ResultsListBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedWord))
            {
                WordItem selectedItem = wordItems.FirstOrDefault(item => item.Word == selectedWord);

                if (selectedItem != null)
                {
                    ShowSelectedItemDetails(selectedItem);
                }
            }
            else
            {
                MessageBox.Show("Please select a word before searching.", "No Word Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowSelectedItemDetails(WordItem selectedItem)
        {
            // Creare și afișare a unei noi ferestre pentru detalii
            WordDetailsWindow detailsWindow = new WordDetailsWindow(selectedItem);
            detailsWindow.ShowDialog();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
    public class WordItem
    {
        public string Word { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
    }
}
