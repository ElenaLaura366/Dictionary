using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Dictionary
{
    public class SearchWordViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> FilteredWords { get; } = new ObservableCollection<string>();

        private List<WordItem> wordItems;
        private string selectedCategory;
        private string searchText;

        public string SelectedCategory
        {
            get => selectedCategory;
            set { selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); FilterWords(); }
        }

        public string SearchText
        {
            get => searchText;
            set { searchText = value; OnPropertyChanged(nameof(SearchText)); FilterWords(); }
        }

        public ICommand SearchCommand { get; }
        public ICommand BackCommand { get; }

        public SearchWordViewModel()
        {
            LoadCategories();
            LoadWordItems();
            SearchCommand = new RelayCommand(ShowWordDetails);
            BackCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void LoadCategories()
        {
            string relativePath = @"..\..\Data\words.json";
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.GetFullPath(Path.Combine(appDirectory, relativePath));

            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                var items = JsonConvert.DeserializeObject<List<WordItem>>(jsonData) ?? new List<WordItem>();
                var categories = items.Select(item => item.Category).Distinct().OrderBy(c => c);
                Categories.Clear();
                Categories.Add("All Categories"); // Adăugăm categoria implicită
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
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
            }
        }

        private void FilterWords()
        {
            FilteredWords.Clear();
            if (string.IsNullOrEmpty(SearchText))
            {
                return;
            }

            var filteredItems = wordItems.Where(item => item.Word.ToLower().StartsWith(SearchText.ToLower()));

            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All Categories")
            {
                filteredItems = filteredItems.Where(item => item.Category == SelectedCategory);
            }

            foreach (var item in filteredItems)
            {
                FilteredWords.Add(item.Word);
            }
        }

        private void ShowWordDetails(object selectedWord)
        {
            if (selectedWord is string word)
            {
                var wordItem = wordItems.FirstOrDefault(item => item.Word == word);
                if (wordItem != null)
                {
                    var detailsWindow = new WordDetailsWindow(wordItem);
                    detailsWindow.ShowDialog();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
