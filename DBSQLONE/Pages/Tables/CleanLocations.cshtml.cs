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
    public class CleanLocationsModel : PageModel
    {
        public List<CleanLocation> Table = new List<CleanLocation>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM CleanLocations";
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
                            Table.Add(new CleanLocation
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                IdPlayer = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdPlayer"))),
                                IdLocation = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdLocation"))),
                                Passed = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Passed")))
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

        public IActionResult OnPostUpdate(CleanLocation cleanLocation)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE CleanLocations SET IdPlayer = @IdPlayer, IdLocation = @IdLocation, Passed = @Passed WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = cleanLocation.Id;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = cleanLocation.IdPlayer;
                cmd.Parameters.Add("@IdLocation", MySqlDbType.Int32).Value = cleanLocation.IdLocation;
                cmd.Parameters.Add("@Passed", MySqlDbType.Date).Value = cleanLocation.Passed;
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

        public IActionResult OnPostDelete(CleanLocation cleanLocation)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM CleanLocations WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = cleanLocation.Id;
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

        public IActionResult OnPostInsert(CleanLocation cleanLocation)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT CleanLocations(IdPlayer, IdLocation, Passed) VALUES(@IdPlayer, @IdLocation, @Passed)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = cleanLocation.IdPlayer;
                cmd.Parameters.Add("@IdLocation", MySqlDbType.Int32).Value = cleanLocation.IdLocation;
                cmd.Parameters.Add("@Passed", MySqlDbType.Date).Value = cleanLocation.Passed;

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
