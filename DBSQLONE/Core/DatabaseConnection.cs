using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBSQLONE.Models
{
    public class DatabaseConnection
    {
        public static MySqlConnection Get(string host, int port, string database, string username, string password)
        {
            string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }

        public static MySqlConnection GetMyDB()
        {
            return Get("localhost", 3306, "gamedb", "root", "1997");
        }
    }
}
