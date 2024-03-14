using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using VocabularyLibrary;

namespace VocabularyDesktop
{
    public partial class LearningWords : Window
    {
        ObservableCollection<Word> words;
        User user;
        public List<Word> fullListRepeatWords, listLearnWords;
        int iCurrent, iCurrentword;

        public LearningWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            showWordUC.textBlockTranscription.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0xD8, 0xAA));
            this.words = words;
            this.user = user;
        }     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateFullListWords();
            listLearnWords = fullListRepeatWords;

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

        private void Window_Closed(object sender, EventArgs e)
        {
            Window exerciseWindow = new Exercise(words, user);
            exerciseWindow.Show();
            LearningWordsWindow.Close();
        }

        private void ShowWords(int i)
        {
            showWordUC.textBlockEnglish.Text = listLearnWords[i].English;
            showWordUC.textBlockTranscription.Text = listLearnWords[i].Transcription;
            showWordUC.textBlockUkrainian.Text = listLearnWords[i].Ukrainian;
            showWordUC.progressBarStatus.Value = (i + 1) * 100 / listLearnWords.Count;
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

        private void CreateFullListWords()
        {
            fullListRepeatWords = new List<Word>();
            for (int i = 0; i < words.Count; i++)
                if (!words[i].Status)
                    fullListRepeatWords.Add(words[i]);
        }

        // Обробка зміни вибраного RadioButton в ShowWordUC
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CreateFullListWords();
            listLearnWords = new List<Word>();

            if (showWordUC.rBAll.IsChecked == true)
            {
                listLearnWords = fullListRepeatWords;
            }
            if (showWordUC.rBMy.IsChecked == true)
            {
                if (fullListRepeatWords.Count >= 5)
                    for (int i = 0; i < 5; i++)
                        listLearnWords.Add(fullListRepeatWords[i]);
                else
                {
                    MessageBox.Show("You have less words!");
                    showWordUC.rBAll.IsChecked = true;
                }
            }
            if (showWordUC.rBUser.IsChecked == true)
            {
                if (fullListRepeatWords.Count >= Convert.ToInt16(showWordUC.rBTextUser.Text))
                    for (int i = 0; i < Convert.ToInt16(showWordUC.rBTextUser.Text); i++)
                        listLearnWords.Add(fullListRepeatWords[i]);
                else
                {
                    MessageBox.Show("You have less words!");
                    showWordUC.rBAll.IsChecked = true;
                }
            }

            iCurrent = 0;
            ShowWords(iCurrent);
            showWordUC.expander.IsExpanded = false;
        }

    }
}
