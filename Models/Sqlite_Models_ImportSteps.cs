using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [SQLite.Table("sqlitemodelsimportsteps")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_ImportSteps
    {
        [JsonProperty(PropertyName = "id")]

        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.NotNull]
        public virtual int FID { get; set; }


        [JsonProperty(PropertyName = "time")]
        public virtual string Time { get; set; }


        [JsonProperty(PropertyName = "iserror")]
        public virtual bool IsError { get; set; }


        [JsonProperty(PropertyName = "message")]
        public virtual string Message { get; set; }
    }
}
