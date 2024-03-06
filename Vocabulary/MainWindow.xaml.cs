using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Net;

namespace Vocabulary
{
    public partial class MainWindow : Window
    {
        User user;
        ObservableCollection<Word> words;

        public MainWindow()
        {
            InitializeComponent();
            user = DatabaseMethods.GetDefaultUser();
            DatabaseMethods.LoadDataFromDatabase(user.Id, out words);

        }

        public MainWindow(User user)
        {
            InitializeComponent();
            if (user != null)
                this.user = user;
            else user = DatabaseMethods.GetDefaultUser();
            DatabaseMethods.LoadDataFromDatabase(user.Id, out words);
        }
            
        //Метод для відображення списку слів
        private void List_Click(object sender, RoutedEventArgs e)
        {
            Window fullList = new ListOfWords(words, user);
            fullList.Show();
            MainWindowWindow.Hide();
        }

        //Метод для переходy до вибору вправ
        private void Exercise_Click(object sender, RoutedEventArgs e)
        {
            Window exercise = new Exercise(words, user);
            exercise.Show();
            MainWindowWindow.Hide();
        }

        //Метод для авторизації
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            Window auto = new Authorization();
            auto.Show();
            MainWindowWindow.Hide();
        }

        private void Download3000()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            words = new ObservableCollection<Word>();
            
            using (StreamReader sr = new StreamReader(openFileDialog.FileName, System.Text.Encoding.UTF8))
            {
                while (sr.Peek() >= 0)
                {
                    string lineCurrent = sr.ReadLine();
                    string[] newword = lineCurrent.Split('\\');
                    WorkWithNewWords(newword[0], newword[2], newword[3], (Level)Enum.Parse(typeof(Level), newword[4]));

                }
            }
            

            void WorkWithNewWords(string newEnglish, string newTranscription, string newUkrainian, Level newLevel)
            {
                int id = DatabaseMethods.GetIdNewWord();
                words.Add(new Word(id, newEnglish, newTranscription, newUkrainian, false, newLevel));
                DatabaseMethods.InsertWordIntoDatabase(words[words.Count - 1]);
            }
        }
    }
}
