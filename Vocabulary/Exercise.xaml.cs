using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Vocabulary
{
    /// <summary>
    /// Логика взаимодействия для Exercise.xaml
    /// </summary>
    public partial class Exercise : Window
    {
        ObservableCollection<Word> words;

        public Exercise(ObservableCollection<Word> words)
        {
            InitializeComponent();
            this.words = words;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
        }

        private void Translation_Click(object sender, RoutedEventArgs e)
        {
            Window translation = new Translation(words);
            translation.Show();
            ExerciseWindow.Hide();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            Window learn = new LearningWords(words);
            learn.Show();
            ExerciseWindow.Hide();
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            Window repeat = new RepeatWords(words);
            repeat.Show();
            ExerciseWindow.Hide();
        }

        private void ExerciseWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            ExerciseWindow.Close();
        }
    }
}
