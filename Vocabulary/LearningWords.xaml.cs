using System;
using System.Collections;
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
using MySql.Data.MySqlClient;

namespace Vocabulary
{
    public partial class LearningWords : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        ObservableCollection<Word> words;
        User user;
        public List<Word> listLearnWords;
        int iCurrent;

        public LearningWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listLearnWords = new List<Word>();
            for (int i = 0; i < words.Count; i++)
                if (!words[i].status)
                    listLearnWords.Add(words[i]);

            if (listLearnWords.Count() != 0)
            {
                iCurrent = 0;
                ShowWords(iCurrent);
            }
            else
            {
                MessageBox.Show("Congratulations! You learned all the words on the list.");
                LearningWordsWindow.Close();
            }
        }

        private void ShowWords(int i)
        {
            labelEnglish.Content = listLearnWords[i].english;
            labelTranscription.Content = listLearnWords[i].transcription;
            labelUkrainian.Content = listLearnWords[i].ukrainian;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent > 0)
            {
                ShowWords(iCurrent - 1);
                iCurrent--;
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent < listLearnWords.Count - 1)
            {
                ShowWords(iCurrent + 1);
                iCurrent++;
            }
        }      

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            UpDataInDatabase();
            UpDataInWordsArray();
            listLearnWords.Remove(listLearnWords[iCurrent]);

            if (listLearnWords.Count() != 0)
            {
                if (iCurrent == listLearnWords.Count)
                    ShowWords(--iCurrent);
                else ShowWords(iCurrent);
            }
            else
            {
                LearningWordsWindow.Close();
                MessageBox.Show("Congratulations! You learned all the words on the list.");
            }

        }

        private void UpDataInWordsArray()
        {
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].english == listLearnWords[iCurrent].english)
                    words[i].status = true;
            }
        }

        private void UpDataInDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.words SET status = @newValue WHERE EnglishWord='{listLearnWords[iCurrent].english}'";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@newValue", true);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected < 1)
                        MessageBox.Show($"An error occurred while changing a status of word." + "\nPlease contact the admin!");
                }
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window exerciseWindow = new Exercise(words, user);
            exerciseWindow.Show();
            LearningWordsWindow.Close();
        }


    }
}
