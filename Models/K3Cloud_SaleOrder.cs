using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_SaleOrder
    {
        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],
        public JArray NeedReturnFields { get; set; } = new JArray();//": [],
        public bool IsDeleteEntry { get; set; } = true;//": "true",
        public string SubSystemId { get; set; } = "";//": "",
        public bool IsVerifyBaseDataField { get; set; } = false;//": "false",
        public bool IsEntryBatchFill { get; set; } = true;//": "true",
        public bool ValidateFlag { get; set; } = true;//": "true",
        public bool NumberSearch { get; set; } = true;//": "true",
        public bool IsAutoAdjustField { get; set; } = false;//": "false",
        public string InterationFlags { get; set; } = "";//": "",
        public string IgnoreInterationFlag { get; set; } = "";//": "",
        public bool IsControlPrecision { get; set; } = false;//": "false",
        public K3Cloud_SaleOrder_Model Model { get; set; } = new K3Cloud_SaleOrder_Model();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_SaleOrder_Model
    {
        public int FID { get; set; } = 0;//": 0,
        public string FBillNo { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FUserID F_PLYE_CreatorId { get; set; } = new K3Cloud_Common.K3Cloud_FUserID();//": {
        public string FOrgBillNo { get; set; } = "";
        public bool FIsHistoryBill { get; set; } = false;
        public K3Cloud_Common.K3Cloud_FNumber FJSCurrency { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() {FNumber = "PRE001" };
        public K3Cloud_Common.K3Cloud_FNumber FJSOrg { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "100" };
        public string F_PLYE_CreateDate { get; set; } = "2022-10-1";//": "1900-01-01",
        public string FDate { get; set; } = "";//": "1900-01-01",
        public int FTotoleAmountz { get; set; } = 0;//": 0,
        public int FReceiveAmount { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FSaler { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();////": {
        public K3Cloud_Common.K3Cloud_FNumber FShop { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FChannel { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FCustomer { get; set; } = "";//": "",
        public string FCustomerPhone { get; set; } = "";//": "",
        public string FCustomerAddress { get; set; } = "";//": "",
        public string FCustomerAddress_Tag { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBillTypeID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FOrgID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public List<K3Cloud_SaleOrder_Model_FRobam_SaleOrderEntity> FRobam_SaleOrderEntity { get; set; } = new List<K3Cloud_SaleOrder_Model_FRobam_SaleOrderEntity>();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_SaleOrder_Model_FRobam_SaleOrderEntity
    {
        public int FEntryID { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FMaterialID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FUnitID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public decimal FQty { get; set; } = 0;//": 0,
        public decimal FPrice { get; set; } = 0;//": 0,
        public decimal FAmount { get; set; } = 0;//": 0,
        public bool FIsFree  { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FOutStock { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FTicketPrice { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FStockStatusId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": "",  
        //2022-12-3 单据项次
        public int FItemNo { get; set; }
    }
}
