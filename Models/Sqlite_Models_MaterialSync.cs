using Newtonsoft.Json;

namespace Models
{
    [SQLite.Table("sqlitemodelsmaterialsync")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_MaterialSync: Sqlite_Models_ImportSteps
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
