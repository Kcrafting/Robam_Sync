using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    //创建参数
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Create
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
        public K3Cloud_BillType_Model Model { get; set; } = new K3Cloud_BillType_Model();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model
    {
        public string FBILLTYPEID{ get; set; } = "";//": "",
        public K3Cloud_BillType_Model_FBillFormID FBillFormID { get; set; } = new K3Cloud_BillType_Model_FBillFormID();
        public string FNumber { get; set; } = "";//": "",
        public string FName { get; set; } = "";//": "",
        public K3Cloud_BillType_Model_FBillCodeRuleID FBillCodeRuleID { get; set; } = new K3Cloud_BillType_Model_FBillCodeRuleID();
        public string FDescription { get; set; } = "";//": "",
        public bool FIsDefault { get; set; } = false;
        public K3Cloud_BillType_Model_FLayoutSolution FLayoutSolution { get; set; } = new K3Cloud_BillType_Model_FLayoutSolution();
        public bool FControlPrintCount { get; set; } = false;//": "false",
        public int FMaxPrintCount { get; set; } = 0;//": 0,
        public string FDefPrintTemplate { get; set; } = "";//": "",
        public string FDefWnReportTemplate { get; set; } = "";//": "",
        public List<K3Cloud_BillType_Model_FFieldControl> FFieldControl { get; set; } = null;
        public List<K3Cloud_BillType_Model_FWFSetting> FWFSetting { get; set; } = null;
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FBillFormID
    {
        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FBillCodeRuleID
    {
        [JsonProperty(PropertyName = "FNAME")]
        public string FNAME { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FLayoutSolution
    {
        [JsonProperty(PropertyName = "FID")]
        public string FID { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FFieldControl
    {
        [JsonProperty(PropertyName = "FEntryID")]
        public string FEntryID{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FSEQ")]
        public int FSEQ{ get; set; } = 0;// 0,
        [JsonProperty(PropertyName = "FMustInput")]
        public bool FMustInput{ get; set; } = false;// public string False",
        [JsonProperty(PropertyName = "FFieldKey")]
        public string FFieldKey{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FEnabled")]
        public bool FEnabled{ get; set; } = false;// public string False",
        [JsonProperty(PropertyName = "FEDITENABLED")]
        public bool FEDITENABLED{ get; set; } = false;// public string False",
        [JsonProperty(PropertyName = "FDefaultValue")]
        public string FDefaultValue{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FFormId")]
        public string FFormId{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FDefaultValueId")]
        public string FDefaultValueId{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FDefaultFuncName")]
        public string FDefaultFuncName{ get; set; } = "";// "",
        [JsonProperty(PropertyName = "FFieldElementType")]
        public int FFieldElementType{ get; set; } = 0;// 0,
        [JsonProperty(PropertyName = "FSysMustInput")]
        public bool FSysMustInput{ get; set; } = false;// public string False"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FWFSetting
    {
        [JsonProperty(PropertyName = "FEntryId")]
        public string FEntryId { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "FBFVersionId")]
        public K3Cloud_BillType_Model_FWFSetting_FBFVersionId FBFVersionId { get; set; } = new K3Cloud_BillType_Model_FWFSetting_FBFVersionId();//": 

        [JsonProperty(PropertyName = "FBeginDate")]
        public string FBeginDate { get; set; } = "2020-01-01";//": "1900-01-01",

        [JsonProperty(PropertyName = "FEndDate")]
        public string FEndDate { get; set; } = "2099-01-01";//": "1900-01-01",

        [JsonProperty(PropertyName = "FIsDefaultBF")]
        public bool FIsDefaultBF { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Model_FWFSetting_FBFVersionId
    {
        [JsonProperty(PropertyName = "FNAME")]
        public string FNAME { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_View
    {
        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = "";//": "",

        [JsonProperty(PropertyName = "IsSortBySeq")]
        public bool IsSortBySeq { get; set; } = false;//": "false"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Result
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_BillType_Result_Result Result { get; set; } = new K3Cloud_BillType_Result_Result();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Result_Result
    {
        [JsonProperty(PropertyName = "ResponseStatus")]
        public K3Cloud_BillType_Result_Result_ResponseStatus ResponseStatus { get; set; } = new K3Cloud_BillType_Result_Result_ResponseStatus();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Result_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "ErrorCode")]
        public int ErrorCode { get; set; } = 0;//": 500,
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess { get; set; } = false;//": false,
        [JsonProperty(PropertyName = "Errors")]
        public List<K3Cloud_BillType_Result_Result_ResponseStatus_Errors> Errors { get; set; } = new List<K3Cloud_BillType_Result_Result_ResponseStatus_Errors>();
        [JsonProperty(PropertyName = "SuccessEntitys")]
        public JArray SuccessEntitys { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode { get; set; } //": 8
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Result_Result_ResponseStatus_Errors
    {
        [JsonProperty(PropertyName = "FieldName")]
        public string? FieldName { get; set; } = null;//": null,
        [JsonProperty(PropertyName = "Message")]
        public string? Message { get; set; }//": "传递的编码值不存在",
        [JsonProperty(PropertyName = "DIndex")]
        public int? DIndex { get; set; }//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Commit
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
    public class K3Cloud_BillType_Audit
    {
        [JsonProperty(PropertyName = "Numbers")]
        public JArray Numbers { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "Ids")]
        public string Ids { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "InterationFlags")]
        public string InterationFlags { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "NetworkCtrl")]
        public string NetworkCtrl { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IsVerifyProcInst")]
        public string IsVerifyProcInst { get; set; } = "";//": "",
        [JsonProperty(PropertyName = "IgnoreInterationFlag")]
        public string IgnoreInterationFlag { get; set; } = "";//": ""
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Save
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_BillType_Save_Result Result { get; set; } = new K3Cloud_BillType_Save_Result();
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Save_Result
    {
        [JsonProperty(PropertyName = "ResponseStatus")]
        public K3Cloud_BillType_Save_Result_ResponseStatus ResponseStatus { get; set; } = new K3Cloud_BillType_Save_Result_ResponseStatus();
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = "";
        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; } = "";
        [JsonProperty(PropertyName = "NeedReturnData")]
        public JArray NeedReturnData { get; set; } = new JArray();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Save_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess { get; set; } = true;//": true,
        [JsonProperty(PropertyName = "Errors")]
        public JArray Errors { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "SuccessEntitys")]
        public List<K3Cloud_BillType_Save_Result_ResponseStatus_SuccessEntitys> SuccessEntitys { get; set; } = new List<K3Cloud_BillType_Save_Result_ResponseStatus_SuccessEntitys>();//":
        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages { get; set; } = new JArray();//": [],
        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode { get; set; } = 0;//": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Save_Result_ResponseStatus_SuccessEntitys
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; } = "";
        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; } = "";
        [JsonProperty(PropertyName = "DIndex")]
        public int DIndex { get; set; } = 0;
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_BillType_Save_Result_NeedReturnData
    {

    }

    //查询参数
    public class ResponseStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "Errors")]
        public List<K3Cloud_BillType_Result_Result_ResponseStatus_Errors> Errors { get; set; } = new List<K3Cloud_BillType_Result_Result_ResponseStatus_Errors>();
    }

    public class MultiLanguageTextItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string PkId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LocaleId { get; set; }
        /// <summary>
        /// 客户零售出库单
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }

    public class NameItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 客户零售出库单
        /// </summary>
        public string Value { get; set; }
    }

    public class DescriptionItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }

    public class K3Cloud_BillType_QueryList_MultiLanguageTextItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string PkId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LocaleId { get; set; }
        /// <summary>
        /// 采购入库单
        /// </summary>
        public string Name { get; set; }
    }

    public class K3Cloud_BillType_QueryList_NameItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 采购入库单
        /// </summary>
        public string Value { get; set; }
    }

    public class BillFormID
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MultiLanguageTextItem> MultiLanguageText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<K3Cloud_BillType_QueryList_NameItem> Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Number { get; set; }
    }

    public class ModifierId
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserAccount { get; set; }
    }

    public class CreatorId
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserAccount { get; set; }
    }

    public class AuditorID
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserAccount { get; set; }
    }


    public class FieldNameItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 明细信息.行钩稽状态
        /// </summary>
        public string Value { get; set; }
    }

    public class BillTypeFieldControlItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MustInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FieldKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MultiLanguageTextItem> MultiLanguageText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<FieldNameItem> FieldName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DefaultValueType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultFuncParam { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DefaultFuncID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FieldElementType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SysMustInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultFuncName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Enabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EditEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultValueId { get; set; }
    }

    public class K3Cloud_BillType_QueryList_Result_Result
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DocumentStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ForbidStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MultiLanguageTextItem> MultiLanguageText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NameItem> Name { get; set; }
        /// <summary>
        /// 客户零售出库单
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DescriptionItem> Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BillCodeRuleID_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BillCodeRuleID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MaxPrintCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrintAfterAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ControlPrintCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BillFormID_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BillFormID BillFormID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ModifierId_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ModifierId ModifierId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CreatorId_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CreatorId CreatorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuditDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuditorID_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AuditorID AuditorID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ForbidderID_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ForbidderID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ForbidDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsSysPreSet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefPrintTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutSolution_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayoutSolution { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefWnReportTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FRobam_ForbidImport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BillTypeFieldControlItem> BillTypeFieldControl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> WFSetting { get; set; }
    }

    public class K3Cloud_BillType_QueryList_Result
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public K3Cloud_BillType_QueryList_Result_Result Result { get; set; }
    }

    public class K3Cloud_BillType_QueryList
    {
        /// <summary>
        /// 
        /// </summary>
        public K3Cloud_BillType_QueryList_Result Result { get; set; }
    }

}
