using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class K3Cloud_Common
    {
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_Common_Group
        {
            public string GroupFieldKey { get; set; } = "";
            public int GroupPkId { get; set; } = 0;
            public int FParentId { get; set; } = 0;
            public string FNumber { get; set; } = "";
            public string FName { get; set; } = "";
            public string FDescription { get; set; } = "";
        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_FNumber
        {
            [JsonProperty(PropertyName = "FNumber")]
            public string FNumber { get; set; } = "";
        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_FUserID
        {
            [JsonProperty(PropertyName = "FUserID")]
            public string FUserID { get; set; } = "";
        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_View_Qrcode
        {
            public string url { get; set; }
            public K3Cloud_View_Qrcode_para para { get; set; }
            public string target { get; set; }
            public K3Cloud_View_Qrcode_feature feature { get; set; }
            public string title { get; set; }

        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_View_Qrcode_para
        {
            public bool __pi__ { get; set; } = true;
            public string detailURL { get; set; } = "";//": "http://dev-fx.hzrobam.com/#/fenxiao/",
            public string urlPath { get; set; } = "";//": "http://172.18.8.36/DWGateway/restful/",
            public string testUser { get; set; } = "";//": "administrator",
            public string P_ORDER_NO_SELECT { get; set; } = "";//": "100001S99-2209011493",
            public string testPassword { get; set; } = "";//": "fxrobam",
            public string ss { get; set; } = "";//": "ŽÚÑò",
            public string fine_hyperlink { get; set; } = "";//": "64e5d068-9d34-4f72-905c-3136ddf2d11d"
        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_View_Qrcode_feature
        {
            public int width { get; set; }//": 1850,
            public int height { get; set; }//": 700
        }
        
    }
    public class K3Cloud_FNumberAndName
    {
        public string FNumber { get; set; }
        public string FName { get; set; }
    }
}
