using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для LearningWords.xaml
    /// </summary>
    public partial class LearningWords : Window
    {
        public List<Word> listAllWords = new List<Word>();
        public List<Word> listLearnWords;
        string FilePath;
        int iCurrent;

        public LearningWords(List<Word> listAllWords, string FilePath)
        {
            InitializeComponent();
            this.listAllWords = listAllWords;
            this.FilePath = FilePath;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if(iCurrent > 0)
            {
                textBlock1.Text = listLearnWords[iCurrent - 1].english;
                textBlock2.Text = listLearnWords[iCurrent - 1].russian;
                iCurrent--;
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent < listLearnWords.Count - 1)
            {
                textBlock1.Text = listLearnWords[iCurrent + 1].english;
                textBlock2.Text = listLearnWords[iCurrent + 1].russian;
                iCurrent++; 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listLearnWords = new List<Word>();
            for (int i = 0; i < listAllWords.Count; i++)
                if (!listAllWords[i].status)
                    listLearnWords.Add(listAllWords[i]);

            iCurrent = 0;
            textBlock1.Text = listLearnWords[0].english;
            textBlock2.Text = listLearnWords[0].russian;
        }
    }
}
