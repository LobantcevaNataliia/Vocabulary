using System.Windows;

namespace VocabularyDesktop
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        public Message(string info)
        {
            InitializeComponent();
            lblInfo.Content = info;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
