using DBSQLONE.Models;
using DBSQLONE.Models.Authentication;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Core
{
    public class Check
    {
        public static bool Checking(User user)
        {
            User NewUser = new();
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM Users WHERE EmailUser = @EmailUser";
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.Parameters.Add("@EmailUser", MySqlDbType.VarChar).Value = user.EmailUser;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NewUser = new User
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                Nickname = Convert.ToString(reader.GetValue(reader.GetOrdinal("Nickname"))),
                                EmailUser = Convert.ToString(reader.GetValue(reader.GetOrdinal("EmailUser"))),
                                PasswordUser = Convert.ToString(reader.GetValue(reader.GetOrdinal("PasswordUser"))),
                                EditingRights = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("EditingRights"))),
                                DeleteRights = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("DeleteRights"))),
                                AddRights = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("AddRights"))),
                            };
                        }
                    }
                }
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

            if (NewUser.PasswordUser != user.PasswordUser)
            {
                return false;
            }
            return true;
        }
    }
}
