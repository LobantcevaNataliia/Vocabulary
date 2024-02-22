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
                newEnglishTb.Text = " ";
                newTranscriptionTb.Text = " ";
                newUkrainianTb.Text = " ";
            }
            else MessageBox.Show("You need to fill all lines to add new word!");
        }

        private void DownloadWords_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxA.IsChecked == true)
                MessageBox.Show(DatabaseMethods.AddWords("A", ref words, user.Id));
            if (checkBoxB.IsChecked == true)
                MessageBox.Show(DatabaseMethods.AddWords("B", ref words, user.Id));
            if (checkBoxC.IsChecked == true)
                MessageBox.Show(DatabaseMethods.AddWords("C", ref words, user.Id));
          
           // AuthorizationWindow.Close();
        }

        private void WorkWithNewWords(string newEnglish, string newTranscription, string newUkrainian, bool newStatus)
        {
            int id = DatabaseMethods.GetIdNewWord();
            words.Add(new Word(id, newEnglish, newTranscription, newUkrainian, newStatus, Level.U));

            //newEnglish = Change(newEnglish.Trim());
            //newTranscription = newTranscription.Trim();
            //newUkrainian = Change(newUkrainian.Trim());

            if (!WordExists(words[words.Count - 1].English))
            {
                //int id = DatabaseMethods.GetIdNewWord();
                MessageBox.Show(DatabaseMethods.InsertWordIntoDatabase(words[words.Count - 1]));
                //words.Add(new Word(id, newEnglish, newTranscription, newUkrainian, newStatus, Level.U));
                MessageBox.Show(DatabaseMethods.InsertDependenceIntoDatabase(words[words.Count - 1], user.Id));

            //    ListOfWords l = new ListOfWords(words,user);
            //    l.ListWords.Items.Add(new { i = words.Count, words[words.Count - 1].English, words[words.Count - 1].Transcription, words[words.Count - 1].Ukrainian, words[words.Count - 1].Status });
            }
            else
            {
                MessageBox.Show($"The word '{words[words.Count - 1].English}' already exists!");
                words.RemoveAt(words.Count - 1);
            }
        }

        //private string Change(string str)
        //{
        //    if (str.Length > 1)
        //        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        //    else return str.ToUpper();
        //}

        private bool WordExists(string newEnglish)
        {
            bool exist = false;

            for (int i = 0; i < words.Count - 1; i++)
                if (words[i].English == newEnglish)
                    exist = true;

            return exist;
        }

        private void AddWordsWindow_Closed(object sender, EventArgs e)
        {
            Window listOfWordsWindow = new ListOfWords(words,user);
            listOfWordsWindow.Show();
            AddWordsWindow.Close();
        }
    }
}
