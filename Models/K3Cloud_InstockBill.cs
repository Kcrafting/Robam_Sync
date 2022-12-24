using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static Models.K3Cloud_Common;

namespace Models
{

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_InstockBill
    {
        public JArray NeedUpDateFields { get; set; }
        public JArray NeedReturnFields { get; set; }
        public bool IsDeleteEntry{ get; set; }
        public string SubSystemId{ get; set; }
        public bool IsVerifyBaseDataField { get; set; }
        public bool IsEntryBatchFill { get; set; }
        public bool ValidateFlag { get; set; }
        public bool NumberSearch { get; set; }
        public bool IsAutoAdjustField { get; set; }
        public string InterationFlags{ get; set; }
        public string IgnoreInterationFlag{ get; set; }
        public bool IsControlPrecision { get; set; }
        public bool ValidateRepeatJson { get; set; }
        public K3Cloud_InstockBill_Model Model { get; set; } = new K3Cloud_InstockBill_Model();
    }
    public class K3Cloud_InstockBill_Model
    {
        public int FID { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FBillTypeID { get; set; } = new K3Cloud_FNumber();
        public string FBillNo { get; set; }
        public string FDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public K3Cloud_Common.K3Cloud_FNumber FStockDeptId { get; set; } = new K3Cloud_FNumber() { FNumber = ""};
        public K3Cloud_Common.K3Cloud_FNumber FStockerGroupId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FStockerId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FPurchaseDeptId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FPurchaserGroupId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FPurchaserId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FSupplierId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FSupplyId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public string FSupplyAddress { get; set; } 
        public K3Cloud_Common.K3Cloud_FNumber FSettleId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FChargeId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public string FOwnerTypeIdHead { get; set; } = "BD_OwnerOrg";
        public K3Cloud_Common.K3Cloud_FNumber FOwnerIdHead { get; set; } = new K3Cloud_FNumber() { FNumber = "100" };
        public K3Cloud_Common.K3Cloud_FUserID FConfirmerId { get; set; } = new K3Cloud_FUserID();
        public string FConfirmDate { get; set; }
        public string FScanBox { get; set; }
        public string FCDateOffsetUnit { get; set; }
        public int FCDateOffsetValue { get; set; }
        public K3Cloud_InstockBill_Model_FProviderContactID FProviderContactID { get; set; } = new K3Cloud_InstockBill_Model_FProviderContactID();
        public string FSplitBillType { get; set; }
        public string FSupplyEMail { get; set; }
        public string FRobamBillNo { get; set; }
        public string FRobamDate { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FRobamCompany { get; set; } = new K3Cloud_FNumber();
        public K3Cloud_InstockBill_Model_FInStockFin FInStockFin { get; set; } = new K3Cloud_InstockBill_Model_FInStockFin();
        public List<K3Cloud_InstockBill_Model_FInStockEntry> FInStockEntry { get; set; } = new List<K3Cloud_InstockBill_Model_FInStockEntry>() { };
        public string FJsonText { get; set; }
    }
    public class K3Cloud_InstockBill_Model_FProviderContactID
    {
        [JsonProperty(PropertyName = "FCONTACTNUMBER")]
        public string FCONTACTNUMBER { get; set; } = "";
    }
    public class K3Cloud_InstockBill_Model_FInStockFin
    {
        public int FEntryId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FSettleTypeId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FPayConditionId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };    
        public K3Cloud_Common.K3Cloud_FNumber FSettleCurrId { get; set; } = new K3Cloud_FNumber() { FNumber = "PRE001" };
        public bool FIsIncludedTax { get; set; } = false;
        public string FPriceTimePoint { get; set; } = "1";
        public K3Cloud_Common.K3Cloud_FNumber FPriceListId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };   
        public K3Cloud_Common.K3Cloud_FNumber FDiscountListId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };    
        public K3Cloud_Common.K3Cloud_FNumber FLocalCurrId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };   
        public K3Cloud_Common.K3Cloud_FNumber FExchangeTypeId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public int FExchangeRate { get; set; } = 1;
        public bool FISPRICEEXCLUDETAX { get; set; } = false;
        public int FAllDisCount { get; set; } = 0;
    }
    public class K3Cloud_InstockBill_Model_FInStockEntry
    {
        public int FEntryId { get; set; }
        public string FRowType { get; set; }
        public string FWWInType { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FMaterialId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };    
        public K3Cloud_Common.K3Cloud_FNumber FUnitID { get; set; } = new K3Cloud_FNumber() { FNumber = "" };    
        public string FMaterialDesc { get; set; }
        public JObject FAuxPropId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FParentMatId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };   
        public decimal FWWPickMtlQty { get; set; }
        public decimal FRealQty { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FPriceUnitID { get; set; } = new K3Cloud_FNumber() { FNumber = "" };   
        public decimal FPrice { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FLot { get; set; } = new K3Cloud_FNumber() { FNumber = "" };   
        public K3Cloud_Common.K3Cloud_FNumber FTaxCombination { get; set; } = new K3Cloud_FNumber() { FNumber = "" };    
        public K3Cloud_Common.K3Cloud_FNumber FStockId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public decimal FDisPriceQty { get; set; }
        public JObject FStockLocId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FStockStatusId { get; set; } = new K3Cloud_FNumber() { FNumber = "KCZT01_SYS" };
        public string FMtoNo { get; set; }
        public bool FGiveAway { get; set; }
        public string FNote { get; set; }
        public string FProduceDate { get; set; }
        public string FOWNERTYPEID { get; set; } = "BD_OwnerOrg";
        public K3Cloud_Common.K3Cloud_FNumber FExtAuxUnitId { get; set; } = new K3Cloud_FNumber() { FNumber = "" };
        public decimal FExtAuxUnitQty { get; set; }
        public bool FCheckInComing { get; set; }
        public string FProjectNo { get; set; }
        public bool FIsReceiveUpdateStock { get; set; }
        public decimal FInvoicedJoinQty { get; set; }
        public decimal FPriceBaseQty { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FSetPriceUnitID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "" };
        public K3Cloud_Common.K3Cloud_FNumber FRemainInStockUnitId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "" };
        public bool FBILLINGCLOSE { get; set; }
        public decimal FRemainInStockQty { get; set; }
        public decimal FAPNotJoinQty { get; set; }
        public decimal FRemainInStockBaseQty { get; set; }
        public decimal FTaxPrice { get; set; }
        public decimal FEntryTaxRate { get; set; } = 1.0m;
        public decimal FDiscountRate { get; set; }
        public decimal FCostPrice { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FBOMId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "" };
        public string FSupplierLot { get; set; }
        public string FExpiryDate { get; set; }
        public decimal FAuxUnitQty { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FOWNERID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "100" };
        public int FAllAmountExceptDisCount { get; set; }
        public int FPriceDiscount { get; set; }
        public decimal FConsumeSumQty { get; set; }
        public decimal FRobam_Price { get; set; }
        public decimal FBaseConsumeSumQty { get; set; }
        public decimal FRobam_Amount { get; set; }
        public decimal FRejectsDiscountAmount { get; set; }
        public string FSalOutStockBillNo { get; set; }
        public int FSalOutStockEntryId { get; set; }
        public decimal FBeforeDisPriceQty { get; set; }
        
        public List<K3Cloud_InstockBill_Model_FInStockEntry_FEntryPruCost> FEntryPruCost { get; set; } = new List<K3Cloud_InstockBill_Model_FInStockEntry_FEntryPruCost>();
        public List<K3Cloud_InstockBill_Model_FInStockEntry_FTaxDetailSubEntity> FTaxDetailSubEntity { get; set; } = new List<K3Cloud_InstockBill_Model_FInStockEntry_FTaxDetailSubEntity>();
        public List<K3Cloud_InstockBill_Model_FInStockEntry_FSerialSubEntity> FSerialSubEntity { get; set; } = new List<K3Cloud_InstockBill_Model_FInStockEntry_FSerialSubEntity>();
        public List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity> FRobam_SubEntity { get; set; } = new List<K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity>();
        //2022-12-3 增加单据项次
        public int FItemNo { get; set; }
    }
    public class K3Cloud_InstockBill_Model_FInStockEntry_FEntryPruCost
    {
        public int FDetailID { get; set; }
        public decimal FCostAmount { get; set; }
    }
    public class K3Cloud_InstockBill_Model_FInStockEntry_FTaxDetailSubEntity
    {
        public int FDetailID { get; set; }
        public decimal FTaxRate { get; set; } = 1.0m;
    }
    public class K3Cloud_InstockBill_Model_FInStockEntry_FSerialSubEntity
    {
        public int FDetailID { get; set; }
        public string FSerialNo { get; set; }
        public string FSerialNote { get; set; }
    }

    public class K3Cloud_InstockBill_Model_FInStockEntry_FRobam_SubEntity
    {
        public int FDetailID { get; set; }
        public string FQrCodeText { get; set; }
    }
}
