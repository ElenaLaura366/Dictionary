using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Dictionary
{
    public enum DisplayMode { Image, Text }
    public class PlayWindowViewModel : INotifyPropertyChanged
    {
        private List<WordItem> words = new List<WordItem>();
        private WordItem currentWordItem;
        private int currentRound = 0;
        private int score = 0;
        private DisplayMode currentDisplayMode;
        private const int totalRounds = 5;
        private List<WordItem> usedWords = new List<WordItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> UsedWords { get; } = new ObservableCollection<string>();

        public string CurrentWord { get; set; }

        public string CurrentImagePath { get; set; }

        public string CurrentDescription { get; set; }

        public string ScoreText => $"Scor: {score}";

        public ICommand SubmitCommand { get; }
        public ICommand BackCommand { get; }
        public Action CloseAction { get; set; }

        public PlayWindowViewModel()
        {
            SubmitCommand = new RelayCommand(Submit);
            LoadWords();
            StartNewRound();
            BackCommand = new RelayCommand(o => CloseAction?.Invoke());
        }

        private void LoadWords()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Data\\words.json");
            var json = File.ReadAllText(fullPath);
            words = JsonConvert.DeserializeObject<List<WordItem>>(json);
        }

        private void StartNewRound()
        {
            if (currentRound >= totalRounds)
            {
                MessageBox.Show($"Joc terminat! Scorul tău final este: {score}");
                CloseAction?.Invoke();
            }

            Random rnd = new Random();
            // Choose a word that hasn't been used yet
            WordItem newItem;
            do
            {
                newItem = words[rnd.Next(words.Count)];
            } while (usedWords.Contains(newItem));

            currentWordItem = newItem;
            usedWords.Add(newItem); // add the new word to the list of used words

            CurrentWord = currentWordItem.Word;

            currentDisplayMode = (currentWordItem.ImageName == "default.png") ? DisplayMode.Text : (DisplayMode)rnd.Next(2);

            UpdateUI();
            currentRound++;
        }

        private void UpdateUI()
        {
            if (currentDisplayMode == DisplayMode.Image)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images\\", currentWordItem.ImageName);
                CurrentImagePath = imagePath;
                CurrentDescription = string.Empty; // Resetează câmpul de descriere
            }
            else
            {
                CurrentImagePath = null;
                CurrentDescription = currentWordItem.Description;
            }
            OnPropertyChanged(nameof(CurrentImagePath));
            OnPropertyChanged(nameof(CurrentDescription));
        }

        public string UserInput { get; set; }
        private void Submit(object parameter)
        {
            if (!string.IsNullOrEmpty(UserInput) && UserInput.ToLower() == currentWordItem.Word.ToLower())
            {
                score++; // Increment the score
                MessageBox.Show("Corect!");
            }
            else
            {
                MessageBox.Show($"Greșit! Cuvântul era: {currentWordItem.Word}");
            }
            StartNewRound(); // Start a new round
            OnPropertyChanged(nameof(ScoreText)); // Update the score
            UserInput = string.Empty; // Resetează câmpul de introducere
            OnPropertyChanged(nameof(UserInput)); // Actualizează interfața pentru a reflecta modificarea
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
