using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    public class K3Cloud_Bill_Model_XSTHD_Model
    {
        public int FID { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FBillTypeID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "XSTHD01_SYS" };
        public string FBillNo { get; set; }
        public string FDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public K3Cloud_Common.K3Cloud_FNumber FRetcustId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "001" };
        public K3Cloud_Common.K3Cloud_FNumber FReturnReason { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FHeadLocId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FTransferBizType { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FSaleGroupId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FSalesManId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FStockDeptId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FStockerGroupId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FStockerId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FHeadNote { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FReceiveCustId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FReceiveAddress { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FSettleCustId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FName FReceiveCusContact { get; set; } = new K3Cloud_Common.K3Cloud_FName();
        public K3Cloud_Common.K3Cloud_FNumber FPayCustId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FOwnerTypeIdHead { get; set; } = "BD_OwnerOrg";
        public K3Cloud_Common.K3Cloud_FNumber FOwnerIdHead { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "100" };
        public string FScanBox { get; set; }
        public string FCDateOffsetUnit { get; set; }
        public int FCDateOffsetValue { get; set; }
        public string FIsTotalServiceOrCost { get; set; } = "false";
        public string FSHOPNUMBER { get; set; }
        public string FGYDATE { get; set; } = "1900-01-01";
        public string FGYExpressNo { get; set; } = "";
        public string FLinkMan { get; set; } = "";
        public string FLinkPhone { get; set; } = "";
        public K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity SubHeadEntity { get; set; } = new K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity();
        public List<K3Cloud_Bill_Model_XSTHD_Model_FEntity> FEntity { get; set; } = new List<K3Cloud_Bill_Model_XSTHD_Model_FEntity>();
        public List<K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace> FRetStockTrace { get; set; } = new List<K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace>();
    }

    public class K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity
    {
        public int FEntryId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FSettleCurrId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "PRE001" };
        public string FThirdBillNo { get; set; } = "";
        public string FThirdBillId { get; set; } = "";
        public string FThirdSrcType { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FSettleTypeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FChageCondition { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FPriceListId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FDiscountListId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FLocalCurrId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FExchangeTypeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public int FExchangeRate { get; set; } = 0;
        public string FBuyerNick { get; set; } = "";
        public string FReceiverAddress { get; set; } = "";
        public string FReceiverName { get; set; } = "";
        public string FReceiverMobile { get; set; } = "";
        public string FReceiverCountry { get; set; } = "";
        public string FReceiverState { get; set; } = "";
        public string FReceiverCity { get; set; } = "";
        public string FReceiverDistrict { get; set; } = "";
        public string FReceiverPhone { get; set; } = "";
        public string FVIPCODE { get; set; } = "";
        public string FGYTAGCODE { get; set; } = "";
        public string FGYTAGNAME { get; set; } = "";
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FTaxDetailSubEntity
    {
        public int FDetailID { get; set; }
        public int FTaxRate { get; set; }
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FSerialSubEntity
    {
        public int FDetailID { get; set; }
        public string FSerialNo { get; set; }
        public string FSerialNote { get; set; }
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_FEntity
    {
        public int FENTRYID { get; set; }
        public string FRowType { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FMapId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FMaterialId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public JObject FAuxpropId { get; set; } = new JObject();
        public K3Cloud_Common.K3Cloud_FNumber FUnitID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FInventoryQty { get; set; }
        public decimal FRealQty { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FParentMatId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FPrice { get; set; }
        public decimal FTaxPrice { get; set; }
        public string FIsFree { get; set; } = "false";
        public K3Cloud_Common.K3Cloud_FNumber FTaxCombination { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FEntryTaxRate { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FBOMId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FReturnType { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "THLX01_SYS" };
        public string FOwnerTypeId { get; set; } = "BD_OwnerOrg";
        public K3Cloud_Common.K3Cloud_FNumber FOwnerId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber() { FNumber = "100"};
        public K3Cloud_Common.K3Cloud_FNumber FStockId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public JObject FStocklocId { get; set; } = new JObject();
        public K3Cloud_Common.K3Cloud_FNumber FStockstatusId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FLot { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FDeliveryDate { get; set; } = "1900-01-01";
        public string FMtoNo { get; set; }
        public string FProduceDate { get; set; } = "1900-01-01";
        public string FExpiryDate { get; set; } = "1900-01-01";
        public string FNote { get; set; }
        public decimal FDiscountRate { get; set; }
        public decimal FPriceDiscount { get; set; }
        public decimal FAuxUnitQty { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FExtAuxUnitId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FExtAuxUnitQty { get; set; }
        public string FISCONSUMESUM { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FSalUnitID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FSalUnitQty { get; set; }
        public decimal FSalBaseQty { get; set; }
        public decimal FPriceBaseQty { get; set; }
        public string FProjectNo { get; set; }
        public string FQualifyType { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FEOwnerSupplierId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FIsOverLegalOrg { get; set; } = "false";
        public K3Cloud_Common.K3Cloud_FNumber FESettleCustomerId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FSOEntryId { get; set; }
        public string FThirdEntryId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FPriceListEntry { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public decimal FARNOTJOINQTY { get; set; }
        public string FIsReturnCheck { get; set; } = "false";
        public string FETHIRDBILLID { get; set; }
        public string FETHIRDBILLNO { get; set; }
        public string FSettleBySon { get; set; } = "false";
        public int FBOMEntryId { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FMaterialID_Sal { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FMrbBillNo { get; set; }
        public int FMrbEntryId { get; set; }
        public K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FTaxDetailSubEntity FTaxDetailSubEntity { get; set; } = new K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FTaxDetailSubEntity();
        public K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FSerialSubEntity FSerialSubEntity { get; set; } = new K3Cloud_Bill_Model_XSTHD_Model_SubHeadEntity_FSerialSubEntity();
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FLogComId
    {
        public string FCODE { get; set; }
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FReturnStockTraceDetail
    {
        public int FDetailID { get; set; }
        public string FTraceTime { get; set; }
        public string FTraceDetail { get; set; }
    }
    public class K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace
    {
        public int FEntryID { get; set; }
        public K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FLogComId FLogComId { get; set; } = new K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FLogComId();
        public string FCarryBillNo { get; set; } = "";
        public string FPhoneNumber { get; set; }
        public string FFrom { get; set; }
        public string FTo { get; set; }
        public string FDelTime { get; set; } = "1900-01-01";
        public string FTraceStatus { get; set; }
        public string FReceiptTime { get; set; } = "1900-01-01";
        public K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FReturnStockTraceDetail FReturnStockTraceDetail = new K3Cloud_Bill_Model_XSTHD_Model_FRetStockTrace_FReturnStockTraceDetail();
    }
    public class K3Cloud_Bill_Model_XSTHD : K3Cloud_Common.K3Cloud_Bill_SaveHeader<K3Cloud_Bill_Model_XSTHD_Model>
    {
    }


}
