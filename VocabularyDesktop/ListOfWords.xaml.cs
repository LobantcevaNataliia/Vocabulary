using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;
using System.IO;
using VocabularyLibrary;

namespace VocabularyDesktop
{
    /// <summary>
    /// Логика взаимодействия для ListOfWords.xaml
    /// </summary>
    public partial class ListOfWords : Window
    {
        ObservableCollection<Word> words;
        User user;

        public ListOfWords(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;

            for (int i = 1; i <= words.Count; i++)
                ListWords.Items.Add(new { i, words[i - 1].English, words[i - 1].Transcription, words[i - 1].Ukrainian, words[i - 1].Status, words[i - 1].Level });

        }

        private void ListOfWordsWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            ListOfWordsWindow.Close();
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            AddWords addWindow = new AddWords(words, user);
            addWindow.addWordCheckBoxes.Visibility = Visibility.Hidden;
            addWindow.addWordGrid.Visibility = Visibility.Visible;
            addWindow.Show();
            ListOfWordsWindow.Hide();
        }

        private void Add3000_Click(object sender, RoutedEventArgs e)
        {
            AddWords addWindow = new AddWords(words, user);
            addWindow.addWordCheckBoxes.Visibility = Visibility.Visible;
            addWindow.addWordGrid.Visibility = Visibility.Hidden;
            addWindow.Show();
            ListOfWordsWindow.Hide();
        }

        private void DownloadWords_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            try
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName, System.Text.Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string? lineCurrent = sr.ReadLine();
                        // Перевірка на порожній рядок
                        if (!string.IsNullOrEmpty(lineCurrent))
                        {
                            string[] newword = lineCurrent.Split('-');
                            if (newword.Length == 4)
                                WorkWithNewWords(newword[0], newword[1], newword[2], Convert.ToBoolean(Convert.ToInt16(newword[3])));
                            else
                                MessageBox.Show("You need to ...");
                        }
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
                MessageBox.Show(DatabaseMethods.InsertDependenceIntoDatabase(words[words.Count - 1],user.Id));

                ListWords.Items.Add(new { i = words.Count, words[words.Count - 1].English, words[words.Count - 1].Transcription, words[words.Count - 1].Ukrainian, words[words.Count - 1].Status, words[words.Count - 1].Level });
            }
            else
            {
                MessageBox.Show($"The word '{newEnglish}' already exists!");
                words.RemoveAt(words.Count - 1);
            }
        }

        private bool WordExists(string newEnglish)
        {
            bool exist = false;

            for (int i = 0; i < words.Count - 1; i++)
                if (words[i].English == newEnglish)
                    exist = true;

            return exist;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Отримання DataGridCellInfo обраної ячейки
            DataGridCellInfo cellInfo = ListWords.SelectedCells.FirstOrDefault();

            // Отримання значення ячейки
            int cellValue = Convert.ToInt16(cellInfo.Item.GetType().GetProperty(cellInfo.Column.SortMemberPath)?.GetValue(cellInfo.Item, null));

            int id = words[cellValue - 1].Id;

            MessageBox.Show(DatabaseMethods.DeleteDependenceFromDatabase(id, user.Id));
            if(words[cellValue - 1].Level == Level.U)
                MessageBox.Show(DatabaseMethods.DeleteWordFromDatabase(id));

            words.RemoveAt(cellValue - 1);
            ListWords.Items.Clear();
            for (int i = 1; i <= words.Count; i++)
                ListWords.Items.Add(new { i, words[i - 1].English, words[i - 1].Transcription, words[i - 1].Ukrainian, words[i - 1].Status, words[i - 1].Level });


        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Отримання DataGridCellInfo обраної ячейки
            DataGridCellInfo cellInfo = ListWords.SelectedCells.FirstOrDefault();

            // Отримання значення ячейки
            int cellValue = Convert.ToInt16(cellInfo.Item.GetType().GetProperty(cellInfo.Column.SortMemberPath)?.GetValue(cellInfo.Item, null));

            int id = words[cellValue - 1].Id;

            if (!words[cellValue - 1].Status)
                words[cellValue - 1].Status = true;
            else words[cellValue - 1].Status = false;

            MessageBox.Show(DatabaseMethods.ChangeStatusWordInDatabase(id, words[cellValue - 1].Status, user.Id));
            ListWords.Items[cellValue - 1] = new { i = cellValue, words[cellValue - 1].English, words[cellValue - 1].Transcription, words[cellValue - 1].Ukrainian, words[cellValue - 1].Status, words[cellValue - 1].Level };

        } 
    }
}
