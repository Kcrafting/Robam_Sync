using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    public interface Sqlite_Models_Parent
    {
        public string key { get; set; }
        public int index { get; set; }
        public string errorTime { get; set; }
        public bool isError { get; set; }
        public string description { get; set; }
        public Result_TableMessage_RowData Format();
    }
}
