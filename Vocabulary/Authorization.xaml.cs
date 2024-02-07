using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Vocabulary
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization(string action)
        {
            InitializeComponent();

            if(action == "LogIn")
            {
                UserEmailLabel.Visibility = Visibility.Hidden;
                UserEmailTextBox.Visibility = Visibility.Hidden;
            }
        }
        
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        User user;

        private void AuthorizationWindow_Closed(object sender, EventArgs e)
        {
            Window mainWindow = new MainWindow(user);
            mainWindow.Show();
            AuthorizationWindow.Close();
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserNameTextBox.Text;
            string userEmail = UserEmailTextBox.Text;
            string userPassword = HashPassword(UserPasswordTextBox.ToString());

            UserExists(userName, userEmail, userPassword);

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

        private void UserExists(string userName, string userEmail, string userPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE UserName = @UserName";
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
                        CheckUser(user, userEmail, userPassword);
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

        private void CheckUser(User user, string userEmail, string userPassword)
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
            UserEmailLabel.Visibility = Visibility.Visible;
            UserEmailTextBox.Visibility = Visibility.Visible;
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
