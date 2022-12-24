using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus
    {
        [JsonProperty(PropertyName = "NeedUpDateFields")]
        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "NeedReturnFields")]
        public JArray NeedReturnFields { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "IsDeleteEntry")]
        public bool IsDeleteEntry { get; set; } = true;//": "true",
        [JsonProperty(PropertyName = "SubSystemId")]
        public string SubSystemId { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IsVerifyBaseDataField")]
        public bool IsVerifyBaseDataField { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "IsEntryBatchFill")]
        public bool IsEntryBatchFill { get; set; } = true;//": "true",
        [JsonProperty(PropertyName = "ValidateFlag")]
        public bool ValidateFlag { get; set; } = true;//": "true",
        [JsonProperty(PropertyName = "NumberSearch")]
        public bool NumberSearch { get; set; } = true;//": "true",
        [JsonProperty(PropertyName = "IsAutoAdjustField")]
        public bool IsAutoAdjustField { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IsControlPrecision")]
        public bool IsControlPrecision { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "Model")]
        public K3Cloud_StockStatus_Model Model { get; set; } = new K3Cloud_StockStatus_Model();//":
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus_Model
    {
        [JsonProperty(PropertyName = "FStockStatusId")]
        public int FStockStatusId{ get; set; } = 0;//": 0,
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "FType")]
        public string FType { get; set; } = "0";//": "",
        [JsonProperty(PropertyName = "FDescription")]
        public string FDescription { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "FAvailable")]
        public bool FAvailable { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "FNotSale")]
        public bool FNotSale { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "FNotGet")]
        public bool FNotGet { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "FAvailableLock")]
        public bool FAvailableLock { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "FAvailableMRP")]
        public bool FAvailableMRP { get; set; } = false;//": "false",
        [JsonProperty(PropertyName = "FGroup")]
        public K3Cloud_StockStatus_Model_FGroup FGroup { get; set; } = new K3Cloud_StockStatus_Model_FGroup();
        [JsonProperty(PropertyName = "FAvailableAlert")]
        public bool FAvailableAlert { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus_Model_FGroup
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus_View
    {
        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IsSortBySeq")]
        public bool IsSortBySeq { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus_Commit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers { get; set; } = new JArray();//": "",
        [JsonProperty(PropertyName = "Ids")]
        public string Ids { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "SelectedPostId")]
        public int SelectedPostId { get; set; } = 0;//": "false"
        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": "",
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_StockStatus_Audit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers { get; set; } = new JArray();//": "",


        [JsonProperty(PropertyName = "Ids")]
        public string Ids { get; set; } = "";//": "",


        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags { get; set; } = "";//": "",
        
        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl { get; set; } = "";//": "",


        [JsonProperty(PropertyName = "IsVerifyProcInst")]
        public int IsVerifyProcInst { get; set; } = 0;//": "false"


    
        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": "",
    }
}
