using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using DBSQLONE.Models.Authentication;
using System.Data.Common;
using DBSQLONE.Models;

namespace DBSQLONE.Pages.Authentication
{
    public class AuthorizationModel : PageModel
    {
        public User NewUser = new();
        public string ErrorMessage;

        public void OnGet()
        {
        }

        public IActionResult OnPostAuthorization(User user)
        {
            if (Execute(user))
            {
                Response.Cookies.Append("Nickname", NewUser.Nickname);
                Response.Cookies.Append("EmailUser", NewUser.EmailUser);
                Response.Cookies.Append("PasswordUser", NewUser.PasswordUser);
                // Переадресация на просмотр
                return RedirectToPage("/Index");
            }
            else
            {
                NewUser = user;
                // Вывод ошибки об авторизации
                ErrorMessage = "Вы ввели неправильный логин или пароль";
                return null;
            }
        }

        public bool Execute(User user)
        {
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
