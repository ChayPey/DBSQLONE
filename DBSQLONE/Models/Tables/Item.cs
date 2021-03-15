using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBSQLONE.Models.Tables
{
    public class Item
    {
        public int Id { get; set; }
        public int IdType { get; set; }
        public string NameItem { get; set; }
        public string DescriptionItem { get; set; }
    }
}
