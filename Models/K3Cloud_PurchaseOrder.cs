using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder
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
        public K3Cloud_PurchaseOrder_Model Model { get; set; } = new K3Cloud_PurchaseOrder_Model();//":

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model
    {
        [JsonProperty(PropertyName = "FID")]
        public int FID { get; set; } = 0;
        [JsonProperty(PropertyName = "FBillNo")]
        public string FBillNo { get; set; } = "";
        [JsonProperty(PropertyName = "FCompany")]
        public K3Cloud_PurchaseOrder_Model_FCompany FCompany { get; set; } = new K3Cloud_PurchaseOrder_Model_FCompany();
        [JsonProperty(PropertyName = "FBillTypeID")]
        public K3Cloud_PurchaseOrder_Model_FBillTypeID FBillTypeID { get; set; } = new K3Cloud_PurchaseOrder_Model_FBillTypeID();
        [JsonProperty(PropertyName = "FDate")]
        public string FDate { get; set; } = "";
        [JsonProperty(PropertyName = "FBookDate")]
        public string FBookDate { get; set; } = "";
        [JsonProperty(PropertyName = "FDisBillNo")]
        public string FDisBillNo { get; set; } = "";
        [JsonProperty(PropertyName = "FOrgBillTypeID")]
        public string FOrgBillTypeID { get; set; } = "";
        [JsonProperty(PropertyName = "FContact")]
        public string FContact { get; set; } = "";
        [JsonProperty(PropertyName = "FContactPhone")]
        public string FContactPhone { get; set; } = "";
        [JsonProperty(PropertyName = "FSendUnitType")]
        public string FSendUnitType { get; set; } = "";
        [JsonProperty(PropertyName = "FSendUnit")]
        public K3Cloud_PurchaseOrder_Model_FSendUnit FSendUnit { get; set; } = new K3Cloud_PurchaseOrder_Model_FSendUnit();
        [JsonProperty(PropertyName = "FSendInventory")]
        public K3Cloud_PurchaseOrder_Model_FSendInventory FSendInventory { get; set; } = new K3Cloud_PurchaseOrder_Model_FSendInventory();
        [JsonProperty(PropertyName = "FSendInventoryPlace")]
        public K3Cloud_PurchaseOrder_Model_FSendInventoryPlace FSendInventoryPlace { get; set; } = new K3Cloud_PurchaseOrder_Model_FSendInventoryPlace();
        [JsonProperty(PropertyName = "FReciveUnitType")]
        public string FReciveUnitType { get; set; } = "";
        [JsonProperty(PropertyName = "FReciveUnit")]
        public K3Cloud_PurchaseOrder_Model_FReciveUnit FReciveUnit { get; set; } = new K3Cloud_PurchaseOrder_Model_FReciveUnit();
        [JsonProperty(PropertyName = "FContactAddress")]
        public string FContactAddress { get; set; } = "";
        [JsonProperty(PropertyName = "FCustomer")]
        public string FCustomer { get; set; } = "";
        [JsonProperty(PropertyName = "FOrgBillNo")]
        public string FOrgBillNo { get; set; } = "";
        [JsonProperty(PropertyName = "FOrgID")]
        public K3Cloud_Common.K3Cloud_FNumber FOrgID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        [JsonProperty(PropertyName = "FSendCompany")]
        public K3Cloud_Common.K3Cloud_FNumber FSendCompany { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        [JsonProperty(PropertyName = "FReciveCompany")]
        public K3Cloud_Common.K3Cloud_FNumber FReciveCompany { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        [JsonProperty(PropertyName = "FRobam_PurchaseOrderEntity")]
        public List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity> FRobam_PurchaseOrderEntity { get; set; } = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity>();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FCompany
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FBillTypeID
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FSendUnit
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FSendInventory
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FSendInventoryPlace
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FReciveUnit
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;
        [JsonProperty(PropertyName = "FMaterialID")]
        public K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FMaterialID FMaterialID { get; set; } = new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FMaterialID();
        [JsonProperty(PropertyName = "FUnitID")]
        public K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FUnitID FUnitID { get; set; } = new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FUnitID();
        [JsonProperty(PropertyName = "FQty")]
        public decimal FQty { get; set; } = 0m;
        [JsonProperty(PropertyName = "FQty_Fact")]
        public decimal FQty_Fact { get; set; } = 0m;
        [JsonProperty(PropertyName = "FQty_Recive")]
        public decimal FQty_Recive { get; set; } = 0m;
        [JsonProperty(PropertyName = "FSourceBillNo")]
        public string FSourceBillNo { get; set; } = "";
        [JsonProperty(PropertyName = "FInventory")]
        public K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventory FInventory { get; set; } = new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventory();
        [JsonProperty(PropertyName = "FInventoryPlace")]
        public K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventoryPlace FInventoryPlace { get; set; } = new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventoryPlace();
        [JsonProperty(PropertyName = "FStockStatusId")]
        public K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FStockStatusId FStockStatusId { get; set; } = new K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FStockStatusId();
        [JsonProperty(PropertyName = "FRobam_PurchaseOrderCodeEntity")]
        public List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity> FRobam_PurchaseOrderCodeEntity { get; set; } = new List<K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity>();
        
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FMaterialID
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FUnitID
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventory
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FInventoryPlace
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FStockStatusId
    {
        [JsonProperty(PropertyName = "FNUMBER")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_PurchaseOrder_Model_FRobam_PurchaseOrderEntity_FRobam_PurchaseOrderCodeEntity
    {
        [JsonProperty(PropertyName = "FDetailID")]
        public int FDetailID { get; set; } = 0;
        [JsonProperty(PropertyName = "FQrCodeText")]
        public string FQrCodeText { get; set; } = "";
    }
}
