using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace Vocabulary
{
    internal static class DatabaseMethods
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;



       
    }
}
