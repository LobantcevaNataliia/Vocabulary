using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VocabularyLibrary;

namespace VocabularyDesktop
{
    /// <summary>
    /// Логика взаимодействия для Exercise.xaml
    /// </summary>
    public partial class Exercise : Window
    {
        ObservableCollection<Word> words;
        User user;

        public Exercise(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
        }

        private void Translation_Click(object sender, RoutedEventArgs e)
        {
            Window translation = new Translation(words, user);
            translation.Show();
            ExerciseWindow.Hide();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            Window learn = new LearningWords(words, user);
            learn.Show();
            ExerciseWindow.Hide();
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            Window repeat = new RepeatWords(words, user);
            repeat.Show();
            ExerciseWindow.Hide();
        }

        private void ExerciseWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            ExerciseWindow.Close();
        }
    }
}
