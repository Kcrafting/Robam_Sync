using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer
    {

        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],
        public JArray NeedReturnFields = new JArray();//": [],
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
        public K3Cloud_Customer_Model Model { get; set; } = new K3Cloud_Customer_Model();
}
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model
    {
        public int FCUSTID { get; set; } = 0;//": 0,
        public string FNumber { get; set; } = "";//": "",
        public string FName { get; set; } = "";//": "",
        public string FShortName { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FCOUNTRY { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FPROVINCIAL { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FADDRESS { get; set; } = "";//": "",
        public string FZIP { get; set; } = "";//": "",
        public string FWEBSITE { get; set; } = "";//": "",
        public string FTEL { get; set; } = "";//": "",
        public string FFAX { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FCompanyClassify { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FCompanyNature { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FCompanyScale { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FINVOICETITLE { get; set; } = "";//": "",
        public string FTAXREGISTERCODE { get; set; } = "";//": "",
        public string FINVOICEBANKNAME { get; set; } = "";//": "",
        public string FINVOICETEL { get; set; } = "";//": "",
        public string FINVOICEBANKACCOUNT { get; set; } = "";//": "",
        public string FINVOICEADDRESS { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FSUPPLIERID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public bool FIsGroup { set; get; } = false;//": "false",
        public bool FIsDefPayer { set; get; } = false;//": "false",
        public K3Cloud_Common.K3Cloud_FNumber FCustTypeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FGROUPCUSTID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FGroup { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FTRADINGCURRID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public string FDescription { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FSALDEPTID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FSELLER { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FSETTLETYPEID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FRECCONDITIONID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FDISCOUNTLISTID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FPRICELISTID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public int FTRANSLEADTIME { get; set; } = 0;//": 0,
        public string FInvoiceType { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FTaxType { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FRECEIVECURRID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public int FPriority { get; set; } = 1;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FTaxRate { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public bool FISCREDITCHECK { set; get; } = false;//": "false",
        public bool FIsTrade { set; get; } = false;//": "false",
        public bool FUncheckExpectQty { set; get; } = false;//": "false",
        public string FLegalPerson { get; set; } = "";//": "",
        public string FRegisterFund { get; set; } = "";//": "",
        public string FFoundDate { get; set; } = "";//": "",
        public string FDomains { get; set; } = "";//": "",
        public string FSOCIALCRECODE { get; set; } = "";//": "",
        public string FRegisterAddress { get; set; } = "";//": "",
        public K3Cloud_Customer_Model_FT_BD_CUSTOMEREXT FT_BD_CUSTOMEREXT { get; set; } = new K3Cloud_Customer_Model_FT_BD_CUSTOMEREXT();
        public List<K3Cloud_Customer_Model_FT_BD_CUSTLOCATION> FT_BD_CUSTLOCATION { get; set; } = new List<K3Cloud_Customer_Model_FT_BD_CUSTLOCATION>();
        public List<K3Cloud_Customer_Model_FT_BD_CUSTBANK> FT_BD_CUSTBANK { get; set; } = new List<K3Cloud_Customer_Model_FT_BD_CUSTBANK>();
        public List<K3Cloud_Customer_Model_FT_BD_CUSTCONTACT> FT_BD_CUSTCONTACT { get; set; } = new List<K3Cloud_Customer_Model_FT_BD_CUSTCONTACT>();
        public List<K3Cloud_Customer_Model_FT_BD_CUSTORDERORG> FT_BD_CUSTORDERORG { get; set; } = new List<K3Cloud_Customer_Model_FT_BD_CUSTORDERORG>();
        public List<K3Cloud_Customer_Model_FT_BD_CUSTSUBACCOUNT> FT_BD_CUSTSUBACCOUNT { get; set; } = new List<K3Cloud_Customer_Model_FT_BD_CUSTSUBACCOUNT>();
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTOMEREXT
    {
    public int FEntryId { get; set; } = 0;//": 0,
    public bool FEnableSL { get; set; } = false;//": "false",
    public string FFreezeLimit { get; set; } = "";//": "",
    public K3Cloud_Common.K3Cloud_FUserID FFreezeOperator { get; set; } = new K3Cloud_Common.K3Cloud_FUserID();//": {
    public string FFreezeDate { get; set; } = "";//": "1900-01-01",
    public K3Cloud_Common.K3Cloud_FNumber FPROVINCE { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    public K3Cloud_Common.K3Cloud_FNumber FCITY { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    public K3Cloud_Common.K3Cloud_FNumber FDefaultConsiLoc { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    public K3Cloud_Common.K3Cloud_FNumber FDefaultSettleLoc { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    public K3Cloud_Common.K3Cloud_FNumber FDefaultPayerLoc { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    public K3Cloud_Common.K3Cloud_FNumber FDefaultContact { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
    public int FMarginLevel { get; set; } = 0;//": 0,
    public string FDebitCard { get; set; } = "";//": "",
    public K3Cloud_Common.K3Cloud_FNumber FSettleId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
    public K3Cloud_Common.K3Cloud_FNumber FChargeId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
    public bool FALLOWJOINZHJ { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTLOCATION
    {
        public K3Cloud_Common.K3Cloud_FNumber FContactId { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public bool FIsDefaultConsigneeCT { get; set; } = false;
        public bool FIsCopy { get; set; } = false;
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTBANK
    {
        public int FENTRYID { get; set; } = 0;//": 0,
        public K3Cloud_Common.K3Cloud_FNumber FCOUNTRY1 { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FBANKCODE { get; set; } = "";//": "",
        public string FACCOUNTNAME { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBankTypeRec { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FTextBankDetail { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FBankDetail { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public string FOpenAddressRec { get; set; } = "";//": "",
        public string FOPENBANKNAME { get; set; } = "";//": "",
        public string FCNAPS { get; set; } = "";//": "",
        public K3Cloud_Common.K3Cloud_FNumber FCURRENCYID { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

        public bool FISDEFAULT1 { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTCONTACT
    {
        public int FENTRYID { get; set; } = 0;//": 0,
        public string FNUMBER1 { get; set; } = "";//": "",
        public string FNAME1 { get; set; } = "";//": "",
        public string FADDRESS1 { get; set; } = "";//": "",
        public int FTRANSLEADTIME1 { get; set; } = 0;//": 0,
        public string FMOBILE { get; set; } = "";//": "",
        public bool FIsDefaultConsignee { get; set; } = false;//": "false",
        public bool FIsDefaultSettle { get; set; } = false;//": "false",
        public bool FIsDefaultPayer { get; set; } = false;//": "false",
        public bool FIsUsed { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTORDERORG
    {
        public int FEntryID { get; set; } = 0;//": 0,
        public bool FIsDefaultOrderOrg { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Customer_Model_FT_BD_CUSTSUBACCOUNT
    {
        public int FEntryID { get; set; } = 0;//": 0,
        public string FSUBACCOUNTTYPE { get; set; } = "";//": "",
        public string FSUBACCOUNT { get; set; } = "";//": ""
    }

}
