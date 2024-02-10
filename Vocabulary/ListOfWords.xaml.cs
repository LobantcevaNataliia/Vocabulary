using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Linq;
using Org.BouncyCastle.Crypto;
using Microsoft.Win32;
using System.IO;

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
            for (int i = 1; i <= words.Count; i++)
                ListWords.Items.Add(new { i, words[i - 1].english, words[i - 1].transcription, words[i - 1].ukrainian, words[i - 1].status });

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

        private void DownloadWords_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            try
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName, System.Text.Encoding.UTF8))
                {
                    while (sr.Peek() >= 0)
                    {
                        string lineCurrent = sr.ReadLine();
                        string[] newword = lineCurrent.Split('-');
                        if (newword.Length == 4)
                            WorkWithNewWords(newword[0], newword[1], newword[2], Convert.ToBoolean(Convert.ToInt16(newword[3])));
                        else MessageBox.Show("You need to ...");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while download a words: {ex.Message}" + "\nPlease contact the admin!");
            }
        }

        private void WorkWithNewWords(string newEnglish, string newTranscription, string newUkrainian, bool newStatus)
        {
            newEnglish = Change(newEnglish.Trim());
            newTranscription = newTranscription.Trim();
            newUkrainian = Change(newUkrainian.Trim());

            if (!WordExists(newEnglish))
            {
                int id = GetIdNewWord();
                InsertWordIntoDatabase(id, newEnglish, newTranscription, newUkrainian);
                words.Add(new Word(id, newEnglish, newTranscription, newUkrainian, newStatus, Level.U));
                InsertDependenceIntoDatabase(words[words.Count - 1]);
                
                
                ListWords.Items.Add(new { i = words.Count, words[words.Count - 1].english, words[words.Count - 1].transcription, words[words.Count - 1].ukrainian, words[words.Count - 1].status });
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

            for(int i = 0; i < words.Count; i++)
                if (words[i].english == newEnglish)
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

        private void InsertWordIntoDatabase(int id, string newEnglish,string newTranscription,string newUkrainian)
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
                        command.Parameters.AddWithValue("@Value2", word.id);
                        command.Parameters.AddWithValue("@Value3", word.status);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding dependence: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }


        private void DeleteWordFromDatabase(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa слова в БД
                    string query = "DELETE FROM myVocabDB.words WHERE WordID=@Value1;";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while delete a word: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }

        private void DeleteDependenceFromDatabase(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa зв'язку в БД
                    string query = "DELETE FROM myVocabDB.learnedwords WHERE UserId=@Value1 AND WordId=@Value2;";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", user.id);
                        command.Parameters.AddWithValue("@Value2", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while delete dependence: {ex.Message}" + "\nPlease contact the admin!");
                }
            }

        }

        private void ChangeStatusWordInDatabase(int wordId, bool newStatus)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.learnedwords SET Status = @newValue1 WHERE WordId= @newValue2 AND UserId= @newValue3;";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@newValue1", newStatus);
                    cmd.Parameters.AddWithValue("@newValue2", wordId);
                    cmd.Parameters.AddWithValue("@newValue3", user.id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected < 1)
                        MessageBox.Show($"An error occurred while changing the status of word." + "\nPlease contact the admin!");
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

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Отримання DataGridCellInfo обраної ячейки
            DataGridCellInfo cellInfo = ListWords.SelectedCells.FirstOrDefault();

            // Отримання значення ячейки
            int cellValue = Convert.ToInt16(cellInfo.Item.GetType().GetProperty(cellInfo.Column.SortMemberPath).GetValue(cellInfo.Item, null));

            int id = words[cellValue - 1].id;

            DeleteDependenceFromDatabase(id);
            if(words[cellValue - 1].level == Level.U)
                DeleteWordFromDatabase(id);

            words.RemoveAt(cellValue - 1);
            ListWords.Items.Clear();
            for (int i = 1; i <= words.Count; i++)
                ListWords.Items.Add(new { i, words[i - 1].english, words[i - 1].transcription, words[i - 1].ukrainian, words[i - 1].status });

        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Отримання DataGridCellInfo обраної ячейки
            DataGridCellInfo cellInfo = ListWords.SelectedCells.FirstOrDefault();

            // Отримання значення ячейки
            int cellValue = Convert.ToInt16(cellInfo.Item.GetType().GetProperty(cellInfo.Column.SortMemberPath).GetValue(cellInfo.Item, null));

            int id = words[cellValue - 1].id;

            if (!words[cellValue - 1].status)
                words[cellValue - 1].status = true;
            else words[cellValue - 1].status = false;

            ChangeStatusWordInDatabase(id, words[cellValue - 1].status);
            ListWords.Items[cellValue - 1] = new { i = cellValue, words[cellValue - 1].english, words[cellValue - 1].transcription, words[cellValue - 1].ukrainian, words[cellValue - 1].status };
            
        }

    }
}
