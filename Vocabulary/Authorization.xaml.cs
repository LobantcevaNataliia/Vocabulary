using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();

            UserEmailLabel.Visibility = Visibility.Hidden;
            UserEmailTextBox.Visibility = Visibility.Hidden;
            
        }
        
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        User user;
        ObservableCollection<Word> words;

        private void AuthorizationWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            AuthorizationWindow.Close();
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Конвертуємо байти у рядок (hex representation)
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private string CreateTemporeryPassword()
        {

            return "";
        }

        private void UserExists(string userName, string userEmail, string userPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM myVocabDB.Users WHERE UserName = @UserName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    if (command.ExecuteScalar() == null)
                        AddNewUser(userName, userEmail, userPassword);
                    else
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                (
                                    Convert.ToInt32(reader["UserID"]),
                                    reader["UserName"].ToString(),
                                    reader["UserEmail"].ToString(),
                                    reader["UserPassword"].ToString()
                                );
                            }
                        }
                        CheckUser(user, userPassword);
                    }
                }
            }
        }

        private void UserExists(string userName, string userPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM myVocabDB.Users WHERE UserName = @UserName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    if (command.ExecuteScalar() == null)
                        MessageBox.Show("Wrong data");
                    else
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                (
                                    Convert.ToInt32(reader["UserID"]),
                                    reader["UserName"].ToString(),
                                    reader["UserEmail"].ToString(),
                                    reader["UserPassword"].ToString()
                                );
                            }
                        }
                        CheckUser(user, userPassword);
                    }
                }
            }
        }


        private void AddNewUser(string userName, string userEmail, string userPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Вставкa даних в БД
                    string query = "INSERT INTO myVocabDB.users (UserName, UserEmail, UserPassword) VALUES (@Value1, @Value2, @Value3)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", userName);
                        command.Parameters.AddWithValue("@Value2", userEmail);
                        command.Parameters.AddWithValue("@Value3", userPassword);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding a word: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }

        private void CheckUser(User user, string userPassword)
        {
            if (user.password == userPassword)
            {
                Window mainWindow = new MainWindow(user);
                mainWindow.Show();
                AuthorizationWindow.Close();
            }
            else MessageBox.Show("Wrong data");
        }     

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userEmail = UserEmailTextBox.Text;
            string userPassword = HashPassword(UserPasswordBox.ToString());

            //UserExists(userName, userEmail, userPassword);
            
            

        }

        CheckBox checkBox1 = new CheckBox();
        CheckBox checkBox2 = new CheckBox();
        CheckBox checkBox3 = new CheckBox();
        public void AddWordsWindow()
        {
            Window addWords = new Window();
            addWords.Height = 300;
            addWords.Width = 300;
            addWords.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StackPanel stackPanel = new StackPanel();
            stackPanel.Background = System.Windows.Media.Brushes.Blue;

            Label label = new Label();
            label.Content = "Click to add to your account the most used words to study";
            label.FontWeight = FontWeights.Bold;
            label.Foreground = System.Windows.Media.Brushes.Aquamarine;
            label.HorizontalAlignment = HorizontalAlignment.Left;

            CheckBox checkBox1 = new CheckBox();
            checkBox1.Content = "Thousand of the most used English words";
            checkBox1.IsChecked = true;
            CheckBox checkBox2 = new CheckBox();
            checkBox2.Content = "The second thousand most used English words";
            CheckBox checkBox3 = new CheckBox();
            checkBox3.Content = "The third thousand most used English words";

            Button buttonOk = new Button();
            buttonOk.Content = "Ok";
            buttonOk.Click += ButtonOk_Click;

            stackPanel.Children.Add(label);
            stackPanel.Children.Add(checkBox1);
            stackPanel.Children.Add(checkBox2);
            stackPanel.Children.Add(checkBox3);
            stackPanel.Children.Add(buttonOk);
            addWords.Content = stackPanel;
            addWords.Show();

        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
                AddWords("A");
            if (checkBox1.IsChecked == true)
                AddWords("B");
            if (checkBox1.IsChecked == true)
                AddWords("C");

            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            AuthorizationWindow.Close();
        }

        public void AddWords(string level)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE Level = @UserLevel";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserLevel", level);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Word word = new Word
                            (
                                Convert.ToInt32(reader["WordID"]),
                                reader["EnglishWord"].ToString(),
                                reader["Transcription"].ToString(),
                                reader["UkrainianWord"].ToString(),
                                Convert.ToBoolean(reader["Status"]),
                                (Level)Enum.Parse(typeof(Level), reader["Level"].ToString())
                            );
                            InsertDependenceIntoDatabase(word);
                            //words.Add(word);
                        }

                    }
                }
            }
        }
        
        private void InsertDependenceIntoDatabase(Word word)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa зв'язку в БД
                    string query = "INSERT INTO myVocabDB.learnedwords (UserId, WordId, Status) VALUES (@Value1, @Value2, @Value3)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", user.id);
                        command.Parameters.AddWithValue("@Value2", word.Id);
                        command.Parameters.AddWithValue("@Value3", false);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding dependence: {ex.Message}" + "\nPlease contact the admin!");
                }
            }
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userPassword = HashPassword(UserPasswordBox.ToString());

            UserExists(userName, userPassword);
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userEmail = UserEmailTextBox.Text;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM myVocabDB.Users WHERE UserName = @UserName AND UserEmail = @UserEmail";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@UserEmail", userEmail);

                    if (command.ExecuteScalar() == null)
                        MessageBox.Show("Check your data");
                    else
                        MessageBox.Show($"Your Temporery password is: {CreateTemporeryPassword()}");
                }
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            TextBlockAuto.Text = "Please enter your username and email to reset your password";
            UserEmailLabel.Visibility = Visibility.Visible;
            UserEmailTextBox.Visibility = Visibility.Visible;
            UserPasswordLabel.Visibility = Visibility.Hidden;
            UserPasswordBox.Visibility = Visibility.Hidden;
            LogIn.Visibility = Visibility.Hidden;
            SignUp.Visibility = Visibility.Hidden;
            ResetPassword.Visibility = Visibility.Visible;
            NoAccount.Visibility = Visibility.Hidden;
            HaveAccount.Visibility = Visibility.Visible;
            ForgotPassword.Visibility = Visibility.Hidden;
        }

        private void HaveAccount_Click(object sender, RoutedEventArgs e)
        {
            TextBlockAuto.Text = "Log in into your account";
            UserEmailLabel.Visibility = Visibility.Hidden;
            UserEmailTextBox.Visibility = Visibility.Hidden;
            UserPasswordLabel.Visibility = Visibility.Visible;
            UserPasswordBox.Visibility = Visibility.Visible;
            LogIn.Visibility = Visibility.Visible;
            SignUp.Visibility = Visibility.Hidden;
            ResetPassword.Visibility = Visibility.Hidden;
            NoAccount.Visibility = Visibility.Visible;
            HaveAccount.Visibility = Visibility.Hidden;
            ForgotPassword.Visibility = Visibility.Visible;
        }

        private void NoAccount_Click(object sender, RoutedEventArgs e)
        {
            TextBlockAuto.Text = "Create account";
            UserEmailLabel.Visibility = Visibility.Visible;
            UserEmailTextBox.Visibility = Visibility.Visible;
            LogIn.Visibility = Visibility.Hidden;
            SignUp.Visibility = Visibility.Visible;
            ResetPassword.Visibility = Visibility.Hidden;
            NoAccount.Visibility = Visibility.Hidden;
            HaveAccount.Visibility = Visibility.Visible;
            ForgotPassword.Visibility = Visibility.Hidden;
        }
    }
}
