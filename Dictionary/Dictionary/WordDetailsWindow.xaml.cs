using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for WordDetailsWindow.xaml
    /// </summary>
    public partial class WordDetailsWindow : Window
    {
        public WordDetailsWindow()
        {
            InitializeComponent();
        }
        public WordDetailsWindow(WordItem wordItem)
        {
            InitializeComponent();
            ShowWordDetails(wordItem);
        }

        private void ShowWordDetails(WordItem wordItem)
        {
            // Afișare detalii despre obiectul selectat
            if (wordItem != null)
            {
                CustomWord.Text = wordItem.Word;
                CustomCategory.Text = wordItem.Category;
                CustomDescription.Text = wordItem.Description;

                // Setarea imaginii
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Images\\", wordItem.ImageName);
                if (File.Exists(imagePath))
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                    MyImage.Source = bitmap;
                }
                else
                {
                    MessageBox.Show("Image not found.");
                }
            }
        }
    }
}
