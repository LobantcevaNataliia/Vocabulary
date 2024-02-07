using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Vocabulary
{
    public partial class LearningWords : Window
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        ObservableCollection<Word> words;
        User user;
        public List<Word> listLearnWords;
        int iCurrent, iCurrentword;

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
            textBlockEnglish.Text = listLearnWords[i].english;
            textBlockTranscription.Text = listLearnWords[i].transcription;
            textBlockUkrainian.Text = listLearnWords[i].ukrainian;
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
            UpDataInWordsArray();
            UpDataInDatabase();
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
                {
                    words[i].status = true;
                    iCurrentword = i;
                }
            }
        }

        private void UpDataInDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.learnedwords SET status = @newValue WHERE WordId='{words[iCurrentword].id}' AND UserId='{user.id}'";

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
