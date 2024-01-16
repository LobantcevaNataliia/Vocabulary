using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class LearningWords : Window
    {
        ObservableCollection<Word> words;
        public List<Word> listLearnWords;
        int iCurrent;

        public LearningWords(ObservableCollection<Word> words)
        {
            InitializeComponent();
            this.words = words;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if(iCurrent > 0)
            {
                textBlockEnglish.Text = listLearnWords[iCurrent - 1].english;
                textBlockTranscription.Text = listLearnWords[iCurrent - 1].transcription;
                textBlockUkrainian.Text = listLearnWords[iCurrent - 1].ukrainian;
                iCurrent--;
            }

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (iCurrent < listLearnWords.Count - 1)
            {
                textBlockEnglish.Text = listLearnWords[iCurrent + 1].english;
                textBlockTranscription.Text = listLearnWords[iCurrent + 1].transcription;
                textBlockUkrainian.Text = listLearnWords[iCurrent + 1].ukrainian;
                iCurrent++; 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listLearnWords = new List<Word>();
            for (int i = 0; i < words.Count; i++)
                if (!words[i].status)
                    listLearnWords.Add(words[i]);

            iCurrent = 0;
            textBlockEnglish.Text = listLearnWords[0].english;
            textBlockTranscription.Text = listLearnWords[0].transcription;
            textBlockUkrainian.Text = listLearnWords[0].ukrainian;
        }

        private void Window_Closed(object sender, EventArgs e)
        {            
            Window exerciseWindow = new Exercise(words);
            exerciseWindow.Show();
            LearningWordsWindow.Close();
        }
    }
}
