using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBSQLONE.Models;
using DBSQLONE.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace DBSQLONE.Pages.Authentication
{
    public class RegistrationModel : PageModel
    {
        public Registration Registration;
        public string ErrorMessage;
        public void OnGet()
        { }

        public IActionResult OnPostRegistration(Registration registration)
        {
            if(Execute(registration))
            {
                // Переадресация на вход
                return RedirectToPage("/Index");
            }
            else
            {
                return null;
            }
        }

        public bool Execute(Registration registration)
        {
            MySqlConnection conn = DatabaseConnection.GetMyDB();
            try
            {
                conn.Open();
                string sql = "INSERT Users(Nickname, EmailUser, PasswordUser) VALUES(@Nickname, @EmailUser, @PasswordUser)";
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@Nickname", MySqlDbType.VarChar).Value = registration.Nickname;
                cmd.Parameters.Add("@EmailUser", MySqlDbType.VarChar).Value = registration.EmailUser;
                cmd.Parameters.Add("@PasswordUser", MySqlDbType.VarChar).Value = registration.PasswordUser;
                cmd.ExecuteNonQuery();
            }
            catch
            {
                ErrorMessage = "Емайл или Имя уже используется";
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return true;
        }
    }
}
