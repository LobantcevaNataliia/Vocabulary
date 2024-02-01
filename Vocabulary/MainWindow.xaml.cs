using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

namespace Vocabulary
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DefaultUser();

        }

        public MainWindow(User user)
        {
            InitializeComponent();
            if (user != null)
                this.user = user;
            else DefaultUser();
        }

        User user;
        ObservableCollection<Word> words;
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void DefaultUser()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE UserName = @UserName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", "Guest");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new User
                            (
                                Convert.ToInt32(reader["UserID"]),
                                reader["UserName"].ToString(),
                                reader["UserEmail"].ToString(),
                                reader["UserPassword"].ToString()
                            );
                        }
                    }            
                }
            }
        }

        //Метод для завантаження словника з бази даних
        private void LoadDataFromDatabase()
        {
            words = new ObservableCollection<Word>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // команда SQL для вибірки слів конкретного користувача
                string sql = "SELECT myVocabDB.words.*, LearnedWords.Status FROM myVocabDB.words " +
                             "INNER JOIN myVocabDB.learnedwords ON words.WordId = learnedwords.WordId " +
                             "WHERE learnedwords.UserId = @UserId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Додано параметр для ідентифікатора користувача
                    command.Parameters.AddWithValue("@UserId", user.id);

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
            Window auto = new Authorization();
            auto.Show();
            MainWindowWindow.Hide();
        }

        //Метод для відображення списку слів
        private void List_Click(object sender, RoutedEventArgs e)
        {
            Window fullList = new ListOfWords(words, user);
            fullList.Show();
            MainWindowWindow.Hide();
        }

        //Метод для переходy до вибору вправ
        private void Exercise_Click(object sender, RoutedEventArgs e)
        {
            Window exercise = new Exercise(words, user);
            exercise.Show();
            MainWindowWindow.Hide();
        }
    }
}
