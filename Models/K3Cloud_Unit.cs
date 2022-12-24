using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    /*
     {
    "NeedUpDateFields": [],
    "NeedReturnFields": [],
    "IsDeleteEntry": "true",
    "SubSystemId": "",
    "IsVerifyBaseDataField": "false",
    "IsEntryBatchFill": "true",
    "ValidateFlag": "true",
    "NumberSearch": "true",
    "IsAutoAdjustField": "false",
    "InterationFlags": "",
    "IgnoreInterationFlag": "",
    "IsControlPrecision": "false",
    "Model": {
        "FUNITID": 0,
        "FNumber": "",
        "FName": "",
        "FUnitGroupId": {
            "FNumber": ""
        },
        "FPrecision": 0,
        "FRoundType": "",
        "SubHeadEntity": {
            "FUNITCONVERTRATEID": 0,
            "FConvertType": "",
            "FConvertNumerator": 0,
            "FConvertDenominator": 0
        }
    }
}
     */
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit
    {
        [JsonProperty(PropertyName = "NeedUpDateFields")]
        public JArray NeedUpDateFields { get; set; }//": [],

        [JsonProperty(PropertyName = "NeedReturnFields")]
        public JArray NeedReturnFields{ get; set; }//": [],

        [JsonProperty(PropertyName = "IsDeleteEntry")]
        public bool IsDeleteEntry{ get; set; }//": "true",

        [JsonProperty(PropertyName = "SubSystemId")]
        public string SubSystemId{ get; set; }//": "",

        [JsonProperty(PropertyName = "IsVerifyBaseDataField")]
        public bool IsVerifyBaseDataField{ get; set; }//": "false",

        [JsonProperty(PropertyName = "IsEntryBatchFill")]
        public bool IsEntryBatchFill{ get; set; }//": "true",

        [JsonProperty(PropertyName = "ValidateFlag")]
        public bool ValidateFlag{ get; set; }//": "true",

        [JsonProperty(PropertyName = "NumberSearch")]
        public bool NumberSearch{ get; set; }//": "true",

        [JsonProperty(PropertyName = "IsAutoAdjustField")]
        public bool IsAutoAdjustField{ get; set; }//": "false",

        [JsonProperty(PropertyName = "InterationFlags")]
        public bool InterationFlags{ get; set; }//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public bool IgnoreInterationFlag{ get; set; }//": "",

        [JsonProperty(PropertyName = "IsControlPrecision")]
        public bool IsControlPrecision{ get; set; }//": "false",

        [JsonProperty(PropertyName = "Model")]
        public K3Cloud_Unit_Model Model{ get; set; }//": "false",

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Model
    {
        [JsonProperty(PropertyName = "FUNITID")]
        public int FUNITID{ get; set; }//": 0,

        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber{ get; set; }//": "",

        [JsonProperty(PropertyName = "FName")]
        public string FName{ get; set; }//": "",

        [JsonProperty(PropertyName = "FUnitGroupId")]
        public K3Cloud_Unit_Model_FUnitGroupId FUnitGroupId{ get; set; }//":

        [JsonProperty(PropertyName = "FPrecision")]
        public int FPrecision{ get; set; }//": 0,

        [JsonProperty(PropertyName = "FRoundType")]
        public string FRoundType{ get; set; }//": "",

        [JsonProperty(PropertyName = "SubHeadEntity")]
        public K3Cloud_Unit_Model_SubHeadEntity SubHeadEntity{ get; set; }//":
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Model_FUnitGroupId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber{ get; set; }//": "false",
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Model_SubHeadEntity
    {
        [JsonProperty(PropertyName = "FUNITCONVERTRATEID")]
        public int FUNITCONVERTRATEID{ get; set; }//": 0,

        [JsonProperty(PropertyName = "FConvertType")]
        public string FConvertType{ get; set; }//": "",

        [JsonProperty(PropertyName = "FConvertNumerator")]
        public decimal FConvertNumerator{ get; set; }//": 0,

        [JsonProperty(PropertyName = "FConvertDenominator")]
        public decimal FConvertDenominator { get; set; }//": 0
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Result
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_Unit_Result_Result Result{ get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Result_Result
    {
        [JsonProperty(PropertyName = "ResponseStatus")]
        public K3Cloud_Unit_Result_Result_ResponseStatus ResponseStatus{ get; set; }//": {

        [JsonProperty(PropertyName = "Id")]
        public int Id{ get; set; }//": 100074,

        [JsonProperty(PropertyName = "Number")]
        public string Number{ get; set; }//": "测试",

        [JsonProperty(PropertyName = "NeedReturnData")]
        public JArray NeedReturnData{ get; set; }//": [
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Result_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess{ get; set; }//": true,

        [JsonProperty(PropertyName = "Errors")]
        public JArray Errors{ get; set; }//": [],

        [JsonProperty(PropertyName = "SuccessEntitys")]
        public List<K3Cloud_Unit_Result_Result_ResponseStatus_SuccessEntitys> SuccessEntitys{ get; set; }//": [

        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages{ get; set; }//": [],

        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode{ get; set; }//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Result_Result_ResponseStatus_SuccessEntitys
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id{ get; set; }//": 100074,
        [JsonProperty(PropertyName = "Number")]
        public string Number{ get; set; }//": "测试",
        [JsonProperty(PropertyName = "DIndex")]
        public int DIndex{ get; set; }//": 0
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Commit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers{ get; set; }//": [],

        [JsonProperty(PropertyName = "Ids")]
        public string Ids{ get; set; }//": "",

        [JsonProperty(PropertyName = "SelectedPostId")]
        public int SelectedPostId{ get; set; }//": 0,

        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl{ get; set; }//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag{ get; set; }//": ""
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Unit_Audit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers{ get; set; }//": [],

        [JsonProperty(PropertyName = "Ids")]
        public string Ids{ get; set; }//": "",

        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags{ get; set; }//": "",

        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl{ get; set; }//": "",

        [JsonProperty(PropertyName = "IsVerifyProcInst")]
        public string IsVerifyProcInst{ get; set; }//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag{ get; set; }//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Current_Query
    {
        [JsonProperty(PropertyName = "FormId")]
        public string FormId { get; set; } = "";//": "",


        [JsonProperty(PropertyName = "FieldKeys")]
        public string FieldKeys{ get; set; } = "";//": "",


        [JsonProperty(PropertyName = "FilterString")]
        public string FilterString { get; set; } = "";//": [],


        [JsonProperty(PropertyName = "OrderString")]
        public string OrderString{ get; set; } = "";//": "",


        [JsonProperty(PropertyName = "TopRowCount")]
        public int TopRowCount { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "StartRow")]
        public int StartRow{ get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "Limit")]
        public int Limit{ get; set; } = 0;//": 2000,


        [JsonProperty(PropertyName = "SubSystemId")]
        public string SubSystemId{ get; set; } = "";//": ""
    }
}
