using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Group
    {
        [JsonProperty(PropertyName = "FParentId")]
        public int FParentId { get; set; } = 0;//": [],

        [JsonProperty(PropertyName = "FNumber")]
        public string FNumber { get; set; } = "";//": [],

        [JsonProperty(PropertyName = "FName")]
        public string FName { get; set; } = "";//": "true",

        [JsonProperty(PropertyName = "FDescription")]
        public string FDescription { get; set; } = "";//": "",        
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Group_Result
    {
        [JsonProperty(PropertyName = "Result")]
        public K3Cloud_Item_Group_Result_ResponseStatus Result { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Group_Result_ResponseStatus
    {
        [JsonProperty(PropertyName = "IsSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty(PropertyName = "Errors")]
        public JArray Errors { get; set; }

        [JsonProperty(PropertyName = "SuccessEntitys")]
        public List<K3Cloud_Item_Group_Result_ResponseStatus_SuccessEntitys> SuccessEntitys { get; set; }

        [JsonProperty(PropertyName = "SuccessMessages")]
        public JArray SuccessMessages { get; set; }

        [JsonProperty(PropertyName = "MsgCode")]
        public int MsgCode { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Group_Result_ResponseStatus_SuccessEntitys
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }//"://": 100103,

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; }//"://": "ceshi",

        [JsonProperty(PropertyName = "DIndex")]
        public int DIndex { get; set; }//"://": 0
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Item_Group_Result_ResponseStatus_SuccessMessages
    {

    }

    //[JsonObject(MemberSerialization.OptOut)]
    //public class K3Cloud_Item_Group_Model
    //{
    //    [JsonProperty(PropertyName = "FID")]
    //    public int FID { get; set; } = 0;
    //    [JsonProperty(PropertyName = "FBillNo")]
    //    public string FBillNo { get; set; } = "";
    //    [JsonProperty(PropertyName = "FOrgAffiliation")]
    //    public K3Cloud_Item_Group_Model_FOrgAffiliation FOrgAffiliation { get; set; } = new K3Cloud_Item_Group_Model_FOrgAffiliation();
    //    [JsonProperty(PropertyName = "FIsApplyAllShop")]
    //    public bool FIsApplyAllShop { get; set; } = false;

    //    [JsonProperty(PropertyName = "FCreatorId")]
    //    public K3Cloud_Item_Group_Model_FCreatorId FCreatorId { get; set; } = new K3Cloud_Item_Group_Model_FCreatorId();

    //    [JsonProperty(PropertyName = "FCreateDate")]
    //    public string FCreateDate { get; set; } = "2022-09-01";

    //    [JsonProperty(PropertyName = "FEntity")]
    //    public K3Cloud_Item_Group_Model_FEntity FEntity { get; set; } = new K3Cloud_Item_Group_Model_FEntity();
    //    [JsonProperty(PropertyName = "FBranchSetEntity")]
    //    public K3Cloud_Item_Group_Model_FBranchSetEntity FBranchSetEntity { get; set; } = new K3Cloud_Item_Group_Model_FBranchSetEntity();
    //}
    //[JsonObject(MemberSerialization.OptOut)]
    //public class K3Cloud_Item_Group_Model_FOrgAffiliation
    //{
    //    [JsonProperty(PropertyName = "FNUMBER")]
    //    public string FNUMBER { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FCreatorId
    //{
    //    [JsonProperty(PropertyName = "FUserID")]
    //    public string FUserID { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FEntity
    //{
    //    [JsonProperty(PropertyName = "FEntryID")]
    //    public int FEntryID { get; set; } = 0;

    //    [JsonProperty(PropertyName = "FLevel")]
    //    public int FLevel { get; set; } = 0;

    //    [JsonProperty(PropertyName = "FMATERIALGROUP")]
    //    public K3Cloud_Item_Group_Model_FEntity_FMATERIALGROUP FMATERIALGROUP { get; set; } = new K3Cloud_Item_Group_Model_FEntity_FMATERIALGROUP();

    //    [JsonProperty(PropertyName = "FShowSeq")]
    //    public int FShowSeq { get; set; } = 0;
    //    [JsonProperty(PropertyName = "FGroupSubEntity")]
    //    public K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity FGroupSubEntity { get; set; } = new K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity();
    //}
    //public class K3Cloud_Item_Group_Model_FEntity_FMATERIALGROUP
    //{
    //    [JsonProperty(PropertyName = "FNumber")]
    //    public string FNumber { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity
    //{
    //    [JsonProperty(PropertyName = "FDetailID")]
    //    public int FDetailID { get; set; } = 0;
    //    [JsonProperty(PropertyName = "FMaterialId")]
    //    public K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity_FMaterialId FMaterialId { get; set; } = new K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity_FMaterialId();
    //    [JsonProperty(PropertyName = "FMaterialSeq")]
    //    public int FMaterialSeq { get; set; } = 0;
    //}
    //public class K3Cloud_Item_Group_Model_FEntity_FGroupSubEntity_FMaterialId
    //{
    //    [JsonProperty(PropertyName = "FNUMBER")]
    //    public string FNUMBER { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FBranchSetEntity
    //{
    //    [JsonProperty(PropertyName = "FEntryID")]
    //    public int FEntryID { get; set; } = 0;
    //    [JsonProperty(PropertyName = "FChannelId")]
    //    public K3Cloud_Item_Group_Model_FBranchSetEntity_FChannelId FChannelId { get; set; } = new K3Cloud_Item_Group_Model_FBranchSetEntity_FChannelId();

    //    [JsonProperty(PropertyName = "FBranchID")]
    //    public K3Cloud_Item_Group_Model_FBranchSetEntity_FChannelId FBranchID { get; set; } = new K3Cloud_Item_Group_Model_FBranchSetEntity_FChannelId();

    //    [JsonProperty(PropertyName = "FIsSelected")]
    //    public bool FIsSelected { get; set; } = false;

    //    [JsonProperty(PropertyName = "FParentId")]
    //    public string FParentId { get; set; } = "";
    //    [JsonProperty(PropertyName = "FRowType")]
    //    public int FRowType { get; set; } = 0;
    //    [JsonProperty(PropertyName = "FRowId")]
    //    public string FRowId { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FBranchSetEntity_FChannelId
    //{
    //    [JsonProperty(PropertyName = "FNUMBER")]
    //    public string FNUMBER { get; set; } = "";
    //}
    //public class K3Cloud_Item_Group_Model_FBranchSetEntity_FBranchID
    //{
    //    [JsonProperty(PropertyName = "FNUMBER")]
    //    public string FNUMBER { get; set; } = "";
    //}
}
