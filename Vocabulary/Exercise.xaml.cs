using System;
using System.Collections.Generic;
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
        public List<Word> list = new List<Word>();
        static string currDir = Environment.CurrentDirectory.ToString();
        string FilePath;

        public Exercise(List<Word> list, string FilePath)
        {
            InitializeComponent();
            this.list = list;
            this.FilePath = FilePath;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
        }

        private void Translation_Click(object sender, RoutedEventArgs e)
        {
            Window translation = new Translation(list, FilePath);
            translation.Show();
            ExerciseWindow.Hide();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            Window learn = new LearningWords(list, FilePath);
            learn.Show();
            ExerciseWindow.Hide();
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            //    Window exercise = new Exercise(listWords, FilePath);
            //    exercise.Show();
            //    ExerciseWindow.Hide();
        }

        private void ExerciseWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            ExerciseWindow.Close();
        }
    }
}
