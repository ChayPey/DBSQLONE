using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Models.Tables
{
    public class GuildMember
    {
        public int Id { get; set; }
        public int IdPlayer { get; set; }
        public int IdGuild { get; set; }
    }
}
