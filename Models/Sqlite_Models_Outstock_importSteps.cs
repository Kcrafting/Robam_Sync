using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [SQLite.Table("sqlitemodelsoutstockimportsteps")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_Outstock_importSteps:Sqlite_Models_ImportSteps
    {
        [JsonProperty(PropertyName = "id")]

        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.NotNull]
        public override int FID { get; set; }


        [JsonProperty(PropertyName = "time")]
        public override string Time { get; set; }


        [JsonProperty(PropertyName = "iserror")]
        public override bool IsError { get; set; }


        [JsonProperty(PropertyName = "message")]
        public override string Message { get; set; }
    }
}
