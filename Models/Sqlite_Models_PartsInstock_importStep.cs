using Newtonsoft.Json;

namespace Models
{
    [SQLite.Table("sqlitemodelspartsinstockimportstep")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_PartsInstock_importStep
    {
        [JsonProperty(PropertyName = "id")]

        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.NotNull]
        public int FID { get; set; }


        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }


        [JsonProperty(PropertyName = "iserror")]
        public bool IsError { get; set; }


        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
