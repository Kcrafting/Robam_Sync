using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_INBillType
    {
        [JsonProperty(PropertyName = "dataobjs")]
        public List<CRM_INBillType_dataobjs> dataobjs { get; set; }
        [JsonProperty(PropertyName = "page")]
        public CRM_INBillType_page page { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_INBillType_dataobjs
    {
        [JsonProperty(PropertyName = "orderTypeId")]
        public int? orderTypeId { get; set; }//": 1000403,
        [JsonProperty(PropertyName = "orderTypeName")]
        public string orderTypeName { get; set; }//": "库位调拨单",
        [JsonProperty(PropertyName = "orderSourceType")]
        public string orderSourceType { get; set; }//": "调拨单据",
        [JsonProperty(PropertyName = "stockOrderTypeId")]
        public int? stockOrderTypeId { get; set; }//": 13,
        [JsonProperty(PropertyName = "blueOrderTypeId")]
        public string blueOrderTypeId { get; set; }//": null,
        [JsonProperty(PropertyName = "orderNoIdentify")]
        public int? orderNoIdentify { get; set; }//": 83,
        [JsonProperty(PropertyName = "negativeStockFlag")]
        public string negativeStockFlag { get; set; }//": "N",
        [JsonProperty(PropertyName = "fullCounteractFlag")]
        public string fullCounteractFlag { get; set; }//": "Y",
        [JsonProperty(PropertyName = "decimalNumberFlag")]
        public string decimalNumberFlag { get; set; }//": "N",
        [JsonProperty(PropertyName = "negativeNumberFlag")]
        public string negativeNumberFlag { get; set; }//": "N",
        [JsonProperty(PropertyName = "createdBy")]
        public int? createdBy { get; set; }//": 1,
        [JsonProperty(PropertyName = "creationDate")]
        public string creationDate { get; set; }//": "2018-01-29",
        [JsonProperty(PropertyName = "remark")]
        public string remark { get; set; }//": null,
        [JsonProperty(PropertyName = "lastUpdatedBy")]
        public int? lastUpdatedBy { get; set; }//": 1,
        [JsonProperty(PropertyName = "lastUpdateDate")]
        public string lastUpdateDate { get; set; }//": "2018-01-29",
        [JsonProperty(PropertyName = "endDate")]
        public string endDate { get; set; }//": null,
        [JsonProperty(PropertyName = "beginDate")]
        public string beginDate { get; set; }//": "2018-01-29",
        [JsonProperty(PropertyName = "orgId")]
        public int? orgId { get; set; }//": 20,
        [JsonProperty(PropertyName = "postpositionOrderTypeId")]
        public string postpositionOrderTypeId { get; set; }//": null,
        [JsonProperty(PropertyName = "isAuto")]
        public string isAuto { get; set; }//": "Y",
        [JsonProperty(PropertyName = "isOptional")]
        public string isOptional { get; set; }//": "Y",
        [JsonProperty(PropertyName = "reversePOrderTypeId")]
        public string reversePOrderTypeId { get; set; }//": null,
        [JsonProperty(PropertyName = "isUpdateStockNum")]
        public string isUpdateStockNum { get; set; }//": "N",
        [JsonProperty(PropertyName = "isInterface")]
        public string isInterface { get; set; }//": "N",
        [JsonProperty(PropertyName = "actualQtyDefault")]
        public string actualQtyDefault { get; set; }//": "N",
        [JsonProperty(PropertyName = "isOnlyIn")]
        public string isOnlyIn { get; set; }//": null,
        [JsonProperty(PropertyName = "isOnlyOut")]
        public string isOnlyOut { get; set; }//": null,
        [JsonProperty(PropertyName = "orderTypeCode")]
        public string orderTypeCode { get; set; }//": null,
        [JsonProperty(PropertyName = "isReceiveInv")]
        public string isReceiveInv { get; set; }//": null,
        [JsonProperty(PropertyName = "partorgood")]
        public string partorgood { get; set; }//": "GOOD",
        [JsonProperty(PropertyName = "invFlag")]
        public string invFlag { get; set; }//": "3"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_INBillType_page
    {
        [JsonProperty(PropertyName = "begin")]
        public int begin{ get; set; }//": 0,
        [JsonProperty(PropertyName = "length")]
        public int length { get; set; }//": 100,
        [JsonProperty(PropertyName = "count")]
        public int count { get; set; }//": 20,
        [JsonProperty(PropertyName = "totalPage")]
        public int totalPage { get; set; }//": 1,
        [JsonProperty(PropertyName = "currentPage")]
        public int currentPage { get; set; }//": 1,
        [JsonProperty(PropertyName = "isCount")]
        public bool isCount{ get; set; }//": true,
        [JsonProperty(PropertyName = "isFirst")]
        public bool isFirst { get; set; }//": true,
        [JsonProperty(PropertyName = "isLast")]
        public bool isLast { get; set; }//": true,
        [JsonProperty(PropertyName = "size")]
        public int size { get; set; }//": 20
    }
}
