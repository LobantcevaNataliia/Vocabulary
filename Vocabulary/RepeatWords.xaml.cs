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
using MySql.Data.MySqlClient;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for RepeatWords.xaml
    /// </summary>
    public partial class RepeatWords : Window
    {
        ObservableCollection<Word> words;
        User user;
        public List<Word> listRepeatWords;
        int iCurrent, iCurrentword;

        public RepeatWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listRepeatWords = new List<Word>();
            for (int i = 0; i < words.Count; i++)
                if (words[i].Status)
                    listRepeatWords.Add(words[i]);

            if (listRepeatWords.Count() != 0)
            {
                iCurrent = 0;
                ShowWords(iCurrent);
            }
            else
            {
                MessageBox.Show("Unfortunately, you don't have learned words yet!");
                RepeatWordsWindow.Close();
            }
        }

        private void ShowWords(int i) 
        {
            textBlockEnglish.Text = listRepeatWords[i].English;
            textBlockTranscription.Text = listRepeatWords[i].Transcription;
            textBlockUkrainian.Text = listRepeatWords[i].Ukrainian;
            progressBarStatus.Value = (i + 1) * 100 / listRepeatWords.Count;
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
            if (iCurrent < listRepeatWords.Count - 1)
            {
                ShowWords(iCurrent + 1);
                iCurrent++;
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            UpDataInWordsArray();
            MessageBox.Show(DatabaseMethods.ChangeStatusWordInDatabase(words[iCurrentword].Id, false, user.Id));
            listRepeatWords.Remove(listRepeatWords[iCurrent]);

            if (listRepeatWords.Count() != 0)
            {
                if (iCurrent == listRepeatWords.Count)
                    ShowWords(--iCurrent);
                else ShowWords(iCurrent);
            }
            else
            {
                RepeatWordsWindow.Close();
                MessageBox.Show("Unfortunately, you don't have learned words yet!");      
            }
        }

        private void UpDataInWordsArray()
        {
            for(int i = 0; i < words.Count; i++)
            {
                if(words[i].English == listRepeatWords[iCurrent].English)
                {
                    words[i].Status = false;
                    iCurrentword = i;
                }
                    
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window exerciseWindow = new Exercise(words, user);
            exerciseWindow.Show();
            RepeatWordsWindow.Close();
        }

        
    }
}
