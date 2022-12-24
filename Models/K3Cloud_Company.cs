using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company
    {
        [JsonProperty(PropertyName = "NeedUpDateFields")]
        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],

        [JsonProperty(PropertyName = "NeedReturnFields")]
        public JArray NeedReturnFields { get; set; } = new JArray();//": [],

        [JsonProperty(PropertyName = "IsDeleteEntry")]
        public bool IsDeleteEntry { get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "SubSystemId")]
        public string SubSystemId{ get; set; }//": "",

        [JsonProperty(PropertyName = "IsVerifyBaseDataField")]
        public bool IsVerifyBaseDataField { get; set; }//": "false",

        [JsonProperty(PropertyName = "IsEntryBatchFill")]
        public bool IsEntryBatchFill { get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "ValidateFlag")]
        public bool ValidateFlag { get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "NumberSearch")]
        public bool NumberSearch { get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "IsAutoAdjustField")]
        public bool IsAutoAdjustField { get; set; }//": "false",

        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags { get; set; }//": "",
        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; }//": "",

        [JsonProperty(PropertyName = "IsControlPrecision")]
        public bool IsControlPrecision { get; set; }//": "false",

        [JsonProperty(PropertyName = "Model")]
        public K3Cloud_Company_Model Model { get; set; } = new K3Cloud_Company_Model();//":
       
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_Model
    {
        [JsonProperty(PropertyName = "FID")]
        public int FID { get; set; }//": 0,

        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber{ get; set; }//": "",

        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; }//": "",

        [JsonProperty(PropertyName = "FCreateDate")]
        public string FCreateDate { get; set; }//": "1900-01-01",

        [JsonProperty(PropertyName = "FAuditTime")]
        public string FAuditTime { get; set; }//": "1900-01-01",

        [JsonProperty(PropertyName = "F_PLYE_CreatorId")]
        public K3Cloud_Company_Model_F_PLYE_CreatorId F_PLYE_CreatorId { get; set; } = new K3Cloud_Company_Model_F_PLYE_CreatorId();//":
        [JsonProperty(PropertyName = "FGroup")]
        public K3Cloud_Company_Model_FGroup FGroup { get; set; } = new K3Cloud_Company_Model_FGroup();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_Model_FGroup
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; }//": "",
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_Model_F_PLYE_CreatorId
    {
        [JsonProperty(PropertyName = "FUserID")]
        public string FUserID { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_GroupQuery
    {
        [JsonProperty(PropertyName = "FormId")]
        public string FormId{ get; set;}//": "",
        [JsonProperty(PropertyName = "GroupFieldKey")]
        public string GroupFieldKey { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "GroupPkIds")]
        public string GroupPkIds { get; set; } = ""; //": "",
        [JsonProperty(PropertyName = "Ids")]
        public string Ids { get; set; } = "";//": ""
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_GroupQuery_Result
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_Company_GroupQuery_Result_Result Result { get; set; }//": "",

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_GroupQuery_Result_Result
    {
        [JsonProperty(PropertyName = "ResponseStatus")]
        public K3Cloud_Company_GroupQuery_Result_Result_ResponseStatus ResponseStatus { get; set; } = new K3Cloud_Company_GroupQuery_Result_Result_ResponseStatus();//": "",
        [JsonProperty(PropertyName = "NeedReturnData", NullValueHandling = NullValueHandling.Include)]
        public List<K3Cloud_Company_GroupQuery_Result_Result_NeedReturnData> NeedReturnData { get; set; } = new List<K3Cloud_Company_GroupQuery_Result_Result_NeedReturnData>();//": "",

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_GroupQuery_Result_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess { get; set; }//": "",
        [JsonProperty(PropertyName = "Errors")]
        public JArray Errors { get; set; }//": "",
        [JsonProperty(PropertyName = "SuccessEntitys")]
        public JArray SuccessEntitys { get; set; }//": "",
        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages { get; set; }//": ""
        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode { get; set; }//": "",
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Company_GroupQuery_Result_Result_NeedReturnData
    {
        [JsonProperty(PropertyName = "FID")]
        public int FID { get; set; }//": "",
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; }//": "",
        [JsonProperty(PropertyName = "FGROUPID")]
        public string FGROUPID { get; set; }//": "",
        [JsonProperty(PropertyName = "FPARENTID")]
        public int FPARENTID { get; set; }//": ""
        [JsonProperty(PropertyName = "FFULLPARENTID")]
        public string FFULLPARENTID { get; set; }//": "",
        [JsonProperty(PropertyName = "FLEFT")]
        public int FLEFT { get; set; }//": ""
        [JsonProperty(PropertyName = "FRIGHT")]
        public int FRIGHT { get; set; }//": ""
        [JsonProperty(PropertyName = "FNAME")]
        public string FNAME { get; set; }//": "",
    }
}
