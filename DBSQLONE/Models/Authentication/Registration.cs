using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DBSQLONE.Models.Authentication
{
    public class Registration
    {
        public string Nickname { get; set; }
        public string EmailUser { get; set; }
        public string PasswordUser { get; set; }
    }
}
