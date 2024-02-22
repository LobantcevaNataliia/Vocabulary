using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

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
    }
}
