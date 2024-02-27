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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for ShowWordUC.xaml
    /// </summary>
    public partial class ShowWordUC : UserControl
    {
        //public TextBlock TextBlockEnglish { get; set; }
        //public TextBlock TextBlockUkrainian { get; set; }
        //public TextBlock TextBlockTranscription { get; set; }
        //public ProgressBar ProgressBarStatus { get; set; }
        public event RoutedEventHandler RadioButtonChecked;

        public ShowWordUC()
        {
            InitializeComponent();

            //TextBlockEnglish = textBlockEnglish;
            //TextBlockTranscription = textBlockTranscription;
            //TextBlockUkrainian = textBlockUkrainian;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Викликати подію при зміні вибраного RadioButton
            RadioButtonChecked?.Invoke(this, e);
        }
    }
}
