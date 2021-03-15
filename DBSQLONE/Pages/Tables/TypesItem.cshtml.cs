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
using DBSQLONE.Core;

namespace DBSQLONE.Pages.Tables
{
    public class TypesItemModel : PageModel
    {
        public List<TypeItem> Table = new List<TypeItem>();
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = Request.Cookies["ErrorMessage"];
            Response.Cookies.Append("ErrorMessage", "");
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "SELECT * FROM TypesItems";
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
                            Table.Add(new TypeItem
                            {
                                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                                NameType = Convert.ToString(reader.GetValue(reader.GetOrdinal("NameType"))),
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

        public IActionResult OnPostUpdate(TypeItem typesItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "UPDATE TypesItems SET NameType = @NameType WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@NameType", MySqlDbType.VarChar).Value = typesItem.NameType;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = typesItem.Id;
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

        public IActionResult OnPostDelete(TypeItem typesItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "DELETE FROM TypesItems WHERE Id = @Id";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = typesItem.Id;
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

        public IActionResult OnPostInsert(TypeItem typesItem)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT TypesItems(NameType) VALUES(@NameType)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@NameType", MySqlDbType.VarChar).Value = typesItem.NameType;
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
