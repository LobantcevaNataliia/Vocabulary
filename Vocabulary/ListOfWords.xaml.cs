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

namespace Vocabulary
{
    /// <summary>
    /// Логика взаимодействия для ListOfWords.xaml
    /// </summary>
    public partial class ListOfWords : Window
    {
        ObservableCollection<Word> words;

        public ListOfWords(ObservableCollection<Word> words)
        {
            InitializeComponent();
            this.words = words;
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
            
            if (newEnglish != "" && newTranscription != "" && newUkrainian != "")
                InsertDataIntoDatabase(newEnglish, newTranscription, newUkrainian, newStatus);
            else MessageBox.Show("You need to fill all lines to add new word!");

        }

        private void InsertDataIntoDatabase(string newEnglish,string newTranscription,string newUkrainian,bool newStatus)
        {
            string connectionString = "server=localhost;user=NataliiaLobantseva;database=myVocabDB;port=3306;password=!23Asdfgh";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Вставкa даних в БД
                    string query = "INSERT INTO myVocabDB.words (EnglishWord, Transcription, UkrainianWord, Status) VALUES (@Value1, @Value2, @Value3, @Value4)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {                    
                        command.Parameters.AddWithValue("@Value1", newEnglish);
                        command.Parameters.AddWithValue("@Value2", newTranscription);
                        command.Parameters.AddWithValue("@Value3", newUkrainian);
                        command.Parameters.AddWithValue("@Value4", newStatus);

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
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            ListOfWordsWindow.Close();
        }
    }
}
