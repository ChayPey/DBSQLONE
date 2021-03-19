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
    public class GetItemsModel : PageModel
    {
        public List<GetItem> Table = new List<GetItem>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM GetItems";
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
                            Table.Add(new GetItem
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                IdItem = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdItem"))),
                                IdPlayer = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IdPlayer"))),
                                Received = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("Received")))

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

        public IActionResult OnPostUpdate(GetItem getItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE GetItems SET IdItem = @IdItem, IdPlayer = @IdPlayer, Received = @Received WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = getItem.Id;
                cmd.Parameters.Add("@IdItem", MySqlDbType.Int32).Value = getItem.IdItem;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = getItem.IdPlayer;
                cmd.Parameters.Add("@Received", MySqlDbType.Date).Value = getItem.Received;
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

        public IActionResult OnPostDelete(GetItem getItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM GetItems WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = getItem.Id;
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

        public IActionResult OnPostInsert(GetItem getItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT GetItems(IdItem, IdPlayer, Received) VALUES(@IdItem, @IdPlayer, @Received)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@IdItem", MySqlDbType.Int32).Value = getItem.IdItem;
                cmd.Parameters.Add("@IdPlayer", MySqlDbType.Int32).Value = getItem.IdPlayer;
                cmd.Parameters.Add("@Received", MySqlDbType.Date).Value = getItem.Received;
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
