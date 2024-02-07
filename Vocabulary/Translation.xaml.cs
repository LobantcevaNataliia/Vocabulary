using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace Vocabulary
{
    public partial class Translation : Window
    {
        ObservableCollection<Word> words;
        User user;
        DispatcherTimer timer;
        Random random;

        int maxExercise = 3, minExercise = 0, currentExercise = 0, indexOfTaskWord;     
        string rightAnswer;
        
        public Translation(ObservableCollection<Word> words, User user)
        {
            InitializeComponent();
            this.words = words;
            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewTask();
        }

        private void TranslationWindow_Closed(object sender, EventArgs e)
        {
            Window exerciseWindow = new Exercise(words, user);
            exerciseWindow.Show();
            TranslationWindow.Close();
        }

        private void NewTask()
        {
            int[] indexOfWords;
            random = new Random();
            indexOfTaskWord = random.Next(words.Count);
            TextBlock[] buttons = { Var1, Var2, Var3, Var4 };
          
            if (currentExercise == 0)
            {
                VisibilityFirstTypeExercise();
                indexOfWords = CreateArrayOfWords();

                Task.Content = words[indexOfTaskWord].english;
                for (int i = 0; i < 4; i++)
                    buttons[i].Text = words[indexOfWords[i]].ukrainian;
            }
            if (currentExercise == 1)
            {
                VisibilityFirstTypeExercise();
                indexOfWords = CreateArrayOfWords();

                Task.Content = words[indexOfTaskWord].ukrainian;
                for (int i = 0; i < 4; i++)
                    buttons[i].Text = words[indexOfWords[i]].english;
            }
            if (currentExercise == 2)
            {
                VisibilitySecondTypeExercise();
                Task.Content = words[indexOfTaskWord].ukrainian;
            }
            if (currentExercise == 3)
            {
                VisibilitySecondTypeExercise();
                Task.Content = words[indexOfTaskWord].english;
            }
        }

        private int[] CreateArrayOfWords() 
        {
            int num;
            int[] indexOfWords = new int[4];
            indexOfWords[0] = indexOfTaskWord;

            for (int i = 1; i < indexOfWords.Length; i++)
            {
                do
                {
                    num = random.Next(words.Count);
                }
                while (indexOfWords.Contains(num));

                indexOfWords[i] = num;
            }

            random = new Random();
            int[] newOrder = indexOfWords.OrderBy(x => random.Next()).ToArray();

            return newOrder;
        }

        private void VisibilityFirstTypeExercise() 
        {
            Var1.Visibility = Visibility.Visible;
            Var2.Visibility = Visibility.Visible;
            Var3.Visibility = Visibility.Visible;
            Var4.Visibility = Visibility.Visible;

            Answer.Visibility = Visibility.Hidden;
            ResultSmile.Visibility = Visibility.Hidden;
            ResultAnswer.Visibility = Visibility.Hidden;
            CheckAnswer.Visibility = Visibility.Hidden;
            RightAnswer.Visibility = Visibility.Hidden;

        }
        
        private void VisibilitySecondTypeExercise()
        {
            Var1.Visibility = Visibility.Hidden;
            Var2.Visibility = Visibility.Hidden;
            Var3.Visibility = Visibility.Hidden;
            Var4.Visibility = Visibility.Hidden;

            Answer.Visibility = Visibility.Visible;
            ResultSmile.Visibility = Visibility.Hidden;
            ResultAnswer.Visibility = Visibility.Hidden;
            CheckAnswer.Visibility = Visibility.Visible;
            RightAnswer.Visibility = Visibility.Hidden;
            Answer.Text = "";
        }

        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer.Visibility = Visibility.Hidden;
            ShowResult(Answer.Text);
            
        }

        public void ShowResult(string userAnswer)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

            if (Equels(indexOfTaskWord, userAnswer))
            {
                //ResultSmile.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3EE07"));
                ResultSmile.Content = "😊";
                ResultAnswer.Text = "Good job!";
            }
            else
            {
                ResultSmile.Content = "😞";
                ResultAnswer.Text = "You've got this! Mistakes happen.";              
            }

            RightAnswer.Text = Task.Content + " - " + words[indexOfTaskWord].transcription + " - " + rightAnswer;
            VisibilityOfResult();
            timer.Start();
        }

        private void VisibilityOfResult()
        {
            Keyboard.ClearFocus();
            Var1.Visibility = Visibility.Hidden;
            Var2.Visibility = Visibility.Hidden;
            Var3.Visibility = Visibility.Hidden;
            Var4.Visibility = Visibility.Hidden;

            Answer.Visibility = Visibility.Hidden;
            ResultSmile.Visibility = Visibility.Hidden;
            Answer.Text = "";

            Task.Visibility = Visibility.Hidden;

            ResultSmile.Visibility = Visibility.Visible;
            ResultAnswer.Visibility = Visibility.Visible;
            RightAnswer.Visibility = Visibility.Visible;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Visibility = Visibility.Visible;
            NewTask();
            timer.Stop();
        }

        private bool Equels(int i, string str)
        {
            if (currentExercise == 1 || currentExercise == 2)
                rightAnswer = (words[i].english).Trim();
            if (currentExercise == 0 || currentExercise == 3)
                rightAnswer = (words[i].ukrainian).Trim(); 

            return rightAnswer.Equals(str.Trim(), StringComparison.OrdinalIgnoreCase) && 
                rightAnswer.Equals(str.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }

        private void Var1_Click(object sender, RoutedEventArgs e)
        {
            ShowResult(Var1.Text.ToString());
        }

        private void Var2_Click(object sender, RoutedEventArgs e)
        {

            ShowResult(Var2.Text.ToString());
        }

        private void Var3_Click(object sender, RoutedEventArgs e)
        {

            ShowResult(Var3.Text.ToString());
        }

        private void Var4_Click(object sender, RoutedEventArgs e)
        {

            ShowResult(Var4.Text.ToString());
        }

        private void NextEx_Click(object sender, RoutedEventArgs e)
        {
            if (maxExercise != currentExercise)
                currentExercise++;
            else currentExercise = minExercise;

            ChangeNameOfExercise();
            NewTask();
        }

        private void PrivEx_Click(object sender, RoutedEventArgs e)
        {
            if (minExercise != currentExercise)
                currentExercise--;
            else currentExercise = maxExercise;

            ChangeNameOfExercise();
            NewTask();
        }

        private void ChangeNameOfExercise()
        {
            switch (currentExercise)
            {
                case 0:
                    NameOfExercise.Content = "Choose right English translation";
                    break;
                case 1:
                    NameOfExercise.Content = "Choose right Ukrainian translation";
                    break;
                case 2:
                    NameOfExercise.Content = "Translate to English";
                    break;
                case 3:
                    NameOfExercise.Content = "Translate to Ukrainian";
                    break;
            }
        }

    }
}
