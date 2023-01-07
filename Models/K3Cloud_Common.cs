using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public class K3Cloud_FName
        {
            [JsonProperty(PropertyName = "FNumber")]
            public string FName { get; set; } = "";
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
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_FNumberAndName
        {
            public string FNumber { get; set; }
            public string FName { get; set; }
        }
        [JsonObject(MemberSerialization.OptOut)]
        public class K3Cloud_Bill_SaveHeader<T>  where T : new()
        {
            [JsonProperty(PropertyName = "NeedUpDateFields")]
            public JArray NeedUpDateFields { get; set; } = new JArray();
            [JsonProperty(PropertyName = "NeedReturnFields")]
            public JArray NeedReturnFields { get; set; } = new JArray();
            [JsonProperty(PropertyName = "IsDeleteEntry")]
            public string IsDeleteEntry { get; set; } = "true";
            [JsonProperty(PropertyName = "SubSystemId")]
            public string SubSystemId { get; set; } = "";
            [JsonProperty(PropertyName = "IsVerifyBaseDataField")]
            public string IsVerifyBaseDataField { get; set; } = "false";
            [JsonProperty(PropertyName = "IsEntryBatchFill")]
            public string IsEntryBatchFill { get; set; } = "true";
            [JsonProperty(PropertyName = "ValidateFlag")]
            public string ValidateFlag { get; set; } = "true";
            [JsonProperty(PropertyName = "NumberSearch")]
            public string NumberSearch { get; set; } = "true";
            [JsonProperty(PropertyName = "IsAutoAdjustField")]
            public string IsAutoAdjustField { get; set; } = "false";
            [JsonProperty(PropertyName = "InterationFlags")]
            public string InterationFlags { get; set; } = "";
            [JsonProperty(PropertyName = "IgnoreInterationFlag")]
            public string IgnoreInterationFlag { get; set; } = "";
            [JsonProperty(PropertyName = "IsControlPrecision")]
            public string IsControlPrecision { get; set; } = "false";
            [JsonProperty(PropertyName = "ValidateRepeatJson")]
            public string ValidateRepeatJson { get; set; } = "false";
            [JsonProperty(PropertyName = "Model")]
            public T Model { get; set; } = new T();

        }

    }
    
}
