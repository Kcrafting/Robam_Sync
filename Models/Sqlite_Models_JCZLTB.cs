using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    [SQLite.Table("sqlitemodelsjczltb")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_JCZLTB : Sqlite_Models_Parent
    {
        public string key { get; set; }
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Unique, SQLite.NotNull]
        public int index { get; set; }
        public string errorTime { get; set; }
        public bool isError { get; set; }
        public string description { get; set; }
        public Result_TableMessage_RowData Format()
        {
            return new Result_TableMessage_RowData() { key = key, index = index, errorTime = errorTime, isError = isError, description = description };
        }
    }
}
