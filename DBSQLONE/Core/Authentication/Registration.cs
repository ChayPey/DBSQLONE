using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DBSQLONE.Core.Authentication
{
    public class Registration
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Execute() 
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT Users(Nickname, EmailUser, PasswordUser) VALUES(@Nickname, @EmailUser, @PasswordUser)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Nickname", MySqlDbType.VarChar).Value = Nickname;
                cmd.Parameters.Add("@EmailUser", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@PasswordUser", MySqlDbType.VarChar).Value = Password;
                cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return true;
        }
    }
}
