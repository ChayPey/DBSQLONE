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

namespace DBSQLONE.Pages.Tables
{
    public class PlayersModel : PageModel
    {
        public List<Player> Players = new List<Player>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
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

        public IActionResult OnPostUpdate(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
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
                Response.Cookies.Append("ErrorMessage", e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Players WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = player.Id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Response.Cookies.Append("ErrorMessage", e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostInsert(Player player)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
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
                Response.Cookies.Append("ErrorMessage", e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return RedirectToPage();
        }
    }
}
