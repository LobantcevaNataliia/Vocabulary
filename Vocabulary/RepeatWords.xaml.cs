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
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
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
                if (words[i].status)
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
            textBlockEnglish.Text = listRepeatWords[i].english;
            textBlockTranscription.Text = listRepeatWords[i].transcription;
            textBlockUkrainian.Text = listRepeatWords[i].ukrainian;
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
            UpDataInDatabase();
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
                if(words[i].english == listRepeatWords[iCurrent].english)
                {
                    words[i].status = false;
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
                    cmd.Parameters.AddWithValue("@newValue", false);

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
            RepeatWordsWindow.Close();
        }

        
    }
}
