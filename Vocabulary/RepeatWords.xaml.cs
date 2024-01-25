using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<Word> listRepeatWords;
        int iCurrent;

        public RepeatWords(ObservableCollection<Word> words)
        {
            InitializeComponent();
            this.words = words;
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
                labelEnglish.Content = listRepeatWords[0].english;
                labelTranscription.Content = listRepeatWords[0].transcription;
                labelUkrainian.Content = listRepeatWords[0].ukrainian;
            }
            else MessageBox.Show("Unfortunately, you don't have learned words yet!");
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent > 0)
            {
                labelEnglish.Content = listRepeatWords[iCurrent - 1].english;
                labelTranscription.Content = listRepeatWords[iCurrent - 1].transcription;
                labelUkrainian.Content = listRepeatWords[iCurrent - 1].ukrainian;
                iCurrent--;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent < listRepeatWords.Count - 1)
            {
                labelEnglish.Content = listRepeatWords[iCurrent + 1].english;
                labelTranscription.Content = listRepeatWords[iCurrent + 1].transcription;
                labelUkrainian.Content = listRepeatWords[iCurrent + 1].ukrainian;
                iCurrent++;
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            UpDataInDatabase();
            words[iCurrent].status = false;
            listRepeatWords.Remove(listRepeatWords[iCurrent]);
            iCurrent++;
            Next_Click(sender, e);

        }

        private void UpDataInDatabase()
        {
            string connectionString = "server=localhost;user=NataliiaLobantseva;database=myVocabDB;port=3306;password=!23Asdfgh"; ;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.words SET status = @newValue WHERE EnglishWord='{listRepeatWords[iCurrent].english}'";

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
            Window exerciseWindow = new Exercise(words);
            exerciseWindow.Show();
            RepeatWordsWindow.Close();
        }

        
    }
}
