using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public DateTime Registered { get; set; }
    }
}
