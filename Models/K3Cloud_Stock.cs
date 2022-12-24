using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock
    {
        [JsonProperty(PropertyName = "NeedUpDateFields")]
        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],

        [JsonProperty(PropertyName = "NeedReturnFields")]
        public JArray NeedReturnFields { get; set; } = new JArray();//": [],

        [JsonProperty(PropertyName = "IsDeleteEntry")]
        public bool IsDeleteEntry { get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "SubSystemId")]
        public string SubSystemId { get; set; }//": "",

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
        public K3Cloud_Stock_Model Model { get; set; } = new K3Cloud_Stock_Model();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model
    {
        [JsonProperty(PropertyName = "FStockId")]
        public int FStockId { get; set; } = 0;
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; } = "";
        [JsonProperty(PropertyName = "FThirdStockType")]
        public string FThirdStockType { get; set; } = "";
        [JsonProperty(PropertyName = "FStockProperty")]
        public string FStockProperty { get; set; } = "";
        [JsonProperty(PropertyName = "FSupplierId")]
        public K3Cloud_Stock_Model_FSupplierId FSupplierId { get; set; } = new K3Cloud_Stock_Model_FSupplierId();
        [JsonProperty(PropertyName = "FCustomerId")]
        public K3Cloud_Stock_Model_FCustomerId FCustomerId { get; set; } = new K3Cloud_Stock_Model_FCustomerId();
        [JsonProperty(PropertyName = "FTHIRDSTOCKNO")]
        public string FTHIRDSTOCKNO { get; set; } = "";
        [JsonProperty(PropertyName = "FAddress")]
        public string FAddress { get; set; } = "";
        [JsonProperty(PropertyName = "FPrincipal")]
        public object FPrincipal { get; set; } = new object();
        [JsonProperty(PropertyName = "FGYStockNumber")]
        public string FGYStockNumber { get; set; } = "";
        [JsonProperty(PropertyName = "FTel")]
        public string FTel { get; set; } = "";
        [JsonProperty(PropertyName = "FGroup")]
        public K3Cloud_Stock_Model_FGroup FGroup { get; set; } = new K3Cloud_Stock_Model_FGroup();
        [JsonProperty(PropertyName = "FDescription")]
        public string FDescription { get; set; } = "";
        [JsonProperty(PropertyName = "FStockStatusType")]
        public string FStockStatusType { get; set; } = "0,1,2,3,4,5,6,7,8";
        [JsonProperty(PropertyName = "FDefReceiveStatusId")]
        public K3Cloud_Stock_Model_FDefReceiveStatusId FDefReceiveStatusId { get; set; } = new K3Cloud_Stock_Model_FDefReceiveStatusId();
        [JsonProperty(PropertyName = "FDefStockStatusId")]
        public K3Cloud_Stock_Model_FDefStockStatusId FDefStockStatusId { get; set; } = new K3Cloud_Stock_Model_FDefStockStatusId();
        [JsonProperty(PropertyName = "FAllowMinusQty")]
        public bool FAllowMinusQty { get; set; } = false;
        [JsonProperty(PropertyName = "FIsGYStock")]
        public bool FIsGYStock { get; set; } = false;
        [JsonProperty(PropertyName = "FAllowLock")]
        public bool FAllowLock { get; set; } = false;
        [JsonProperty(PropertyName = "FNotExpQty")]
        public bool FNotExpQty { get; set; } = false;
        [JsonProperty(PropertyName = "FIsOpenLocation")]
        public bool FIsOpenLocation { get; set; } = false;
        [JsonProperty(PropertyName = "FAllowMRPPlan")]
        public bool FAllowMRPPlan { get; set; } = false;
        [JsonProperty(PropertyName = "FAvailablePicking")]
        public bool FAvailablePicking { get; set; } = false;
        [JsonProperty(PropertyName = "FAvailableAlert")]
        public bool FAvailableAlert { get; set; } = false;
        [JsonProperty(PropertyName = "FSortingPriority")]
        public int FSortingPriority { get; set; } = 0;
        [JsonProperty(PropertyName = "FLocListFormatter")]
        public string FLocListFormatter { get; set; } = "";
        [JsonProperty(PropertyName = "FDeptId")]
        public K3Cloud_Stock_Model_FDeptId FDeptId { get; set; } = new K3Cloud_Stock_Model_FDeptId();
        [JsonProperty(PropertyName = "FStockFlexItem")]
        public List<K3Cloud_Stock_Model_FStockFlexItem> FStockFlexItem { get; set; } = new List<K3Cloud_Stock_Model_FStockFlexItem>();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FSupplierId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FCustomerId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FGroup
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FDefReceiveStatusId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "KCZT01_SYS";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FDefStockStatusId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "KCZT01_SYS";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FDeptId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FStockFlexItem
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;
        [JsonProperty(PropertyName = "FFlexId")]
        public K3Cloud_Stock_Model_FStockFlexItem_FFlexId FFlexId { get; set; } = new K3Cloud_Stock_Model_FStockFlexItem_FFlexId();
        [JsonProperty(PropertyName = "FIsMustInput")]
        public bool FIsMustInput { get; set; } = false;
        [JsonProperty(PropertyName = "FStockFlexDetail")]
        public K3Cloud_Stock_Model_FStockFlexItem_FStockFlexDetail FStockFlexDetail { get; set; } = new K3Cloud_Stock_Model_FStockFlexItem_FStockFlexDetail();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FStockFlexItem_FFlexId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FStockFlexItem_FStockFlexDetail
    {
        [JsonProperty(PropertyName = "FDetailID")]
        public int FDetailID { get; set; } = 0;
        [JsonProperty(PropertyName = "FFlexEntryId")]
        public K3Cloud_Stock_Model_FStockFlexItem_FStockFlexDetail_FFlexEntryId FFlexEntryId { get; set; }
        [JsonProperty(PropertyName = "FIsRepeat")]
        public string FIsRepeat { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Stock_Model_FStockFlexItem_FStockFlexDetail_FFlexEntryId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
}
