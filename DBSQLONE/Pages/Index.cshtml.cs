using DBSQLONE.Core;
using DBSQLONE.Models;
using DBSQLONE.Models.Authentication;
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
        public IActionResult OnGet()
        {
            if (!Check.Checking(new User() { EmailUser = Request.Cookies["EmailUser"], PasswordUser = Request.Cookies["PasswordUser"] }))
            {
                // Переадресация если не авторизованный пользователь
                return RedirectToPage("/Authentication/Authorization");
            }
            return null;
        }
    }
}
