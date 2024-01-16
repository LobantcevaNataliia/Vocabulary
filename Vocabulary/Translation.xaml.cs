using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Vocabulary
{
    public partial class Translation : Window
    {
        ObservableCollection<Word> words;

        int maxCount = 3, minCount = 0, countEx, iEx, amountEx;
        int[] indexArray = new int[4];
        int[] varArray = new int[4];

        public Translation(ObservableCollection<Word> words)
        {
            InitializeComponent();
            this.words = words;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random ran = new Random();
            iEx = ran.Next(words.Count);
            indexArray[0] = iEx;
            Task.Content = words[iEx].english;

            bool repeat = false;
            int num = 0;
            for (int k = 1; k < indexArray.Length; k++)
            {
                do
                {
                    repeat = false;
                    ran = new Random();
                    num = ran.Next(words.Count);
                    for (int i = 0; i < indexArray.Length; i++)
                        if (indexArray[i] == num)
                            repeat = true;
                }
                while (repeat);

                indexArray[k] = num;
            }

            num = 0;
            for (int k = 0; k < indexArray.Length; k++)
            {
                do
                {
                    repeat = false;
                    ran = new Random();
                    num = ran.Next(indexArray.Length + 1);
                    for (int i = 0; i < indexArray.Length; i++)
                        if (varArray[i] == num)
                            repeat = true;
                }
                while (repeat);

                varArray[k] = num;
            }

            Var1.Content = words[indexArray[varArray[0] - 1]].ukrainian;
            Var2.Content = words[indexArray[varArray[1] - 1]].ukrainian;
            Var3.Content = words[indexArray[varArray[2] - 1]].ukrainian;
            Var4.Content = words[indexArray[varArray[3] - 1]].ukrainian;

        }

        private void Answer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MarkR.Visibility = Visibility.Visible;
            MarkW.Visibility = Visibility.Visible;

            if (countEx == 2)
            {
                RightAnswer.Visibility = Visibility.Visible;
                RightAnswer.Content = words[iEx].english;

                if (Equels(iEx, Answer.Text))
                    Answer.Foreground = Brushes.Green;
                else Answer.Foreground = Brushes.Red;
            }
            if (countEx == 3)
            {
                RightAnswer.Visibility = Visibility.Visible;
                RightAnswer.Content = words[iEx].ukrainian;

                if (Equels(iEx, Answer.Text))
                    Answer.Foreground = Brushes.Green;
                else Answer.Foreground = Brushes.Red;
            }
        }

        private void MarkW_Click(object sender, RoutedEventArgs e)
        {
            ChancheStatus(iEx, false);
            NewTask();
        }

        private void MarkR_Click(object sender, RoutedEventArgs e)
        {
            ChancheStatus(iEx, true);
            NewTask();
        }

        public void ChancheStatus(int i, bool status)
        {
            //list[i].status = status;

            //using (StreamWriter sw = new StreamWriter(FilePath, false, System.Text.Encoding.Default))
            //    for (int k = 0; k < list.Count; k++)
            //        sw.WriteLine(list[k].english + "-" + list[k].ukrainian + "-" + Convert.ToInt16(list[k].status));
        }

        private void NewTask()
        {
            indexArray = new int[4];
            varArray = new int[4];

            if (countEx == 0)
            {
                Var1.Visibility = Visibility.Visible;
                Var2.Visibility = Visibility.Visible;
                Var3.Visibility = Visibility.Visible;
                Var4.Visibility = Visibility.Visible;

                Answer.Visibility = Visibility.Hidden;
                MarkR.Visibility = Visibility.Hidden;
                MarkW.Visibility = Visibility.Hidden;
                RightAnswer.Visibility = Visibility.Hidden;

                Random ran = new Random();
                iEx = ran.Next(words.Count);
                indexArray[0] = iEx;
                Task.Content = words[iEx].english;

                bool repeat = false;
                int num = 0;
                for (int k = 1; k < indexArray.Length; k++)
                {
                    do
                    {
                        repeat = false;
                        ran = new Random();
                        num = ran.Next(words.Count);
                        for (int i = 0; i < indexArray.Length; i++)
                            if (indexArray[i] == num)
                                repeat = true;
                    }
                    while (repeat);

                    indexArray[k] = num;
                }

                num = 0;
                for (int k = 0; k < indexArray.Length; k++)
                {
                    do
                    {
                        repeat = false;
                        ran = new Random();
                        num = ran.Next(indexArray.Length + 1);
                        for (int i = 0; i < indexArray.Length; i++)
                            if (varArray[i] == num)
                                repeat = true;
                    }
                    while (repeat);

                    varArray[k] = num;
                }

                Var1.Content = words[indexArray[varArray[0] - 1]].ukrainian;
                Var2.Content = words[indexArray[varArray[1] - 1]].ukrainian;
                Var3.Content = words[indexArray[varArray[2] - 1]].ukrainian;
                Var4.Content = words[indexArray[varArray[3] - 1]].ukrainian;
            }
            if (countEx == 1)
            {
                Var1.Visibility = Visibility.Visible;
                Var2.Visibility = Visibility.Visible;
                Var3.Visibility = Visibility.Visible;
                Var4.Visibility = Visibility.Visible;

                Answer.Visibility = Visibility.Hidden;
                MarkR.Visibility = Visibility.Hidden;
                MarkW.Visibility = Visibility.Hidden;
                RightAnswer.Visibility = Visibility.Hidden;

                Random ran = new Random();
                iEx = ran.Next(words.Count);
                indexArray[0] = iEx;
                Task.Content = words[iEx].ukrainian;

                bool repeat = false;
                int num = 0;
                for (int k = 1; k < indexArray.Length; k++)
                {
                    do
                    {
                        repeat = false;
                        ran = new Random();
                        num = ran.Next(words.Count);
                        for (int i = 0; i < indexArray.Length; i++)
                            if (indexArray[i] == num)
                                repeat = true;
                    }
                    while (repeat);

                    indexArray[k] = num;
                }

                num = 0;
                for (int k = 0; k < indexArray.Length; k++)
                {
                    do
                    {
                        repeat = false;
                        ran = new Random();
                        num = ran.Next(indexArray.Length + 1);
                        for (int i = 0; i < indexArray.Length; i++)
                            if (varArray[i] == num)
                                repeat = true;
                    }
                    while (repeat);

                    varArray[k] = num;
                }

                Var1.Content = words[indexArray[varArray[0] - 1]].english;
                Var2.Content = words[indexArray[varArray[1] - 1]].english;
                Var3.Content = words[indexArray[varArray[2] - 1]].english;
                Var4.Content = words[indexArray[varArray[3] - 1]].english;
            }
            if (countEx == 2)
            {
                NameOfExercise.Foreground = Brushes.Black;
                Var1.Visibility = Visibility.Hidden;
                Var2.Visibility = Visibility.Hidden;
                Var3.Visibility = Visibility.Hidden;
                Var4.Visibility = Visibility.Hidden;

                Answer.Visibility = Visibility.Visible;
                MarkR.Visibility = Visibility.Hidden;
                MarkW.Visibility = Visibility.Hidden;
                RightAnswer.Visibility = Visibility.Hidden;
                Answer.Text = "";
                Answer.Foreground = Brushes.Black;

                Random ran = new Random();
                iEx = ran.Next(words.Count);

                Task.Content = words[iEx].ukrainian;
            }
            if (countEx == 3)
            {
                NameOfExercise.Foreground = Brushes.Black;
                Var1.Visibility = Visibility.Hidden;
                Var2.Visibility = Visibility.Hidden;
                Var3.Visibility = Visibility.Hidden;
                Var4.Visibility = Visibility.Hidden;

                Answer.Visibility = Visibility.Visible;
                MarkR.Visibility = Visibility.Hidden;
                MarkW.Visibility = Visibility.Hidden;
                RightAnswer.Visibility = Visibility.Hidden;
                Answer.Text = "";
                Answer.Foreground = Brushes.Black;

                Random ran = new Random();
                iEx = ran.Next(words.Count);

                Task.Content = words[iEx].english;

            }

        }

        private void Var1_Click(object sender, RoutedEventArgs e)
        {
            if (Equels(iEx, Var1.Content.ToString()))
                NameOfExercise.Foreground = Brushes.Green;
            else NameOfExercise.Foreground = Brushes.Red;

            NewTask();
        }

        private void Var2_Click(object sender, RoutedEventArgs e)
        {
            if (Equels(iEx, Var2.Content.ToString()))
                NameOfExercise.Foreground = Brushes.Green;
            else NameOfExercise.Foreground = Brushes.Red;

            NewTask();
        }

        private void Var3_Click(object sender, RoutedEventArgs e)
        {
            if (Equels(iEx, Var3.Content.ToString()))
                NameOfExercise.Foreground = Brushes.Green;
            else NameOfExercise.Foreground = Brushes.Red;

            NewTask();
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TranslationWindow_Closed(object sender, EventArgs e)
        {
            Window exerciseWindow = new Exercise(words);
            exerciseWindow.Show();
            TranslationWindow.Close();
        }

        private void Var4_Click(object sender, RoutedEventArgs e)
        {
            if (Equels(iEx, Var4.Content.ToString()))
                NameOfExercise.Foreground = Brushes.Green;
            else NameOfExercise.Foreground = Brushes.Red;

            NewTask();
        }

        public bool Equels(int i, string str)
        {
            bool eq = false;
            string[] w1 = str.Split(' ', '-');
            string[] t1 = new string[1];
            if (countEx == 1 || countEx == 2)
                t1 = words[i].english.Split(' ', '-');
            if (countEx == 0 || countEx == 3)
                t1 = words[i].ukrainian.Split(' ', '-');

            List<string> answ = new List<string>();
            List<string> rightAnsw = new List<string>();

            for (int k = 0; k < w1.Length; k++)
                if (w1[k] != " " && w1[k] != "" && w1[k] != "-")
                    answ.Add(w1[k]);
            for (int k = 0; k < t1.Length; k++)
                if (t1[k] != " " && t1[k] != "" && t1[k] != "-")
                    rightAnsw.Add(t1[k]);

            for (int k = 0; k < answ.Count; k++)
                if (answ[k] == rightAnsw[k])
                    eq = true;
                else eq = false;

            return eq;
        }

        private void NextEx_Click(object sender, RoutedEventArgs e)
        {
            if (maxCount != countEx)
                countEx++;
            else countEx = 0;

            if (countEx == 0)
                NameOfExercise.Content = "Choose right English translation";
            if (countEx == 1)
                NameOfExercise.Content = "Choose right Ukrainian translation";
            if (countEx == 2)
                NameOfExercise.Content = "Translate to English";
            if (countEx == 3)
                NameOfExercise.Content = "Translate to Ukrainian";

            NewTask();
        }

        private void PrivEx_Click(object sender, RoutedEventArgs e)
        {
            if (minCount != countEx)
                countEx--;
            else countEx = 3;

            if (countEx == 0)
                NameOfExercise.Content = "Choose right English translation";
            if (countEx == 1)
                NameOfExercise.Content = "Choose right Ukrainian translation";
            if (countEx == 2)
                NameOfExercise.Content = "Translate to English";
            if (countEx == 3)
                NameOfExercise.Content = "Translate to Ukrainian";

            NewTask();
        }
    }
}
