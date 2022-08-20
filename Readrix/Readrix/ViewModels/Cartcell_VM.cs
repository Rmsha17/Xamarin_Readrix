using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.ViewModels
{
    class Cartcell_VM
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string image { get; set; }
    }
}
