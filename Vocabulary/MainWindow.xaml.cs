using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Vocabulary
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ObservableCollection<Word> words;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            words = new ObservableCollection<Word>();

            // Рядок підключення до бази даних MySQL
            string connectionString = "server=localhost;user=NataliiaLobantseva;database=myVocabDB;port=3306;password=!23Asdfgh";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // SQL-запит для вибору всіх записів з таблиці Words
                string query = "SELECT * FROM myVocabDB.words";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Word word = new Word
                            (
                                Convert.ToInt32(reader["WordID"]),
                                reader["EnglishWord"].ToString(),
                                reader["Transcription"].ToString(),
                                reader["UkrainianWord"].ToString(),
                                Convert.ToBoolean(reader["Status"])
                            );

                            words.Add(word);
                        }
                    }
                }
            }
          
        }
        
        private void Auto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            Window fullList = new ListOfWords(words);
            fullList.Show();
            MainWindowWindow.Hide();
        }

        private void Exercise_Click(object sender, RoutedEventArgs e)
        {
            Window exercise = new Exercise(words);
            exercise.Show();
            MainWindowWindow.Hide();
        }
    }
}
