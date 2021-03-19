using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBSQLONE.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using DBSQLONE.Models;

namespace DBSQLONE.Pages.Tables
{
    public class GuildsModel : PageModel
    {
        public List<Guild> Table = new List<Guild>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM Guilds";
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
                            Table.Add(new Guild
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                NameGuild = Convert.ToString(reader.GetValue(reader.GetOrdinal("NameGuild"))),
                                DescriptionGuild = Convert.ToString(reader.GetValue(reader.GetOrdinal("DescriptionGuild"))),
                                CreatorIdPlayer = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CreatorIdPlayer"))),
                                Created = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Created")))
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

        public IActionResult OnPostUpdate(Guild guild)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE Guilds SET NameGuild = @NameGuild, DescriptionGuild = @DescriptionGuild, CreatorIdPlayer = @CreatorIdPlayer, Created = @Created WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = guild.Id;
                cmd.Parameters.Add("@NameGuild", MySqlDbType.VarChar).Value = guild.NameGuild;
                cmd.Parameters.Add("@DescriptionGuild", MySqlDbType.VarChar).Value = guild.DescriptionGuild;
                cmd.Parameters.Add("@CreatorIdPlayer", MySqlDbType.Int32).Value = guild.CreatorIdPlayer;
                cmd.Parameters.Add("@Created", MySqlDbType.Date).Value = guild.Created;
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

        public IActionResult OnPostDelete(Guild guild)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Guilds WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = guild.Id;
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

        public IActionResult OnPostInsert(Guild guild)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT Guilds(NameGuild, DescriptionGuild, CreatorIdPlayer, Created) VALUES(@NameGuild, @DescriptionGuild, @CreatorIdPlayer, @Created)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@NameGuild", MySqlDbType.VarChar).Value = guild.NameGuild;
                cmd.Parameters.Add("@DescriptionGuild", MySqlDbType.VarChar).Value = guild.DescriptionGuild;
                cmd.Parameters.Add("@CreatorIdPlayer", MySqlDbType.Int32).Value = guild.CreatorIdPlayer;
                cmd.Parameters.Add("@Created", MySqlDbType.Date).Value = guild.Created;
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
