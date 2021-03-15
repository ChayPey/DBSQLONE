using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Models.Tables
{
    public class CleanLocation
    {
        public int Id { get; set; }
        public int IdPlayer { get; set; }
        public int IdLocation { get; set; }
        public DateTime Passed { get; set; }
    }
}
