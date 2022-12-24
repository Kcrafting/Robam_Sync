using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Saler
    {
        public JArray NeedUpDateFields { get; set; } = new JArray();//": [],
        public JArray NeedReturnFields { get; set; } = new JArray();///": [],
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
        public K3Cloud_Saler_Model Model { get; set; } = new K3Cloud_Saler_Model();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_Saler_Model
    {
        public int FID { get; set; }//": 0,
        public string FNumber { get; set; }//": "",
        public string FName { get; set; }//": "",
        public K3Cloud_Common.K3Cloud_FUserID FCreatorId { get; set; } = new K3Cloud_Common.K3Cloud_FUserID();//": {
        public string FCreateDate { get; set; } = "";
        public K3Cloud_Common.K3Cloud_FUserID FModifierId { get; set; } = new K3Cloud_Common.K3Cloud_FUserID();//": {
        public string FModifyDate { get; set; } = "";//": "1900-01-01",
        public K3Cloud_Common.K3Cloud_FNumber FGroup { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {
        public K3Cloud_Common.K3Cloud_FNumber FShop { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();//": {

    }
}
