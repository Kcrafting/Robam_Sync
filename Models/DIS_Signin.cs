using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Signin
    {
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [JsonProperty(PropertyName = "statusDescription")]
        public string StatusDescription { get; set; }
        [JsonProperty(PropertyName = "response")]
        public DIS_Signin_Response Response { get; set; }
        [JsonProperty(PropertyName = "profile")]
        public DIS_Signin_Response Profile { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

    }
    public class DIS_Signin_Response
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }
    }
    public class DIS_Signin_Profile
    {
        [JsonProperty(PropertyName = "Program-Code")]
        public string Program_Code { get; set; }

    }
}
