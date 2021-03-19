using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBSQLONE.Core.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DBSQLONE.Pages.Authentication
{
    public class RegistrationModel : PageModel
    {
        public Registration Registration;
        public string ErrorMessage="sdfs";
        public void OnGet()
        {
        }

        public void OnPostRegistration(Registration registration)
        {
            if(registration.Execute())
            {
                ErrorMessage = "Все ок";
                RedirectToPage("../index");
            }
            else
            {
                ErrorMessage = "Все yt jr";
            }
        }
    }
}
