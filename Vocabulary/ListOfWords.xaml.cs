using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text;

namespace Vocabulary
{
    /// <summary>
    /// Логика взаимодействия для ListOfWords.xaml
    /// </summary>
    public partial class ListOfWords : Window
    {
        //public List<string> list, list2;
        //public List<bool> list3;
        public List<Word> list = new List<Word>();
        static string currDir = Environment.CurrentDirectory.ToString();
        string FilePath;

        public ListOfWords(List<Word> list, string FilePath)
        {
            InitializeComponent();
            this.list = list;
            this.FilePath = FilePath;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            list = new List<Word>();
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //    FilePath = openFileDialog.FileName;
            //else FilePath = currDir + @"\test1.txt";

            try
            {
                using (StreamReader sr = new StreamReader(FilePath, Encoding.Default))
                {
                    while (sr.Peek() >= 0)
                    {
                        string lineCurrent = sr.ReadLine();
                        if (lineCurrent == "") continue;
                        string[] words = lineCurrent.Split('-');
                        list.Add(new Word(words[0], words[1], Convert.ToBoolean(Convert.ToInt32(words[2]))));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка считывания. Будет загружен документ \"test1.txt\" ");
                //DownloadTest();
            }

            list = SortForABC();
            for (int i = 0; i < list.Count; i++)
                CreateItem(i, list[i].english, list[i].russian, list[i].status);
        }


        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            string newW = newWord.Text;
            string newT = newTr.Text;
            bool newStatus = false;

            if ((bool)radioButtonKnown.IsChecked)
                newStatus = true;


            using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
                 sw.WriteLine(newW + "-" + newT + "-" + Convert.ToInt16(newStatus));

            CreateItem(ListWords.Items.Count, newW, newT, newStatus);
            //Gr = new Grid();
            //Gr.ColumnDefinitions.Add(new ColumnDefinition());
            //Gr.ColumnDefinitions.Add(new ColumnDefinition());
            //Gr.RowDefinitions.Add(new RowDefinition());
            //Gr.RowDefinitions.Add(new RowDefinition());
            //n = new TextBlock() { Text = (ListWords.Items.Count + 1).ToString() };
            //w = new TextBlock() { Text = word[0].ToString() };
            //t = new TextBlock() { Text = word[1].ToString() };

            //Image imRight = new Image();
            //Image imWrong = new Image();
            //imRight.Source = new BitmapImage(new Uri("C:/Users/user/Desktop/K.bmp"));
            //imWrong.Source = new BitmapImage(new Uri("C:/Users/user/Desktop/K2.bmp"));
            //if (word[2] == "1")
            //{
            //    Grid.SetRow(imRight, 1);
            //    Grid.SetColumn(imRight, 0);
            //    Gr.Children.Add(imRight);
            //}
            //else
            //{
            //    Grid.SetRow(imWrong, 1);
            //    Grid.SetColumn(imWrong, 0);
            //    Gr.Children.Add(imWrong);
            //}

            //Grid.SetRow(n, 0);
            //Grid.SetColumn(n, 0);
            //Grid.SetRow(w, 0);
            //Grid.SetColumn(w, 1);
            //Grid.SetRow(t, 1);
            //Grid.SetColumn(t, 1);
            //Gr.Children.Add(n);
            //Gr.Children.Add(w);
            //Gr.Children.Add(t);
            //ListWords.Items.Add(Gr);
        }

        public void CreateItem(int i, string str1, string str2,bool status)
        {
            Gr = new Grid();
            Gr.ColumnDefinitions.Add(new ColumnDefinition());
            Gr.ColumnDefinitions.Add(new ColumnDefinition());
            Gr.ColumnDefinitions.Add(new ColumnDefinition());

            Grid gr1 = new Grid();
            gr1.RowDefinitions.Add(new RowDefinition());
            gr1.RowDefinitions.Add(new RowDefinition());

            n = new TextBlock()
            {
                Text = (i + 1).ToString(),
                Margin = new Thickness(10),
                FontFamily = new FontFamily("Stencil"),
                FontSize = 18
            };

            w = new TextBlock()
            {
                Text = str1.ToString(),
                Margin = new Thickness(10, 5, 20, 0),
                FontFamily = new FontFamily("Stencil"),
                FontSize = 16
            };

            t = new TextBlock()
            {
                Text = str2.ToString(),
                Margin = new Thickness(10, 0, 40, 5),
                FontFamily = new FontFamily("Century"),
                FontSize = 14
            };

            Image imRight = new Image() { Margin = new Thickness(10) };
            Image imWrong = new Image() { Margin = new Thickness(10) };
            imRight.Source = new BitmapImage(new Uri("D:/Киця/My phone_09_11_2023/Файли/Проги/Vocabulary/K.bmp"));
            imWrong.Source = new BitmapImage(new Uri("D:/Киця/My phone_09_11_2023/Файли/Проги/Vocabulary/K2.bmp"));
            if (status)
            {
                Grid.SetColumn(imRight, 2);
                Gr.Children.Add(imRight);
            }
            else
            {
                Grid.SetColumn(imWrong, 2);
                Gr.Children.Add(imWrong);
            }

            Grid.SetColumn(n, 0);
            Gr.Children.Add(n);

            Grid.SetRow(w, 0);
            Grid.SetRow(t, 1);
            gr1.Children.Add(t);
            gr1.Children.Add(w);
            Grid.SetColumn(gr1, 1);
            Gr.Children.Add(gr1);

            gr1.Width = ListWords.ActualWidth - n.ActualWidth - imRight.ActualWidth - 100;

            ListWords.Items.Add(Gr);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //Window settings = new Window();
           // Grid gridSet = new Grid();

        }

        public List<Word> SortForRight()
        {
            List<Word> sorted = list;
            Word tmp;
            for (int k = 0; k < sorted.Count; k++)
            {
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    if (!sorted[i].status)
                    {
                        tmp = sorted[i];
                        sorted[i] = sorted[i + 1];
                        sorted[i + 1] = tmp;
                    }
                }
            }

            return sorted;
        }

        public List<Word> SortForWrong()
        {
            List<Word> sorted = list;
            Word tmp;
            for (int k = 0; k < sorted.Count; k++)
            {
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    if (sorted[i].status)
                    {
                        tmp = sorted[i];
                        sorted[i] = sorted[i + 1];
                        sorted[i + 1] = tmp;
                    }
                }
            }

            return sorted;
        }

        public List<Word> SortForABC()
        {
            List<Word> sorted = list; ;
            Word tmp;
            char[] ch1, ch2;
            for (int k = 0; k < sorted.Count; k++)
            {
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    ch1 = sorted[i].english.ToCharArray();
                    ch2 = sorted[i + 1].english.ToCharArray();

                    for (int t = 0; t < ch1.Length; t++)
                    {
                        if (ch1[t] > ch2[t])
                        {
                            tmp = sorted[i];
                            sorted[i] = sorted[i + 1];
                            sorted[i + 1] = tmp;
                        }
                        break;
                    }
                }
            }
            return sorted;
        }
    }
}
