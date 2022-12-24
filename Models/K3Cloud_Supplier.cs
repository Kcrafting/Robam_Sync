using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier
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
        public K3Cloud_Supplier_Model Model { get; set; } = new K3Cloud_Supplier_Model();//"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model
    {
         public int FSupplierId{ get; set; } = 0;//": 0,
        public string FNumber { get; set; } = "";//": "",
        public string FName { get; set; } = "";//": "",
        public string FShortName { get; set; } = "";//": "",
        public K3Cloud_Supplier_Model_FGroup FGroup { get; set; } = new K3Cloud_Supplier_Model_FGroup();
        public string FDescription { get; set; } = "";
        public K3Cloud_Supplier_Model_FForbiderId FForbiderId { get; set; } = new K3Cloud_Supplier_Model_FForbiderId();
        public string FForbidDate { get; set; } = "";
        public K3Cloud_Supplier_Model_FGROUPSUPPLYID FGROUPSUPPLYID { get; set; } = new K3Cloud_Supplier_Model_FGROUPSUPPLYID();
        public bool FISGROUP { get; set; } = false;
        public K3Cloud_Supplier_Model_FBaseInfo FBaseInfo { get; set; } = new K3Cloud_Supplier_Model_FBaseInfo();
        public K3Cloud_Supplier_Model_FBusinessInfo FBusinessInfo { get; set; } = new K3Cloud_Supplier_Model_FBusinessInfo();
        public K3Cloud_Supplier_Model_FFinanceInfo FFinanceInfo { get; set; } = new K3Cloud_Supplier_Model_FFinanceInfo();
        public List<K3Cloud_Supplier_Model_FBankInfo> FBankInfo { get; set; } = new List<K3Cloud_Supplier_Model_FBankInfo>();
        public List<K3Cloud_Supplier_Model_FLocationInfo> FLocationInfo { get; set; } = new List<K3Cloud_Supplier_Model_FLocationInfo>();
        public List<K3Cloud_Supplier_Model_FSupplierContact> FSupplierContact { get; set; } = new List<K3Cloud_Supplier_Model_FSupplierContact>();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FGroup
    {
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FForbiderId
    {
        public string FUserID { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FGROUPSUPPLYID
    {
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FBaseInfo
    {
        public int FEntryId { get; set; } = 0;
        public K3Cloud_Common.K3Cloud_FNumber FCountry { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FProvincial { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FAddress { get; set; } = "";
        public string FZip { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FLanguage { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FWebSite { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FTrade { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FFoundDate { get; set; } = "";
        public string FLegalPerson { get; set; } = "";
        public int FRegisterFund { get; set; } = 0;
        public string FRegisterCode { get; set; } = "";
        public string FSOCIALCRECODE { get; set; } = "";
        public string FTendPermit { get; set; } = "";
        public string FRegisterAddress { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FDeptId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FStaffId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FSupplierClassify { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public string FSupplyClassify { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FNumber FSupplierGrade { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FCompanyClassify { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FCompanyNature { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FCompanyScale { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FBusinessInfo
    {
        public int FEntryId { get; set; } = 0;//": 0,
        public K3Cloud_Supplier_Model_FBusinessInfo_FFreezeOperator FFreezeOperator { get; set; } = new K3Cloud_Supplier_Model_FBusinessInfo_FFreezeOperator();//": {
        public string FFreezeDate { get; set; } = "2022-1-1";//": "1900-01-01",
        public K3Cloud_Common.K3Cloud_FNumber FPurchaserGroupId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FParentSupplierId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FSettleTypeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FPRICELISTID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FDiscountListId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FProviderId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FWipStockId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public bool FVmiBusiness { get; set; } = false;//": "false",
        public object FWipStockPlaceId { get; set; } = new object();//": {},
        public K3Cloud_Common.K3Cloud_FNumber FVmiStockId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public bool FEnableSL { get; set; } = false;//": "false",
        public int FDepositRatio { get; set; } = 0;//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FBusinessInfo_FFreezeOperator
    {
        public string FUserID { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FFinanceInfo
    {
        public int FEntryId { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FCustomerId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FPayCurrencyId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FPayCondition { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public K3Cloud_Common.K3Cloud_FNumber FSettleId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public int FPayAdvanceAmount { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FTaxType { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FTaxRegisterCode { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FChargeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FInvoiceType { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FTaxRateId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FFinanceDesc { get; set; } = "";//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FBankInfo
    {
        public int FBankId { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FBankCountry { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FBankCode { get; set; } = "";//": "",
        public string FBankHolder { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBankTypeRec { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FTextBankDetail { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBankDetail { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FOpenAddressRec { get; set; } = "";//": "",
        public string FOpenBankName { get; set; } = "";//": "",
        public string FCNAPS { get; set; } = "";//": "",
        public string FSwiftCode { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBankCurrencyId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public bool FBankIsDefault { get; set; } = false;//": "false",
        public string FBankDesc { get; set; } = "";//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FLocationInfo
    {
        public int FLocationId { get; set; } = 0;//": 0,
        public string FLocName { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FLocNewContact { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FLocAddress { get; set; } = "";//": "",
        public string FLocMobile { get; set; } = "";//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Supplier_Model_FSupplierContact
    {
        public int FContactId { get; set; } = 0;
    }
}
