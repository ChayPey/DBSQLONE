using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Models.Tables
{
    public class Guild
    {
        public int Id { get; set; }
        public string NameGuild { get; set; }
        public string DescriptionGuild { get; set; }
        public int CreatorIdPlayer { get; set; }
        public DateTime Created { get; set; }

    }
}
