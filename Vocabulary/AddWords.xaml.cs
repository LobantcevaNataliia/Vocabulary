using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for AddWords.xaml
    /// </summary>
    public partial class AddWords : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        ObservableCollection<Word> words;
        User user;

        public AddWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            string newEnglish = newEnglishTb.Text;
            string newTranscription = newTranscriptionTb.Text;
            string newUkrainian = newUkrainianTb.Text;
            bool newStatus = false;

            if (!string.IsNullOrEmpty(newEnglish) && !string.IsNullOrEmpty(newTranscription) && !string.IsNullOrEmpty(newUkrainian))
            {
                WorkWithNewWords(newEnglish, newTranscription, newUkrainian, newStatus);
                newEnglishTb.Text = "Enter a new word...";
                newTranscriptionTb.Text = "Enter the transcription of the word...";
                newUkrainianTb.Text = "Enter the translation of the word...";
            }
            else MessageBox.Show("You need to fill all lines to add new word!");
        }

        private void WorkWithNewWords(string newEnglish, string newTranscription, string newUkrainian, bool newStatus)
        {
            //newEnglish = Change(newEnglish.Trim());
            newTranscription = newTranscription.Trim();
            newUkrainian = Change(newUkrainian.Trim());

            if (!WordExists(newEnglish))
            {
                int id = GetIdNewWord();
                InsertWordIntoDatabase(id, newEnglish, newTranscription, newUkrainian);
                words.Add(new Word(id, newEnglish, newTranscription, newUkrainian, newStatus, Level.U));
                InsertDependenceIntoDatabase(words[words.Count - 1]);


                //ListWords.Items.Add(new { i = words.Count, words[words.Count - 1].english, words[words.Count - 1].transcription, words[words.Count - 1].ukrainian, words[words.Count - 1].status });
            }
            else MessageBox.Show($"The word {newEnglish} already exists!");

        }

        private string Change(string str)
        {
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            else return str.ToUpper();
        }

        private bool WordExists(string newEnglish)
        {
            bool exist = false;

            for (int i = 0; i < words.Count; i++)
                if (words[i].English == newEnglish)
                    exist = true;

            return exist;
        }

        private int GetIdNewWord()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // 
                string query = "SELECT MAX(WordID) FROM myVocabDB.words;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result) + 1;
                }
            }
        }

        private void InsertWordIntoDatabase(int id, string newEnglish, string newTranscription, string newUkrainian)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa слова в БД
                    string query = "INSERT INTO myVocabDB.words (WordID, EnglishWord, Transcription, UkrainianWord, Level) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", id);
                        command.Parameters.AddWithValue("@Value2", newEnglish);
                        command.Parameters.AddWithValue("@Value3", newTranscription);
                        command.Parameters.AddWithValue("@Value4", newUkrainian);
                        command.Parameters.AddWithValue("@Value5", "U");
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding a word: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }

        private void InsertDependenceIntoDatabase(Word word)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa зв'язку в БД
                    string query = "INSERT INTO myVocabDB.learnedwords (UserId, WordId, Status) VALUES (@Value1, @Value2, @Value3)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", user.id);
                        command.Parameters.AddWithValue("@Value2", word.Id);
                        command.Parameters.AddWithValue("@Value3", word.Status);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding dependence: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }




    }
}
