using System.Collections.Generic;

namespace Models
{
    public class FrontEnd_TabaleIns
    {
        [SQLite.Column("column")]
        public List<FrontEnd_TabaleColumnDes> Columns { get; set; }
        [SQLite.Column("data")]
        public List<dynamic> Data { get; set; }
    }
}
