using System;
using System.Collections.Generic;
using static Models.K3Cloud_Common;

namespace Models
{
    public class FBillTypeID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FCustomerID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSaleDeptID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FReceiverID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FHeadLocationId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FCarrierID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSalesGroupID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSalesManID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FDeliveryDeptID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FStockerGroupID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FStockerID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSettleID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FReceiverContactID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNAME { get; set; } = "";
    }

    public class FPayerID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FOwnerIdHead
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FRobamShop
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FRobamSaler
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSettleCurrID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSettleTypeID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FReceiptConditionID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FPriceListId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FDiscountListId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FLocalCurrID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FExchangeTypeID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class SubHeadEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int FEntryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FSettleCurrID FSettleCurrID { get; set; } = new FSettleCurrID() { FNumber = "PRE001" };
        /// <summary>
        /// 
        /// </summary>
        public string FThirdBillNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FThirdBillId { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FThirdSrcType { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FSettleTypeID FSettleTypeID { get; set; } = new FSettleTypeID();
        /// <summary>
        /// 
        /// </summary>
        public FReceiptConditionID FReceiptConditionID { get; set; } = new FReceiptConditionID();
        /// <summary>
        /// 
        /// </summary>
        public FPriceListId FPriceListId { get; set; } = new FPriceListId();
        /// <summary>
        /// 
        /// </summary>
        public FDiscountListId FDiscountListId { get; set; } = new FDiscountListId();
        /// <summary>
        /// 
        /// </summary>
        public string FIsIncludedTax { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FLocalCurrID FLocalCurrID { get; set; } = new FLocalCurrID();
        /// <summary>
        /// 
        /// </summary>
        public FExchangeTypeID FExchangeTypeID { get; set; } = new FExchangeTypeID();
        /// <summary>
        /// 
        /// </summary>
        public int FExchangeRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool FIsPriceExcludeTax { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public string FBuyerNick { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverAddress { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverName { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverMobile { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverCountry { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverState { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverCity { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverDistrict { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiverPhone { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FVIPCODE { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FAllDisCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FGYTAGCODE { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FGYTAGNAME { get; set; } = "";
    }

    public class FCustMatID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FMaterialID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FAuxPropId
    {
    }

    public class FUnitID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FParentMatId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FBomID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FOwnerID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FLot
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FTaxCombination
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FExtAuxUnitId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FStockID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FStockLocID
    {
    }

    public class FStockStatusID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FSalUnitID
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FEOwnerSupplierId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FESettleCustomerId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FPriceListEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FMaterialID_Sal
    {
        /// <summary>
        /// 
        /// </summary>
        public string FNumber { get; set; } = "";
    }

    public class FTaxDetailSubEntityItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FDetailID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FTaxRate { get; set; }
    }

    public class FSerialSubEntityItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FDetailID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FSerialNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FSerialNote { get; set; } = "";
    }

    public class FRobam_SubEntityItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FDetailID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FQrCodeText { get; set; } = "";
    }
    public class FEntity_Link
    {
        public int FLinkId { get; set; }
        public string FEntity_Link_FFlowId { get; set; } = "";
        public int FEntity_Link_FFlowLineId { get; set; }
        public string FEntity_Link_FRuleId { get; set; } = "";
        public int FEntity_Link_FSTableId { get; set; }
        public string FEntity_Link_FSTableName { get; set; } = "";
        public long FEntity_Link_FSBillId { get; set; }
        public long FEntity_Link_FSId { get; set; }
        public decimal FEntity_Link_FBaseUnitQtyOld { get; set; }
        public decimal FEntity_Link_FBaseUnitQty { get; set; }
        public decimal FEntity_Link_FSALBASEQTYOld { get; set; }
        public decimal FEntity_Link_FSALBASEQTY { get; set; }
        public decimal FEntity_Link_FAuxUnitQtyOld { get; set; }
        public decimal FEntity_Link_FAuxUnitQty { get; set; }
        public string FEntity_Link_FLnkTrackerId { get; set; } = "";
        public string FEntity_Link_FLnkSState { get; set; } = "";
        public decimal FEntity_Link_FLnkAmount { get; set; }
        public string FEntity_Link_FLnk1TrackerId { get; set; } = "";
        public string FEntity_Link_FLnk1SState { get; set; } = "";
        public decimal FEntity_Link_FLnk1Amount { get; set; }
    }
    public class FEntityItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FENTRYID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FRowType { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FCustMatID FCustMatID { get; set; } = new FCustMatID();
        /// <summary>
        /// 
        /// </summary>
        public FMaterialID FMaterialID { get; set; } = new FMaterialID();
        /// <summary>
        /// 
        /// </summary>
        public FAuxPropId FAuxPropId { get; set; } = new FAuxPropId();
        /// <summary>
        /// 
        /// </summary>
        public FUnitID FUnitID { get; set; } = new FUnitID();
        /// <summary>
        /// 
        /// </summary>
        public decimal FInventoryQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FParentMatId FParentMatId { get; set; } = new FParentMatId();
        /// <summary>
        /// 
        /// </summary>
        public decimal FRealQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FDisPriceQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FTaxPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FIsFree { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FBomID FBomID { get; set; } = new FBomID();
        /// <summary>
        /// 
        /// </summary>
        public string FOwnerTypeID { get; set; } = "BD_OwnerOrg";
        /// <summary>
        /// 
        /// </summary>
        public FOwnerID FOwnerID { get; set; } = new FOwnerID() { FNumber = "100" };
        /// <summary>
        /// 
        /// </summary>
        public FLot FLot { get; set; } = new FLot();
        /// <summary>
        /// 
        /// </summary>
        public string FProduceDate { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FExpiryDate { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FTaxCombination FTaxCombination { get; set; } = new FTaxCombination();
        /// <summary>
        /// 
        /// </summary>
        public int FEntryTaxRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FAuxUnitQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FExtAuxUnitId FExtAuxUnitId { get; set; } = new FExtAuxUnitId();
        /// <summary>
        /// 
        /// </summary>
        public decimal FExtAuxUnitQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FStockID FStockID { get; set; } = new FStockID();
        /// <summary>
        /// 
        /// </summary>
        public FStockLocID FStockLocID { get; set; } = new FStockLocID();
        /// <summary>
        /// 
        /// </summary>
        public FStockStatusID FStockStatusID { get; set; } = new FStockStatusID() { FNumber = "KCZT01_SYS" };
        /// <summary>
        /// 
        /// </summary>
        public string FQualifyType { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FMtoNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FEntrynote { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FDiscountRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FPriceDiscount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FActQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FSalUnitID FSalUnitID { get; set; } = new FSalUnitID();
        /// <summary>
        /// 
        /// </summary>
        public decimal FSALUNITQTY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FSALBASEQTY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FPRICEBASEQTY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FProjectNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FOUTCONTROL { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public decimal FRepairQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FIsCreateProDoc { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FEOwnerSupplierId FEOwnerSupplierId { get; set; } = new FEOwnerSupplierId();
        /// <summary>
        /// 
        /// </summary>
        public string FIsOverLegalOrg { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FESettleCustomerId FESettleCustomerId { get; set; } = new FESettleCustomerId();
        /// <summary>
        /// 
        /// </summary>
        public FPriceListEntry FPriceListEntry { get; set; } = new FPriceListEntry();
        /// <summary>
        /// 
        /// </summary>
        public decimal FARNOTJOINQTY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FQmEntryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FConvertEntryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FSOEntryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FBeforeDisPriceQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FSignQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FCheckDelivery { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FThirdEntryId { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FETHIRDBILLID { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FETHIRDBILLNO { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FAllAmountExceptDisCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FRobamPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal FRobamAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FSettleBySon { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FBOMEntryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FGYENTERTIME { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FMaterialID_Sal FMaterialID_Sal { get; set; } = new FMaterialID_Sal();
        /// <summary>
        /// 
        /// </summary>
        public string FInStockBillno { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FInStockEntryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<FTaxDetailSubEntityItem> FTaxDetailSubEntity { get; set; } = new List<FTaxDetailSubEntityItem>();
        /// <summary>
        /// 
        /// </summary>
        public List<FSerialSubEntityItem> FSerialSubEntity { get; set; } = new List<FSerialSubEntityItem>();
        /// <summary>
        /// 
        /// </summary>
        public List<FRobam_SubEntityItem> FRobam_SubEntity { get; set; } = new List<FRobam_SubEntityItem>();
        public List<FEntity_Link> FEntity_Link { get; set; } = new List<FEntity_Link>();
        //单据项次
        public int FItemNo { get; set; }
        public decimal FRobam_Price { get; set; }
        public decimal FRobam_Amount { get; set; }
    }

    public class FLogComId
    {
        /// <summary>
        /// 
        /// </summary>
        public string FCODE { get; set; } = "";
    }

    public class FOutStockTraceDetailItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FDetailID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FTraceTime { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FTraceDetail { get; set; } = "";
    }

    public class FOutStockTraceItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int FEntryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FLogComId FLogComId { get; set; } = new FLogComId();
        /// <summary>
        /// 
        /// </summary>
        public string FCarryBillNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FPhoneNumber { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FFrom { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FTo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FDelTime { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FTraceStatus { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiptTime { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public List<FOutStockTraceDetailItem> FOutStockTraceDetail { get; set; } = new List<FOutStockTraceDetailItem>();
    }

    public class Model
    {
        /// <summary>
        /// 
        /// </summary>
        public int FID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FBillTypeID FBillTypeID { get; set; } = new FBillTypeID();
        /// <summary>
        /// 
        /// </summary>
        public string FBillNo { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 
        /// </summary>
        public FCustomerID FCustomerID { get; set; } = new FCustomerID();
        /// <summary>
        /// 
        /// </summary>
        public FSaleDeptID FSaleDeptID { get; set; } = new FSaleDeptID();
        /// <summary>
        /// 
        /// </summary>
        public FReceiverID FReceiverID { get; set; } = new FReceiverID();
        /// <summary>
        /// 
        /// </summary>
        public FHeadLocationId FHeadLocationId { get; set; } = new FHeadLocationId();
        /// <summary>
        /// 
        /// </summary>
        public FCarrierID FCarrierID { get; set; } = new FCarrierID();
        /// <summary>
        /// 
        /// </summary>
        public string FCarriageNO { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FSalesGroupID FSalesGroupID { get; set; } = new FSalesGroupID();
        /// <summary>
        /// 
        /// </summary>
        public FSalesManID FSalesManID { get; set; } = new FSalesManID();
        /// <summary>
        /// 
        /// </summary>
        public FDeliveryDeptID FDeliveryDeptID { get; set; } = new FDeliveryDeptID();
        /// <summary>
        /// 
        /// </summary>
        public string FLinkMan { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FLinkPhone { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FStockerGroupID FStockerGroupID { get; set; } = new FStockerGroupID();
        /// <summary>
        /// 
        /// </summary>
        public FStockerID FStockerID { get; set; } = new FStockerID();
        /// <summary>
        /// 
        /// </summary>
        public string FNote { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FReceiveAddress { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FSettleID FSettleID { get; set; } = new FSettleID();
        /// <summary>
        /// 
        /// </summary>
        public FReceiverContactID FReceiverContactID { get; set; } = new FReceiverContactID();
        /// <summary>
        /// 
        /// </summary>
        public FPayerID FPayerID { get; set; } = new FPayerID(); 
        /// <summary>
        /// 
        /// </summary>
        public string FOwnerTypeIdHead { get; set; } = "BD_OwnerOrg";
        /// <summary>
        /// 
        /// </summary>
        public FOwnerIdHead FOwnerIdHead { get; set; } = new FOwnerIdHead() { FNumber = "100" };
        /// <summary>
        /// 
        /// </summary>
        public string FScanBox { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FCDateOffsetUnit { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public int FCDateOffsetValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FPlanRecAddress { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FIsTotalServiceOrCost { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FSHOPNUMBER { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FGYDATE { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FSALECHANNEL { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string FLogisticsNos { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public FRobamShop FRobamShop { get; set; } = new FRobamShop();
        /// <summary>
        /// 
        /// </summary>
        public FRobamSaler FRobamSaler { get; set; } = new FRobamSaler();
        /// <summary>
        /// 
        /// </summary>
        public SubHeadEntity SubHeadEntity { get; set; } = new SubHeadEntity();
        /// <summary>
        /// 
        /// </summary>
        public List<FEntityItem> FEntity { get; set; } = new List<FEntityItem>();
        /// <summary>
        /// 
        /// </summary>
        public List<FOutStockTraceItem> FOutStockTrace { get; set; } = new List<FOutStockTraceItem>();
        public string FRobamBillNo { get; set; } = "";
        public string FRobamDate { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FRobamCompany { get; set; } = new K3Cloud_FNumber();
    }

    public class K3Cloud_OutStockBill
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> NeedUpDateFields { get; set; } = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> NeedReturnFields { get; set; } = new List<string>(); 
        /// <summary>
        /// 
        /// </summary>
        public string IsDeleteEntry { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string SubSystemId { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string IsVerifyBaseDataField { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string IsEntryBatchFill { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string ValidateFlag { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string NumberSearch { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string IsAutoAdjustField { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string InterationFlags { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string IgnoreInterationFlag { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string IsControlPrecision { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string ValidateRepeatJson { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public Model Model { get; set; } = new Model();
    }
}
