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
    public class GuildMembersModel : PageModel
    {
        public List<GuildMember> Table = new List<GuildMember>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM GuildMembers";
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
                            Table.Add(new GuildMember
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                IdPlayer = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdPlayer"))),
                                IdGuild = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdGuild")))
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

        public IActionResult OnPostUpdate(GuildMember guildMember)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE GuildMembers SET IdPlayer = @IdPlayer, IdGuild = @IdGuild WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = guildMember.Id;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = guildMember.IdPlayer;
                cmd.Parameters.Add("@IdGuild", MySqlDbType.Int32).Value = guildMember.IdGuild;
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

        public IActionResult OnPostDelete(GuildMember guildMember)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM GuildMembers WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = guildMember.Id;
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

        public IActionResult OnPostInsert(GuildMember guildMember)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT GuildMembers(IdPlayer, IdGuild) VALUES(@IdPlayer, @IdGuild)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = guildMember.IdPlayer;
                cmd.Parameters.Add("@IdGuild", MySqlDbType.Int32).Value = guildMember.IdGuild;
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
