using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item
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
        public bool IsEntryBatchFill{ get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "ValidateFlag")]
        public bool ValidateFlag{ get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "NumberSearch")]
        public bool NumberSearch{ get; set; } = true;//": "true",

        [JsonProperty(PropertyName = "IsAutoAdjustField")]
        public bool IsAutoAdjustField{ get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "IsControlPrecision")]
        public bool IsControlPrecision{ get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "Model")]
        public K3Cloud_Item_Model Model { get; set; }  = new K3Cloud_Item_Model();

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model
    {
        [JsonProperty(PropertyName = "FMATERIALID")]
        public int FMATERIALID { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FSpecification")]
        public string FSpecification { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FMnemonicCode")]
        public string FMnemonicCode { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FOldNumber")]
        public string FOldNumber { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FDescription")]
        public string FDescription { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FMaterialGroup")]
        public K3Cloud_Item_Model_FMaterialGroup FMaterialGroup { get; set; }  = new K3Cloud_Item_Model_FMaterialGroup();//": {

        [JsonProperty(PropertyName = "FDSMatchByLot")]
        public bool FDSMatchByLot { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FImgStorageType")]
        public string FImgStorageType { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FIsSalseByNet")]
        public bool FIsSalseByNet { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FForbidReson")]
        public string FForbidReson { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FExtVar")]
        public string FExtVar { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FSubHeadEntity")]
        public K3Cloud_Item_Model_FSubHeadEntity FSubHeadEntity { get; set; }  = new K3Cloud_Item_Model_FSubHeadEntity();

        [JsonProperty(PropertyName = "SubHeadEntity")]
        public K3Cloud_Item_Model_SubHeadEntity SubHeadEntity { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity();

        [JsonProperty(PropertyName = "SubHeadEntity1")]
        public K3Cloud_Item_Model_SubHeadEntity1 SubHeadEntity1 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity1();

        [JsonProperty(PropertyName = "SubHeadEntity2")]
        public K3Cloud_Item_Model_SubHeadEntity2 SubHeadEntity2 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity2();

        [JsonProperty(PropertyName = "SubHeadEntity3")]
        public K3Cloud_Item_Model_SubHeadEntity3 SubHeadEntity3 { get; set; } = new K3Cloud_Item_Model_SubHeadEntity3();

        [JsonProperty(PropertyName = "SubHeadEntity4")]
        public K3Cloud_Item_Model_SubHeadEntity4 SubHeadEntity4 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4();

        [JsonProperty(PropertyName = "SubHeadEntity5")]
        public K3Cloud_Item_Model_SubHeadEntity5 SubHeadEntity5 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5();

        [JsonProperty(PropertyName = "SubHeadEntity6")]
        public K3Cloud_Item_Model_SubHeadEntity6 SubHeadEntity6 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity6();

        [JsonProperty(PropertyName = "SubHeadEntity7")]
        public K3Cloud_Item_Model_SubHeadEntity7 SubHeadEntity7 { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity7();

        [JsonProperty(PropertyName = "FBarCodeEntity_CMK")]
        public List<K3Cloud_Item_Model_FBarCodeEntity_CMK> FBarCodeEntity_CMK { get; set; }  = new List<K3Cloud_Item_Model_FBarCodeEntity_CMK>() { };

        [JsonProperty(PropertyName = "FSpecialAttributeEntity")]
        public List<K3Cloud_Item_Model_FSpecialAttributeEntity> FSpecialAttributeEntity { get; set; }  = new List<K3Cloud_Item_Model_FSpecialAttributeEntity>() { };

        [JsonProperty(PropertyName = "FEntityAuxPty")]
        public List<K3Cloud_Item_Model_FEntityAuxPty> FEntityAuxPty { get; set; }  = new List<K3Cloud_Item_Model_FEntityAuxPty>() { };

        [JsonProperty(PropertyName = "FEntityInvPty")]
        public List<K3Cloud_Item_Model_FEntityInvPty> FEntityInvPty { get; set; }  = new List<K3Cloud_Item_Model_FEntityInvPty>() { };
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FMaterialGroup
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FSubHeadEntity
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsControlSal")]
        public bool FIsControlSal { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FLowerPercent")]
        public int FLowerPercent { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FUpPercent")]
        public int FUpPercent { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FCalculateBase")]
        public string FCalculateBase { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FMaxSalPrice_CMK")]
        public int FMaxSalPrice_CMK { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinSalPrice_CMK")]
        public int FMinSalPrice_CMK { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsAutoRemove")]
        public bool FIsAutoRemove { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsMailVirtual")]
        public bool FIsMailVirtual { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsFreeSend")]
        public string FIsFreeSend { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FTimeUnit")]
        public string FTimeUnit { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FRentFreeDura")]
        public int FRentFreeDura { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPricingStep")]
        public int FPricingStep { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinRentDura")]
        public int FMinRentDura { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FRentBeginPrice")]
        public int FRentBeginPrice { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPriceType")]
        public string FPriceType { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FRentStepPrice")]
        public int FRentStepPrice { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FDepositAmount")]
        public int FDepositAmount { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FLogisticsCount")]
        public int FLogisticsCount { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FRequestMinPackQty")]
        public int FRequestMinPackQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinRequestQty")]
        public int FMinRequestQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FRetailUnitID")]
        public K3Cloud_Item_Model_FSubHeadEntity_FRetailUnitID FRetailUnitID { get; set; }  = new K3Cloud_Item_Model_FSubHeadEntity_FRetailUnitID();//": {

        [JsonProperty(PropertyName = "FIsPrinttAg")]
        public bool FIsPrinttAg { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAccessory")]
        public bool FIsAccessory { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FSubHeadEntity_FRetailUnitID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FBARCODE")]
        public string FBARCODE { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FErpClsID")]
        public string FErpClsID { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FFeatureItem")]
        public string FFeatureItem { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FCONFIGTYPE")]
        public string FCONFIGTYPE { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FCategoryID")]
        public K3Cloud_Item_Model_SubHeadEntity_FCategoryID FCategoryID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FCategoryID();//": {

        [JsonProperty(PropertyName = "FTaxType")]
        public K3Cloud_Item_Model_SubHeadEntity_FTaxType FTaxType { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FTaxType();//": {

        [JsonProperty(PropertyName = "FTaxRateId")]
        public K3Cloud_Item_Model_SubHeadEntity_FTaxRateId FTaxRateId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FTaxRateId();//": {

        [JsonProperty(PropertyName = "FBaseUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity_FBaseUnitId FBaseUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FBaseUnitId();//": {
        public bool FIsPurchase { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsInventory")]
        public bool FIsInventory { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsSubContract")]
        public bool FIsSubContract { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsSale")]
        public bool FIsSale { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsProduce")]
        public bool FIsProduce { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsAsset")]
        public bool FIsAsset { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FGROSSWEIGHT")]
        public int FGROSSWEIGHT { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FNETWEIGHT")]
        public int FNETWEIGHT { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FWEIGHTUNITID")]
        public K3Cloud_Item_Model_SubHeadEntity_FWEIGHTUNITID FWEIGHTUNITID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FWEIGHTUNITID();//": {

        [JsonProperty(PropertyName = "FLENGTH")]
        public int FLENGTH { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FWIDTH")]
        public int FWIDTH { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FHEIGHT")]
        public int FHEIGHT { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FVOLUME")]
        public int FVOLUME { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FVOLUMEUNITID")]
        public K3Cloud_Item_Model_SubHeadEntity_FVOLUMEUNITID FVOLUMEUNITID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity_FVOLUMEUNITID();//": {

        [JsonProperty(PropertyName = "FSuite")]
        public string FSuite { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FCostPriceRate")]
        public int FCostPriceRate { get; set; } = 0;//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FCategoryID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FTaxType
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FTaxRateId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FBaseUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FWEIGHTUNITID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity_FVOLUMEUNITID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNUMBER { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FStoreUnitID")]
        public K3Cloud_Item_Model_SubHeadEntity1_FStoreUnitID FStoreUnitID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity1_FStoreUnitID();//": {

        [JsonProperty(PropertyName = "FAuxUnitID")]
        public K3Cloud_Item_Model_SubHeadEntity1_FAuxUnitID FAuxUnitID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity1_FAuxUnitID();//": {

        [JsonProperty(PropertyName = "FUnitConvertDir")]
        public string FUnitConvertDir { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FStockId")]
        public K3Cloud_Item_Model_SubHeadEntity1_FStockId FStockId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity1_FStockId();//": {

        [JsonProperty(PropertyName = "FStockPlaceId")]
        public object FStockPlaceId { get; set; } = new object();//": {},

        [JsonProperty(PropertyName = "FIsLockStock")]
        public bool FIsLockStock { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FIsCycleCounting")]
        public bool FIsCycleCounting { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FCountCycle")]
        public string FCountCycle { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FCountDay")]
        public int FCountDay { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsMustCounting")]
        public bool FIsMustCounting { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FIsBatchManage")]
        public bool FIsBatchManage { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FBatchRuleID")]
        public K3Cloud_Item_Model_SubHeadEntity1_FBatchRuleID FBatchRuleID { get; set; } = new K3Cloud_Item_Model_SubHeadEntity1_FBatchRuleID();//": {

        [JsonProperty(PropertyName = "FIsKFPeriod")]
        public bool FIsKFPeriod { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FIsExpParToFlot")]
        public bool FIsExpParToFlot { get; set; } = false;//": public bool False",

        [JsonProperty(PropertyName = "FExpUnit")]
        public string FExpUnit { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FExpPeriod")]
        public int FExpPeriod { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FOnlineLife")]
        public int FOnlineLife { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FRefCost")]
        public int FRefCost { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FCurrencyId")]
        public K3Cloud_Item_Model_SubHeadEntity1_FCurrencyId FCurrencyId { get; set; } = new K3Cloud_Item_Model_SubHeadEntity1_FCurrencyId();//": {

        [JsonProperty(PropertyName = "FIsEnableMinStock")]
        public bool FIsEnableMinStock { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FIsEnableMaxStock")]
        public bool FIsEnableMaxStock { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FIsEnableSafeStock")]
        public bool FIsEnableSafeStock { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FIsEnableReOrder")]
        public bool FIsEnableReOrder { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FMinStock")]
        public int FMinStock { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FSafeStock")]
        public int FSafeStock { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FReOrderGood")]
        public int FReOrderGood { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FEconReOrderQty")]
        public int FEconReOrderQty { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FMaxStock")]
        public int FMaxStock { get; set; } = 0;//": 0,


        [JsonProperty(PropertyName = "FIsSNManage")]
        public bool FIsSNManage { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FIsSNPRDTracy")]
        public bool FIsSNPRDTracy { get; set; } = false;//": public bool False",


        [JsonProperty(PropertyName = "FSNCodeRule")]
        public K3Cloud_Item_Model_SubHeadEntity1_FSNCodeRule FSNCodeRule { get; set; } = new K3Cloud_Item_Model_SubHeadEntity1_FSNCodeRule();//": {

        [JsonProperty(PropertyName = "FSNUnit")]
        public K3Cloud_Item_Model_SubHeadEntity1_FSNUnit FSNUnit { get; set; } = new K3Cloud_Item_Model_SubHeadEntity1_FSNUnit();//": {

        [JsonProperty(PropertyName = "FSNManageType")]
        public string FSNManageType { get; set; } = "1";//": "",


        [JsonProperty(PropertyName = "FSNGenerateTime")]
        public string FSNGenerateTime { get; set; } = "1";//": "",


        [JsonProperty(PropertyName = "FBoxStandardQty")]
        public int FBoxStandardQty { get; set; } = 0;//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FStoreUnitID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FAuxUnitID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FStockId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FBatchRuleID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FCurrencyId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FSNCodeRule
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity1_FSNUnit
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity2
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FSaleUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity2_FSaleUnitId FSaleUnitId { get; set; } = new K3Cloud_Item_Model_SubHeadEntity2_FSaleUnitId();//": {

        [JsonProperty(PropertyName = "FSalePriceUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity2_FSalePriceUnitId FSalePriceUnitId { get; set; } = new K3Cloud_Item_Model_SubHeadEntity2_FSalePriceUnitId();//": {

        [JsonProperty(PropertyName = "FOrderQty")]
        public int FOrderQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinQty")]
        public int FMinQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMaxQty")]
        public int FMaxQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FOutStockLmtH")]
        public int FOutStockLmtH { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FOutStockLmtL")]
        public int FOutStockLmtL { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FAgentSalReduceRate")]
        public int FAgentSalReduceRate { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsATPCheck")]
        public bool FIsATPCheck { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsReturnPart")]
        public bool FIsReturnPart { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsInvoice")]
        public bool FIsInvoice { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsReturn")]
        public bool FIsReturn { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FAllowPublish")]
        public bool FAllowPublish { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FISAFTERSALE")]
        public bool FISAFTERSALE { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FISPRODUCTFILES")]
        public bool FISPRODUCTFILES { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FISWARRANTED")]
        public bool FISWARRANTED { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FWARRANTY")]
        public int FWARRANTY { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FWARRANTYUNITID")]
        public string FWARRANTYUNITID { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FOutLmtUnit")]
        public string FOutLmtUnit { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FTaxCategoryCodeId")]
        public K3Cloud_Item_Model_SubHeadEntity2_FTaxCategoryCodeId FTaxCategoryCodeId { get; set; } = new K3Cloud_Item_Model_SubHeadEntity2_FTaxCategoryCodeId();//": {

        [JsonProperty(PropertyName = "FSalGroup")]
        public K3Cloud_Item_Model_SubHeadEntity2_FSalGroup FSalGroup { get; set; } = new K3Cloud_Item_Model_SubHeadEntity2_FSalGroup();//": {

        [JsonProperty(PropertyName = "FIsTaxEnjoy")]
        public bool FIsTaxEnjoy{ get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FTaxDiscountsType")]
        public string FTaxDiscountsType { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FUnValidateExpQty")]
        public bool FUnValidateExpQty { get; set; } = false;//": public string False"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity2_FSaleUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity2_FSalePriceUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity2_FTaxCategoryCodeId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity2_FSalGroup
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FBaseMinSplitQty")]
        public int FBaseMinSplitQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPurchaseUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FPurchaseUnitId FPurchaseUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FPurchaseUnitId();//": {

        [JsonProperty(PropertyName = "FPurchasePriceUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FPurchasePriceUnitId FPurchasePriceUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FPurchasePriceUnitId();//": {

        [JsonProperty(PropertyName = "FPurchaseGroupId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FPurchaseGroupId FPurchaseGroupId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FPurchaseGroupId();//": {

        [JsonProperty(PropertyName = "FPurchaserId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FPurchaserId FPurchaserId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FPurchaserId();//": {

        [JsonProperty(PropertyName = "FDefaultVendor")]
        public K3Cloud_Item_Model_SubHeadEntity3_FDefaultVendor FDefaultVendor { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FDefaultVendor();//": {

        [JsonProperty(PropertyName = "FChargeID")]
        public K3Cloud_Item_Model_SubHeadEntity3_FChargeID FChargeID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FChargeID();//": {

        [JsonProperty(PropertyName = "FIsQuota")]
        public bool FIsQuota { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FQuotaType")]
        public string FQuotaType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FMinSplitQty")]
        public int FMinSplitQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsVmiBusiness")]
        public bool FIsVmiBusiness { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FEnableSL")]
        public bool FEnableSL { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsPR")]
        public bool FIsPR { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsReturnMaterial")]
        public bool FIsReturnMaterial { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsSourceControl")]
        public bool FIsSourceControl { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FReceiveMaxScale")]
        public int FReceiveMaxScale { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FReceiveMinScale")]
        public int FReceiveMinScale { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FReceiveAdvanceDays")]
        public int FReceiveAdvanceDays { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FReceiveDelayDays")]
        public int FReceiveDelayDays { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPOBillTypeId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FPOBillTypeId FPOBillTypeId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FPOBillTypeId();//": {

        [JsonProperty(PropertyName = "FAgentPurPlusRate")]
        public int FAgentPurPlusRate { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FDefBarCodeRuleId")]
        public K3Cloud_Item_Model_SubHeadEntity3_FDefBarCodeRuleId FDefBarCodeRuleId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FDefBarCodeRuleId();//": {

        [JsonProperty(PropertyName = "FPrintCount")]
        public int FPrintCount { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinPackCount")]
        public int FMinPackCount { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FDailyOutQtySub")]
        public int FDailyOutQtySub { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FDefaultLineIdSub")]
        public K3Cloud_Item_Model_SubHeadEntity3_FDefaultLineIdSub FDefaultLineIdSub { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity3_FDefaultLineIdSub();//": {

        [JsonProperty(PropertyName = "FIsEnableScheduleSub")]
        public bool FIsEnableScheduleSub { get; set; } = false;//": public string False"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FPurchaseUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = ""; 
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FPurchasePriceUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FPurchaseGroupId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FPurchaserId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FDefaultVendor
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FChargeID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FPOBillTypeId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FDefBarCodeRuleId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity3_FDefaultLineIdSub
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanMode")]
        public string FPlanMode { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FBaseVarLeadTimeLotSize")]
        public int FBaseVarLeadTimeLotSize { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanningStrategy")]
        public string FPlanningStrategy { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FMfgPolicyId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FMfgPolicyId FMfgPolicyId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FMfgPolicyId();//": {

        [JsonProperty(PropertyName = "FOrderPolicy")]
        public string FOrderPolicy { get; set; } = "0";//": "",

        [JsonProperty(PropertyName = "FPlanWorkshop")]
        public K3Cloud_Item_Model_SubHeadEntity4_FPlanWorkshop FPlanWorkshop { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FPlanWorkshop();     //": {

        [JsonProperty(PropertyName = "FFixLeadTime")]
        public int FFixLeadTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FFixLeadTimeType")]
        public string FFixLeadTimeType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FVarLeadTime")]
        public int FVarLeadTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FVarLeadTimeType")]
        public string FVarLeadTimeType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FCheckLeadTime")]
        public int FCheckLeadTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FCheckLeadTimeType")]
        public string FCheckLeadTimeType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FOrderIntervalTimeType")]
        public string FOrderIntervalTimeType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FOrderIntervalTime")]
        public int FOrderIntervalTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMaxPOQty")]
        public int FMaxPOQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinPOQty")]
        public int FMinPOQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIncreaseQty")]
        public int FIncreaseQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FEOQ")]
        public int FEOQ { get; set; } = 1;//": 0,

        [JsonProperty(PropertyName = "FVarLeadTimeLotSize")]
        public int FVarLeadTimeLotSize { get; set; } = 1;//": 0,

        [JsonProperty(PropertyName = "FPlanIntervalsDays")]
        public int FPlanIntervalsDays { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanBatchSplitQty")]
        public int FPlanBatchSplitQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FRequestTimeZone")]
        public int FRequestTimeZone { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanTimeZone")]
        public int FPlanTimeZone { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanGroupId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FPlanGroupId FPlanGroupId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FPlanGroupId();//": {

        [JsonProperty(PropertyName = "FATOSchemeId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FATOSchemeId FATOSchemeId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FATOSchemeId();//": {

        [JsonProperty(PropertyName = "FPlanerID")]
        public K3Cloud_Item_Model_SubHeadEntity4_FPlanerID FPlanerID { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FPlanerID();//": {

        [JsonProperty(PropertyName = "FIsMrpComBill")]
        public bool FIsMrpComBill { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FCanLeadDays")]
        public int FCanLeadDays { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsMrpComReq")]
        public bool FIsMrpComReq { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FLeadExtendDay")]
        public int FLeadExtendDay { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FReserveType")]
        public string FReserveType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FPlanSafeStockQty")]
        public int FPlanSafeStockQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FAllowPartAhead")]
        public bool FAllowPartAhead { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FCanDelayDays")]
        public int FCanDelayDays { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FDelayExtendDay")]
        public int FDelayExtendDay { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FAllowPartDelay")]
        public bool FAllowPartDelay { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FPlanOffsetTimeType")]
        public string FPlanOffsetTimeType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FPlanOffsetTime")]
        public int FPlanOffsetTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FSupplySourceId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FSupplySourceId FSupplySourceId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FSupplySourceId();//": {

        [JsonProperty(PropertyName = "FTimeFactorId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FTimeFactorId FTimeFactorId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FTimeFactorId();//": {

        [JsonProperty(PropertyName = "FQtyFactorId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FQtyFactorId FQtyFactorId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FQtyFactorId();//": {

        [JsonProperty(PropertyName = "FProductLine")]
        public K3Cloud_Item_Model_SubHeadEntity4_FProductLine FProductLine { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FProductLine();//": {

        [JsonProperty(PropertyName = "FWriteOffQty")]
        public int FWriteOffQty { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FPlanIdent")]
        public K3Cloud_Item_Model_SubHeadEntity4_FPlanIdent FPlanIdent { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FPlanIdent();//": {

        [JsonProperty(PropertyName = "FProScheTrackId")]
        public K3Cloud_Item_Model_SubHeadEntity4_FProScheTrackId FProScheTrackId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity4_FProScheTrackId();//": {

        [JsonProperty(PropertyName = "FDailyOutQty")]
        public int FDailyOutQty { get; set; } = 0;//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FMfgPolicyId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FPlanWorkshop
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FPlanGroupId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FATOSchemeId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FPlanerID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FSupplySourceId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FTimeFactorId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FQtyFactorId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FProductLine
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FPlanIdent
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity4_FProScheTrackId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//{ get; set; }//": 0,

        [JsonProperty(PropertyName = "FWorkShopId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FWorkShopId FWorkShopId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FWorkShopId();//": {

        [JsonProperty(PropertyName = "FProduceUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FProduceUnitId FProduceUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FProduceUnitId();//": {

        [JsonProperty(PropertyName = "FFinishReceiptOverRate")]
        public int FFinishReceiptOverRate { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FFinishReceiptShortRate")]
        public int FFinishReceiptShortRate { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FProduceBillType")]
        public K3Cloud_Item_Model_SubHeadEntity5_FProduceBillType FProduceBillType { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FProduceBillType();//": {

        [JsonProperty(PropertyName = "FOrgTrustBillType")]
        public K3Cloud_Item_Model_SubHeadEntity5_FOrgTrustBillType FOrgTrustBillType { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FOrgTrustBillType();//": {

        [JsonProperty(PropertyName = "FIsSNCarryToParent")]
        public bool FIsSNCarryToParent { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsProductLine")]
        public bool FIsProductLine { get; set; } = false;  //": public string False",

        [JsonProperty(PropertyName = "FBOMUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FBOMUnitId FBOMUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FBOMUnitId();//": {

        [JsonProperty(PropertyName = "FLOSSPERCENT")]
        public int FLOSSPERCENT { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FConsumVolatility")]
        public int FConsumVolatility { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsMainPrd")]
        public bool FIsMainPrd { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsCoby")]
        public bool FIsCoby { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsECN")]
        public bool FIsECN { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIssueType")]
        public string FIssueType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FBKFLTime")]
        public string FBKFLTime { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FPickStockId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FPickStockId FPickStockId { get; set; } = new K3Cloud_Item_Model_SubHeadEntity5_FPickStockId();//": {

        [JsonProperty(PropertyName = "FPickBinId")]
        public object FPickBinId { get; set; } = new object();//": {},

        [JsonProperty(PropertyName = "FOverControlMode")]
        public string FOverControlMode { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FMinIssueQty")]
        public int FMinIssueQty { get; set; } = 1;//": 0,

        [JsonProperty(PropertyName = "FISMinIssueQty")]
        public bool FISMinIssueQty { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsKitting")]
        public bool FIsKitting { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FIsCompleteSet")]
        public bool FIsCompleteSet { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FDefaultRouting")]
        public K3Cloud_Item_Model_SubHeadEntity5_FDefaultRouting FDefaultRouting { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FDefaultRouting();//": {

        [JsonProperty(PropertyName = "FStdLaborPrePareTime")]
        public int FStdLaborPrePareTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FStdLaborProcessTime")]
        public int FStdLaborProcessTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FStdMachinePrepareTime")]
        public int FStdMachinePrepareTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FStdMachineProcessTime")]
        public int FStdMachineProcessTime { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FMinIssueUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FMinIssueUnitId FMinIssueUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FMinIssueUnitId();//": {

        [JsonProperty(PropertyName = "FMdlId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FMdlId FMdlId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FMdlId();//": {

        [JsonProperty(PropertyName = "FMdlMaterialId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FMdlMaterialId FMdlMaterialId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FMdlMaterialId();//": {

        [JsonProperty(PropertyName = "FStandHourUnitId")]
        public string FStandHourUnitId { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FBackFlushType")]
        public string FBackFlushType { get; set; } = "1";//": "",

        [JsonProperty(PropertyName = "FFIXLOSS")]
        public int FFIXLOSS { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FIsEnableSchedule")]
        public bool FIsEnableSchedule { get; set; } = false;//": public string False",

        [JsonProperty(PropertyName = "FDefaultLineId")]
        public K3Cloud_Item_Model_SubHeadEntity5_FDefaultLineId FDefaultLineId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity5_FDefaultLineId();//": {

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FWorkShopId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FProduceUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FProduceBillType
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FOrgTrustBillType
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FBOMUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FPickStockId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FDefaultRouting
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FMinIssueUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FMdlId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FMdlMaterialId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity5_FDefaultLineId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity6
    {
        [JsonProperty(PropertyName = "EntryId")]
        public int EntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "CheckIncoming")]
        public bool CheckIncoming { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckProduct")]
        public bool CheckProduct { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckStock")]
        public bool CheckStock { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckReturn")]
        public bool CheckReturn { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckDelivery")]
        public bool CheckDelivery { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "EnableCyclistQCSTK")]
        public bool EnableCyclistQCSTK { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "StockCycle")]
        public int StockCycle { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "EnableCyclistQCSTKEW")]
        public bool EnableCyclistQCSTKEW { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "EWLeadDay")]
        public int EWLeadDay { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "IncSampSchemeId")]
        public K3Cloud_Item_Model_SubHeadEntity6_FIncSampSchemeId IncSampSchemeId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity6_FIncSampSchemeId();//": {

        [JsonProperty(PropertyName = "IncQcSchemeId")]
        public K3Cloud_Item_Model_SubHeadEntity6_FIncQcSchemeId IncQcSchemeId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity6_FIncQcSchemeId();//": {

        [JsonProperty(PropertyName = "InspectGroupId")]
        public K3Cloud_Item_Model_SubHeadEntity6_FInspectGroupId InspectGroupId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity6_FInspectGroupId();//": {

        [JsonProperty(PropertyName = "InspectorId")]
        public K3Cloud_Item_Model_SubHeadEntity6_FInspectorId InspectorId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity6_FInspectorId();//": {

        [JsonProperty(PropertyName = "CheckEntrusted")]
        public bool CheckEntrusted { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckOther")]
        public bool CheckOther { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "IsFirstInspect")]
        public bool IsFirstInspect { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckReturnMtrl")]
        public bool CheckReturnMtrl { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "CheckSubRtnMtrl")]
        public bool CheckSubRtnMtrl { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity6_FIncSampSchemeId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity6_FIncQcSchemeId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity6_FInspectGroupId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity6_FInspectorId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity7
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public int FEntryId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FSubconUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity7_FSubconUnitId FSubconUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity7_FSubconUnitId();//": {

        [JsonProperty(PropertyName = "FSubconPriceUnitId")]
        public K3Cloud_Item_Model_SubHeadEntity7_FSubconPriceUnitId FSubconPriceUnitId { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity7_FSubconPriceUnitId();//": {

        [JsonProperty(PropertyName = "FSubBillType")]
        public K3Cloud_Item_Model_SubHeadEntity7_FSubBillType FSubBillType { get; set; }  = new K3Cloud_Item_Model_SubHeadEntity7_FSubBillType();//": {

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity7_FSubconUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity7_FSubconPriceUnitId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_SubHeadEntity7_FSubBillType
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FBarCodeEntity_CMK
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FCodeType_CMK")]
        public string FCodeType_CMK{ get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FUnitId_CMK")]
        public K3Cloud_Item_Model_FBarCodeEntity_CMK_FUnitId_CMK FUnitId_CMK { get; set; }  = new K3Cloud_Item_Model_FBarCodeEntity_CMK_FUnitId_CMK();//": {
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FBarCodeEntity_CMK_FUnitId_CMK
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FSpecialAttributeEntity
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FEntityAuxPty
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FAuxPropertyId")]
        public K3Cloud_Item_Model_FEntityAuxPty_FAuxPropertyId FAuxPropertyId { get; set; }  = new K3Cloud_Item_Model_FEntityAuxPty_FAuxPropertyId();    //": {

        [JsonProperty(PropertyName = "FIsEnable1")]
        public bool FIsEnable1 { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsComControl")]
        public bool FIsComControl { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectPrice1")]
        public bool FIsAffectPrice1 { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectPlan1")]
        public bool FIsAffectPlan1 { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectCost1")]
        public bool FIsAffectCost1 { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsMustInput")]
        public bool FIsMustInput { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FValueType")]
        public string FValueType { get; set; } = "";//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FEntityAuxPty_FAuxPropertyId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FEntityInvPty
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public int FEntryID { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "FInvPtyId")]
        public K3Cloud_Item_Model_FEntityInvPty_FInvPtyId FInvPtyId { get; set; }  = new K3Cloud_Item_Model_FEntityInvPty_FInvPtyId();//": {

        [JsonProperty(PropertyName = "FIsEnable")]
        public bool FIsEnable { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectPrice")]
        public bool FIsAffectPrice { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectPlan")]
        public bool FIsAffectPlan { get; set; } = false;//": "false",

        [JsonProperty(PropertyName = "FIsAffectCost")]
        public bool FIsAffectCost { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Model_FEntityInvPty_FInvPtyId
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Success
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_Item_Success_Result Result { get;set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Success_Result
    {
        [JsonProperty(PropertyName = "ResponseStatus")]
        public K3Cloud_Item_Success_Result_ResponseStatus ResponseStatus { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "NeedReturnData")]
        public List<object> NeedReturnData { get; set; } = new List<object>() { };
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Success_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess{ get; set; }//": true,

        [JsonProperty(PropertyName = "Errors")]
        public JArray Errors{ get; set; }//": [],

        [JsonProperty(PropertyName = "SuccessEntitys")]
        public List<K3Cloud_Item_Success_Result_ResponseStatus_SuccessEntitys> SuccessEntitys{ get; set; }//":

        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages{ get; set; }//": [],

        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode{ get; set; }//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Success_Result_ResponseStatus_SuccessEntitys
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }//": 100096,

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; }//": "测试物料编码",

        [JsonProperty(PropertyName = "DIndex")]
        public int DIndex { get; set; }//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Success_Result_NeedReturnData
    {
        
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Commit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers { get; set; } = new JArray();//": [],

        [JsonProperty(PropertyName = "Ids")]
        public string Ids { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "SelectedPostId")]
        public int SelectedPostId { get; set; } = 0;//": 0,

        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": ""
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Audit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers;//": [],

        [JsonProperty(PropertyName = "Ids")]
        public string Ids;//": "",

        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags;//": "",

        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl;//": "",

        [JsonProperty(PropertyName = "IsVerifyProcInst")]
        public string IsVerifyProcInst;//": "",

        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag;//": ""
    }
}
