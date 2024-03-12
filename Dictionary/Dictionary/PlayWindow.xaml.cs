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
        private List<WordItem> words = new List<WordItem>();
        private WordItem currentWordItem;

        private int currentRound = 0;
        private int score = 0;
        private enum DisplayMode { Image, Text }
        private DisplayMode currentDisplayMode;
        private const int totalRounds = 5;
        private List<WordItem> usedWords = new List<WordItem>();

        public PlayWindow()
        {
            InitializeComponent();
            LoadWords();
            StartNewRound();
            UpdateScore();
        }

        private void LoadWords()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Data\\words.json");
            var json = File.ReadAllText(fullPath);
            words = JsonConvert.DeserializeObject<List<WordItem>>(json);
        }

        private void StartNewRound()
        {
            txtBox.Text = "";
            MyImage.Source = null;
            MyTextBlock.Text = "";
            if (currentRound >= totalRounds)
            {
                MessageBox.Show($"Joc terminat! Scorul tău final este: {score}");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
                return;
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

            currentDisplayMode = (currentWordItem.ImageName == "default.png") ? DisplayMode.Text : (DisplayMode)rnd.Next(2);

            UpdateUI();
            currentRound++;
        }

        private void UpdateUI()
        {
            if (currentDisplayMode == DisplayMode.Image)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images\\", currentWordItem.ImageName);
                MyImage.Visibility = Visibility.Visible;
                MyTextBlock.Visibility = Visibility.Hidden;
                MyImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)); // Set the image source
            }
            else
            {
                MyImage.Visibility = Visibility.Hidden;
                MyTextBlock.Visibility = Visibility.Visible;
                MyTextBlock.Text = currentWordItem.Description; // Set the text description
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox.Text.ToLower() == currentWordItem.Word.ToLower())
            {
                score++; // Increment the score
                MessageBox.Show("Corect!");
            }
            else
            {
                MessageBox.Show($"Greșit! Cuvântul era: {currentWordItem.Word}");
            }
            StartNewRound(); // Start a new round
            UpdateScore(); // Update the score
        }

        private void UpdateScore()
        {
            ScoreText.Text = $"Scor: {score}";
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
