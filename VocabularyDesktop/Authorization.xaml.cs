using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VocabularyLibrary;

namespace VocabularyDesktop
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        User user = new User();
        ObservableCollection<Word> words = new ObservableCollection<Word>();

        public Authorization()
        {
            InitializeComponent();

            UserEmailLabel.Visibility = Visibility.Hidden;
            UserEmailTextBox.Visibility = Visibility.Hidden;
            
        }

        private void AuthorizationWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            AuthorizationWindow.Close();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userPassword = HashPassword(UserPasswordBox.Password); 
            DatabaseMethods.UserExists(userName, ref user);
            if (user == null)
            {
                Message message = new Message("This userName not exist!");
                message.Show();
            }
                
            else CheckUser(user, userPassword);
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userEmail = UserEmailTextBox.Text;
            string userPassword = HashPassword(UserPasswordBox.Password);

            DatabaseMethods.UserExists(userName, ref user);

            if (user == null)
            {
                user = new User (DatabaseMethods.GetIdNewUser(), userName, userEmail, userPassword);             
                MessageBox.Show(DatabaseMethods.AddNewUser(user));
                
                Window addWindow = new AddWords(words, user);
                addWindow.Show();
                AuthorizationWindow.Close();
            }
            else MessageBox.Show("This userName alredy exist!");
        }
         
        string? temporaryPassword;
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userEmail = UserEmailTextBox.Text;
            DatabaseMethods.UserExists(userName, ref user);
            temporaryPassword = CreateTemporaryPassword();

            if (!DatabaseMethods.ResetPassword(userName, userEmail, temporaryPassword))
                MessageBox.Show("Check your data");
            else
            {
                MessageBox.Show($"Your Temporary password is: {temporaryPassword}");
                NewPasswordWindow();
            }
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

        private void CheckUser(User user, string userPassword)
        {
            if (user.Password == userPassword)
            {
                Window mainWindow = new MainWindow(user);
                mainWindow.Show();
                AuthorizationWindow.Close();
                MessageBox.Show($"The account {user.Name} is logged in!");
            }
            else MessageBox.Show("Wrong userPassword");
        }

        private string CreateTemporaryPassword()
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //Random random = new Random();

            //// Генеруємо рандомний пароль довжиною 8 символів
            //string temporaryPassword = new string(Enumerable.Repeat(chars, 8)
            //    .Select(s => s[random.Next(s.Length)]).ToArray());

            //return temporaryPassword;

            return HashPassword(DateTime.Now.ToString()).Substring(0,6);
        }
        Window newPassword = new Window();
        TextBox tb1 = new TextBox();
        TextBox tb2 = new TextBox();
        TextBox tb3 = new TextBox();
        private void NewPasswordWindow()
        {
            newPassword = new Window();
            newPassword.Height = 300;
            newPassword.Width = 400;
            newPassword.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Grid myGrid = new Grid();
            myGrid.Background = System.Windows.Media.Brushes.AliceBlue;//new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 229, 253, 252));
            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef1);
            myGrid.ColumnDefinitions.Add(colDef2);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);

            Label lb1 = new Label();
            lb1.Content = "Enter your temporary password";
            lb1.Foreground = System.Windows.Media.Brushes.DodgerBlue;
            lb1.HorizontalAlignment = HorizontalAlignment.Left;
            lb1.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(lb1, 0);
            Grid.SetColumn(lb1, 0);

            tb1 = new TextBox();
            tb1.FontSize = 18;
            tb1.Width = 160;
            tb1.Padding = new Thickness(4);
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(tb1, 0);
            Grid.SetColumn(tb1, 1);

            Label lb2 = new Label();
            lb2.Content = "Enter your new password";
            lb2.Foreground = System.Windows.Media.Brushes.DodgerBlue;
            lb2.HorizontalAlignment = HorizontalAlignment.Left;
            lb2.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(lb2, 1);
            Grid.SetColumn(lb2, 0);

            tb2 = new TextBox();
            tb2.FontSize = 18;
            tb2.Width = 160;
            tb2.Padding = new Thickness(4);
            tb2.HorizontalAlignment = HorizontalAlignment.Left;
            tb2.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(tb2, 1);
            Grid.SetColumn(tb2, 1);

            Label lb3 = new Label();
            lb3.Content = "Confirm your new password";
            lb3.Foreground = System.Windows.Media.Brushes.DodgerBlue;
            lb3.HorizontalAlignment = HorizontalAlignment.Left;
            lb3.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(lb3, 2);
            Grid.SetColumn(lb3, 0);

            tb3 = new TextBox();
            tb3.FontSize = 18;
            tb3.Width = 160;
            tb3.Padding = new Thickness(4);
            tb3.HorizontalAlignment = HorizontalAlignment.Left;
            tb3.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(tb3, 2);
            Grid.SetColumn(tb3, 1);

            Button buttonOk = new Button();
            buttonOk.Content = "Ok";
            buttonOk.FontSize = 18;
            buttonOk.Width = 80;
            buttonOk.Height = 30;
            buttonOk.Foreground = System.Windows.Media.Brushes.White;
            buttonOk.Background = System.Windows.Media.Brushes.Green;
            buttonOk.HorizontalAlignment = HorizontalAlignment.Center;
            buttonOk.VerticalAlignment = VerticalAlignment.Center;
            buttonOk.Click += ButtonOk_Click;
            Grid.SetColumnSpan(buttonOk, 2);
            Grid.SetRow(buttonOk, 3);

            // Add the TextBlock elements to the Grid Children collection
            myGrid.Children.Add(tb1);
            myGrid.Children.Add(tb2);
            myGrid.Children.Add(tb3);
            myGrid.Children.Add(lb1);
            myGrid.Children.Add(lb2);
            myGrid.Children.Add(lb3);
            myGrid.Children.Add(buttonOk);
            newPassword.Content = myGrid;
            newPassword.Show();

        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (temporaryPassword == tb1.Text)
            {
                if (tb2.Text == tb3.Text)
                {
                    DatabaseMethods.ResetPassword(user.Name, user.Email, "");
                    if (DatabaseMethods.UpdatePassword(user.Name, HashPassword(tb2.Text)))
                    {
                        MessageBox.Show("Your password changed!");
                        Window mainWindow = new MainWindow(user);
                        mainWindow.Show();
                        AuthorizationWindow.Close();
                    }
                    else MessageBox.Show("Unfortunately, the password has not been changed. " +
                        "\n Please try again or contact the admin");
                }
                else MessageBox.Show("The new password not confirmed!");
            }
            else MessageBox.Show("Wrong temporary password!");

            newPassword.Close();
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
