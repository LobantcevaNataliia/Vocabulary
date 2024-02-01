using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text;
using System.Collections.ObjectModel;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vocabulary
{
    /// <summary>
    /// Логика взаимодействия для ListOfWords.xaml
    /// </summary>
    public partial class ListOfWords : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        ObservableCollection<Word> words;
        User user;

        public ListOfWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListWords.ItemsSource = words;
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            string newEnglish = newEnglishTb.Text;
            string newTranscription = newTranscriptionTb.Text;
            string newUkrainian = newUkrainianTb.Text;
            bool newStatus = false;

            if (!string.IsNullOrEmpty(newEnglish) && !string.IsNullOrEmpty(newTranscription) && !string.IsNullOrEmpty(newUkrainian))
            {
                InsertWordIntoDatabase(Change(newEnglish.Trim()), newTranscription.Trim(), Change(newUkrainian.Trim()), newStatus);
            }
            else MessageBox.Show("You need to fill all lines to add new word!");

            words.Add(new Word
            (
                words.Count + 1,
                Change(newEnglish.Trim()),
                newTranscription.Trim(),
                Change(newUkrainian.Trim()),
                newStatus
            ));
        }

        private string Change(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        private void InsertWordIntoDatabase(string newEnglish,string newTranscription,string newUkrainian,bool newStatus)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa слова в БД
                    string query = "INSERT INTO myVocabDB.words (EnglishWord, Transcription, UkrainianWord) VALUES (@Value1, @Value2, @Value3)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", newEnglish);
                        command.Parameters.AddWithValue("@Value2", newTranscription);
                        command.Parameters.AddWithValue("@Value3", newUkrainian);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding a word: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }

        private void ListOfWordsWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            ListOfWordsWindow.Close();
        }

        private void newEnglishTb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            newEnglishTb.SelectAll();
        }

        private void newTranscriptionTb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            newTranscriptionTb.SelectAll();
        }

        private void newUkrainianTb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            newUkrainianTb.SelectAll();
        }
    }
}
