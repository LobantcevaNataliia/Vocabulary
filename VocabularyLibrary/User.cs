using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyLibrary
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string TemporaryPassword { get; set; }

        public User()
        {
            Id = 2;
            Name = "";
            Email = "";
            Password = "";
            //TemporaryPassword = temporaryPassword;
        }

        public User(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            //TemporaryPassword = temporaryPassword;
        }

    }
}
