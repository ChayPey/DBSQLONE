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
    public class LocationsModel : PageModel
    {
        public List<Location> Table = new List<Location>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM Locations";
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
                            Table.Add(new Location
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                NameLocation = Convert.ToString(reader.GetValue(reader.GetOrdinal("NameLocation"))),
                                DescriptionLocations = Convert.ToString(reader.GetValue(reader.GetOrdinal("DescriptionLocations")))
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

        public IActionResult OnPostUpdate(Location location)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE Locations SET NameLocation = @NameLocation, DescriptionLocations = @DescriptionLocations WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = location.Id;
                cmd.Parameters.Add("@NameLocation", MySqlDbType.VarChar).Value = location.NameLocation;
                cmd.Parameters.Add("@DescriptionLocations", MySqlDbType.VarChar).Value = location.DescriptionLocations;
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

        public IActionResult OnPostDelete(Location location)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Locations WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = location.Id;
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

        public IActionResult OnPostInsert(Location location)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT Locations(NameLocation, DescriptionLocations) VALUES(@NameLocation, @DescriptionLocations)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@NameLocation", MySqlDbType.VarChar).Value = location.NameLocation;
                cmd.Parameters.Add("@DescriptionLocations", MySqlDbType.VarChar).Value = location.DescriptionLocations;
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
