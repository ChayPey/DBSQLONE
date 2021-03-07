using DBSQLONE.Core;
using DBSQLONE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Pages
{
    public class IndexModel : PageModel
    {
        public List<Player> Players= new List<Player>();
        public string ErrorMessage;

        public IndexModel() { }

        public void OnGet()
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            conn.Open();
            try
            {
                string sql = "SELECT * FROM Players";
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Players.Add(new Player
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                Email = Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
                                Pass = Convert.ToString(reader.GetValue(reader.GetOrdinal("Pass"))),
                                Nickname = Convert.ToString(reader.GetValue(reader.GetOrdinal("Nickname"))),
                                Registered = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Registered")))
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public void OnPostUpdate(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            conn.Open();
            try
            {
                string sql = "UPDATE Players SET Nickname = @Nickname, Pass = @Pass, Email = @Email, Registered = @Registered WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Nickname", MySqlDbType.VarChar).Value = player.Nickname;
                cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = player.Pass;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = player.Email;
                cmd.Parameters.Add("@Registered", MySqlDbType.Date).Value = player.Registered;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = player.Id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            OnGet();
        }   

        public void OnPostDelete(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            conn.Open();
            try
            {
                string sql = "DELETE FROM Players WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = player.Id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            OnGet();
        }

        public void OnPostInsert(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            conn.Open();
            try
            {
                string sql = "INSERT Players(Nickname, Pass, Email, Registered) VALUES(@Nickname, @Pass, @Email, @Registered)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Nickname", MySqlDbType.VarChar).Value = player.Nickname;
                cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = player.Pass;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = player.Email;
                cmd.Parameters.Add("@Registered", MySqlDbType.Date).Value = player.Registered;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            OnGet();
        }
    }
}
