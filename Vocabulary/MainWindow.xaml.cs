using System;
using System.Collections.Generic;
using System.Windows;
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

        //Метод для завантаження словника з бази даних
        private void LoadDataFromDatabase()
        {
            words = new ObservableCollection<Word>();

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
        
        //Метод для авторизації
        private void Auto_Click(object sender, RoutedEventArgs e)
        {

        }

        //Метод для відображення списку слів
        private void List_Click(object sender, RoutedEventArgs e)
        {
            Window fullList = new ListOfWords(words);
            fullList.Show();
            MainWindowWindow.Hide();
        }

        //Метод для переходy до вибору вправ
        private void Exercise_Click(object sender, RoutedEventArgs e)
        {
            Window exercise = new Exercise(words);
            exercise.Show();
            MainWindowWindow.Hide();
        }
    }
}
