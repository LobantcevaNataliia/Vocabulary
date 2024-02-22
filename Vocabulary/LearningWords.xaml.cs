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
                if (!words[i].Status)
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
            textBlockEnglish.Text = listLearnWords[i].English;
            textBlockTranscription.Text = listLearnWords[i].Transcription;
            textBlockUkrainian.Text = listLearnWords[i].Ukrainian;
            progressBarStatus.Value = (i + 1) * 100 / listLearnWords.Count;
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
            MessageBox.Show(DatabaseMethods.ChangeStatusWordInDatabase(words[iCurrentword].Id, true, user.Id));
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
                if (words[i].English == listLearnWords[iCurrent].English)
                {
                    words[i].Status = true;
                    iCurrentword = i;
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
