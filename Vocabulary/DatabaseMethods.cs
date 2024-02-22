using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Collections.ObjectModel;

namespace Vocabulary
{
    internal static class DatabaseMethods
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        public static void UserExists(string userName, out User user)
        {
            user = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM myVocabDB.Users WHERE UserName = @UserName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    if (command.ExecuteScalar() != null)
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                user = new User
                                (
                                    Convert.ToInt32(reader["UserID"]),
                                    reader["UserName"].ToString(),
                                    reader["UserEmail"].ToString(),
                                    reader["UserPassword"].ToString()
                                    //reader["TemporaryPassword"].ToString()
                                );
                            }
                        }

                }
            }
        }

        public static int GetIdNewUser()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // 
                string query = "SELECT MAX(UserID) FROM myVocabDB.users;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result) + 1;
                }
            }
        }

        public static string AddNewUser(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Вставкa даних в БД
                    string query = "INSERT INTO myVocabDB.users (UserId,UserName, UserEmail, UserPassword) VALUES (@Value1, @Value2, @Value3, @Value4)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", user.Id);
                        command.Parameters.AddWithValue("@Value2", user.Name);
                        command.Parameters.AddWithValue("@Value3", user.Email);
                        command.Parameters.AddWithValue("@Value4", user.Password);

                        command.ExecuteNonQuery();

                        return $"User {user.Name} added successfully!";
                    }
                }
                catch (Exception ex)
                {
                    return $"An error occurred while adding a user: \n{ex.Message}" + "\nPlease contact the admin!";
                }
            }
        }

        public static bool ResetPassword(string userName, string userEmail, string temporaryPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.Users SET TemporaryPassword = @TemporaryPassword WHERE UserName = @UserName AND UserEmail = @UserEmail";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@TemporaryPassword", temporaryPassword);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@UserEmail", userEmail);


                    if (command.ExecuteNonQuery() < 1)
                        return false;
                    else return true;
                }
            }
        }

        public static bool UpdatePassword(string userName, string newPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.Users SET UserPassword = @NewPassword WHERE UserName = @UserName";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);
                    command.Parameters.AddWithValue("@UserName", userName);


                    if (command.ExecuteNonQuery() < 1)
                        return false;
                    else return true;
                }
            }
        }

        public static User GetDefaultUser()
        {
            User user = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE UserName = @UserName";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", "Guest");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
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
                }
            }
            return user;
        }

        //Метод для завантаження словника з бази даних
        public static void LoadDataFromDatabase(int userId, out ObservableCollection<Word> words)
        {
            words = new ObservableCollection<Word>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // команда SQL для вибірки слів конкретного користувача
                string sql = "SELECT myVocabDB.words.*, LearnedWords.Status FROM myVocabDB.words " +
                             "INNER JOIN myVocabDB.learnedwords ON words.WordId = learnedwords.WordId " +
                             "WHERE learnedwords.UserId = @UserId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    // Додано параметр для ідентифікатора користувача
                    command.Parameters.AddWithValue("@UserId", userId);

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

                            words.Add(word);
                        }

                    }
                }
            }
        }

        public static int GetIdNewWord()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // 
                string query = "SELECT MAX(WordID) FROM myVocabDB.words;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result) + 1;
                }
            }
        }

        public static string InsertWordIntoDatabase(Word word)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa слова в БД
                    string query = "INSERT INTO myVocabDB.words (WordID, EnglishWord, Transcription, UkrainianWord, Level) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", word.Id);
                        command.Parameters.AddWithValue("@Value2", word.English);
                        command.Parameters.AddWithValue("@Value3", word.Transcription);
                        command.Parameters.AddWithValue("@Value4", word.Ukrainian);
                        command.Parameters.AddWithValue("@Value5", word.Level.ToString());
                        command.ExecuteNonQuery();

                        return $"Words added successfully!";
                    }
                }
                catch (Exception ex)
                {
                    return $"An error occurred while adding a word: {ex.Message}" + "\nPlease contact the admin!";
                }
            }
        }

        public static string AddWords(string level, ref ObservableCollection<Word> words, int userId)
        {
            if(words == null)
                words = new ObservableCollection<Word>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM myVocabDB.words WHERE Level = @UserLevel";
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
                                    false,
                                    (Level)Enum.Parse(typeof(Level), reader["Level"].ToString())
                                );
                                InsertDependenceIntoDatabase(word,userId);
                                words.Add(word);
                            }
                            return "Words added!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return $"An error occurred while adding a words: {ex.Message}" + "\nPlease contact the admin!";
                }
            }
        }

        public static string InsertDependenceIntoDatabase(Word word, int userId)
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
                        command.Parameters.AddWithValue("@Value1", userId);
                        command.Parameters.AddWithValue("@Value2", word.Id);
                        command.Parameters.AddWithValue("@Value3", false);

                        command.ExecuteNonQuery();

                        return $"Words added successfully!";
                    }
                }
                catch (Exception ex)
                {
                    return $"An error occurred while adding dependence: {ex.Message}" + "\nPlease contact the admin!";
                }
            }
        }


        public static string DeleteWordFromDatabase(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa слова в БД
                    string query = "DELETE FROM myVocabDB.words WHERE WordID=@Value1;";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", id);
                        //command.ExecuteNonQuery();
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    return $"An error occurred while delete a word: {ex.Message}" + "\nPlease contact the admin!";
                }
            }
        }

        public static string DeleteDependenceFromDatabase(int id, int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Вставкa зв'язку в БД
                    string query = "DELETE FROM myVocabDB.learnedwords WHERE UserId=@Value1 AND WordId=@Value2;";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", userId);
                        command.Parameters.AddWithValue("@Value2", id);
                        command.ExecuteNonQuery();
                        return "";
                    }
                }
                catch (Exception ex)
                {
                     return $"An error occurred while delete dependence: {ex.Message}" + "\nPlease contact the admin!";
                }
            }

        }

        public static string ChangeStatusWordInDatabase(int wordId, bool newStatus, int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE myVocabDB.learnedwords SET Status = @newValue1 WHERE WordId= @newValue2 AND UserId= @newValue3;";

                using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@newValue1", newStatus);
                    cmd.Parameters.AddWithValue("@newValue2", wordId);
                    cmd.Parameters.AddWithValue("@newValue3", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected < 1)
                        return $"An error occurred while changing the status of word." + "\nPlease contact the admin!";
                    else return "";
                }
            }

        }
    }
}
