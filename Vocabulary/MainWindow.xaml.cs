using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Vocabulary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Work();
        }

        public List<Word> listWords = new List<Word>();
        string FilePath;
        static string currDir = Environment.CurrentDirectory.ToString();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Work();
        }

        public void Work()
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //    FilePath = openFileDialog.FileName;
            //else FilePath = currDir + @"\test1.txt";
            FilePath = currDir + @"\v.txt";
            try
            {
                using (StreamReader sr = new StreamReader(FilePath, Encoding.Default))
                {
                    while (sr.Peek() >= 0)
                    {
                        string lineCurrent = sr.ReadLine();
                        if (lineCurrent == "") continue;
                        string[] words = lineCurrent.Split('-');
                        listWords.Add(new Word(words[0], words[1], Convert.ToBoolean(Convert.ToInt32(words[2]))));

                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка считывания. Будет загружен документ \"test1.txt\" ");
                //DownloadTest();
            }
        }
        private void Auto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            Window fullList = new ListOfWords(listWords, FilePath);
            fullList.Show();
            MainWindowWindow.Hide();
        }

        private void Exercise_Click(object sender, RoutedEventArgs e)
        {
            Window exercise = new Exercise(listWords, FilePath);
            exercise.Show();
            MainWindowWindow.Hide();
        }
    }
}
