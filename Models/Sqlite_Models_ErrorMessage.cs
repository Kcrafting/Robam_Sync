using Newtonsoft.Json;

namespace Models
{
    [SQLite.Table("SqliteModelsErrorMessage")]
    [JsonObject(MemberSerialization.OptOut)]
    public class Sqlite_Models_ErrorMessage
    {
        [JsonProperty(PropertyName = "fid")]

        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.NotNull]
        public int FID { get; set; }


        [JsonProperty(PropertyName = "frecordtime")]
        public string FRecordTime { get; set; }


        [JsonProperty(PropertyName = "ferrorfilepath")]
        public string FErrorFilepath { get; set; }


        [JsonProperty(PropertyName = "ferrorfunction")]
        public string FErrorFunction { get; set; }


        [JsonProperty(PropertyName = "ferrormessage")]
        public string FErrorMessage { get; set; }


        [JsonProperty(PropertyName = "ferrorlinenumber")]
        public string FErrorLinenumber { get; set; }
    }
}
