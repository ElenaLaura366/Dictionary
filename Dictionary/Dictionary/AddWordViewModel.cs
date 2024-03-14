using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.Win32; // Necesar pentru OpenFileDialog
using System.IO;

namespace Dictionary
{
    public class AddWordViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();
        public ICommand AddWordCommand { get; private set; }
        public ICommand SelectImageCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        private string word;
        private string description;
        private string imageName;
        private string category;
        private bool isPhrase;

        public string Word
        {
            get => word;
            set
            {
                word = value;
                OnPropertyChanged(nameof(Word));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ImageName
        {
            get => imageName;
            set
            {
                imageName = value;
                OnPropertyChanged(nameof(ImageName));
            }
        }

        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public bool IsPhrase
        {
            get => isPhrase;
            set
            {
                if (isPhrase != value)
                {
                    isPhrase = value;
                    OnPropertyChanged(nameof(IsPhrase));
                    OnPropertyChanged(nameof(IsCategoryVisible));
                    OnPropertyChanged(nameof(IsCategoryListVisible));

                    if (!isPhrase)
                    {
                        // Reset Category when switching back to the list view
                        Category = null;
                    }
                }
            }
        }

        public Visibility IsCategoryVisible => IsPhrase ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsCategoryListVisible => IsPhrase ? Visibility.Collapsed : Visibility.Visible;

        public AddWordViewModel()
        {
            AddWordCommand = new RelayCommand(param => AddWord());
            SelectImageCommand = new RelayCommand(param => SelectImage());
            BackCommand = new RelayCommand(param => GoBack());
            LoadCategories();
        }

        private void AddWord()
        {
            if (!string.IsNullOrWhiteSpace(Word) && !string.IsNullOrWhiteSpace(Category) && !string.IsNullOrWhiteSpace(Description))
            {
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = @"..\..\Data\words.json";
                string filePath = Path.Combine(projectDirectory, relativePath);

                List<dynamic> words = new List<dynamic>();
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    words = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();
                }

                if (!words.Any(w => w.Word.ToString().ToLower() == Word.ToLower()))
                {
                    var newWord = new
                    {
                        Word,
                        Category,
                        Description,
                        ImageName = ImageName ?? "default.png"
                    };
                    words.Add(newWord);

                    string newJson = JsonConvert.SerializeObject(words, Formatting.Indented);
                    File.WriteAllText(filePath, newJson);

                    // Adaugă categoria nouă în lista de categorii, dacă nu există deja
                    if (!Categories.Contains(Category))
                    {
                        Categories.Add(Category);
                        // Aici ar trebui să te asiguri că lista de categorii din UI este actualizată
                        OnPropertyChanged(nameof(Categories));
                    }

                    // Resetează formularul
                    ResetForm();

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
        private void ResetForm()
        {
            Word = string.Empty;
            Description = string.Empty;
            ImageName = null;
            IsPhrase = false; // Reset this if necessary
                              // Do not clear the categories list or selected category to allow for quick additions
        }

        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Acesta este directorul bin\Debug sau bin\Release
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = @"..\..\Images\"; // Ajustează aceasta conform structurii proiectului tău
                string targetPath = Path.Combine(projectDirectory, relativePath);

                // Verifică și creează directorul dacă nu există
                Directory.CreateDirectory(targetPath);

                string sourceFilePath = openFileDialog.FileName;
                string destFilePath = Path.Combine(targetPath, Path.GetFileName(sourceFilePath));

                // Copiază fișierul în directorul țintă, suprascriindu-l dacă există deja
                File.Copy(sourceFilePath, destFilePath, true);

                // Actualizează proprietatea ImageName pentru a reflecta calea relativă sau numele fișierului pentru imaginea selectată
                ImageName = Path.GetFileName(sourceFilePath);
            }
        }

        private void GoBack()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is AddWordWindow)
                    {
                        window.Close();
                    }
                }
            });
        }

        private void LoadCategories()
        {
            Categories.Clear();
            string filePath = @"..\..\Data\words.json"; // Ajustează acest cale conform nevoilor tale
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var words = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();
                var categories = new HashSet<string>();
                foreach (var word in words)
                {
                    categories.Add(word.Category.ToString());
                }

                foreach (string category in categories.OrderBy(c => c))
                {
                    Categories.Add(category);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
