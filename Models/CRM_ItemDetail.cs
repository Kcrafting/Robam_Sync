using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_ItemDetail
    {
        [JsonProperty(PropertyName = "materials")]
        public List<CRM_ItemDetail_materials> materials { get; set; } = new List<CRM_ItemDetail_materials>();
        [JsonProperty(PropertyName = "page")]
        public CRM_ItemDetail_page page { get; set; } = new CRM_ItemDetail_page();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_ItemDetail_materials
    {
        [JsonProperty(PropertyName = "materialId")]
        public int? materialId{ get; set; }//": 120388293,

        [JsonProperty(PropertyName = "materialCode")]
        public string? materialCode{ get; set; }//": "800-JV391.W",

        [JsonProperty(PropertyName = "materialDesc")]
        public string? materialDesc { get; set; }//": "双出水净水机",

        [JsonProperty(PropertyName = "materialType")]
        public string? materialType { get; set; }//": "1",

        [JsonProperty(PropertyName = "unitCode")]
        public string? unitCode { get; set; }//": "PCS",

        [JsonProperty(PropertyName = "beginDate")]
        public string? beginDate { get; set; }//": "2022-01-18 09:14:25",

        [JsonProperty(PropertyName = "lengthUnitCode")]
        public string? lengthUnitCode { get; set; }//": "M",

        [JsonProperty(PropertyName = "length")]
        public int? length { get; set; }//": 0,

        [JsonProperty(PropertyName = "width")]
        public int? width { get; set; }//": 0,

        [JsonProperty(PropertyName = "height")]
        public int? height { get; set; }//": 0,

        [JsonProperty(PropertyName = "volumeUnit")]
        public string? volumeUnit { get; set; }//": "M3",

        [JsonProperty(PropertyName = "volume")]
        public int? volume { get; set; }//": 0,

        [JsonProperty(PropertyName = "qualityUnit")]
        public string? qualityUnit { get; set; }//": "KG",

        [JsonProperty(PropertyName = "quality")]
        public int? quality { get; set; }//": 13,

        [JsonProperty(PropertyName = "lastUpdateDate")]
        public string? lastUpdateDate { get; set; }//": "2022-08-04 00:00:00",

        [JsonProperty(PropertyName = "materialName")]
        public string? materialName { get; set; }//": "双出水净水机",

        [JsonProperty(PropertyName = "specification")]
        public string? specification { get; set; }//": "800-JV391",

        [JsonProperty(PropertyName = "productModelCode")]
        public string? productModelCode { get; set; }//": "800-JV391",

        [JsonProperty(PropertyName = "productTypeCode")]
        public string? productTypeCode { get; set; }//": "P1",

        [JsonProperty(PropertyName = "isSpecialGasSource")]
        public string? isSpecialGasSource { get; set; }//": "N",

        [JsonProperty(PropertyName = "isEbsImport")]
        public string? isEbsImport { get; set; }//": "Y",

        [JsonProperty(PropertyName = "productTypeName")]
        public string? productTypeName { get; set; }//": "净水机",

        [JsonProperty(PropertyName = "updatetor")]
        public string? updatetor { get; set; }//": "超级用户",

        [JsonProperty(PropertyName = "trademark")]
        public string? trademark { get; set; }//": "ROBAM",

        [JsonProperty(PropertyName = "packageOrgName")]
        public string? packageOrgName { get; set; }//": "老板物料",

        [JsonProperty(PropertyName = "materialTypeName")]
        public string? materialTypeName { get; set; }//": "商品",

        [JsonProperty(PropertyName = "settleType")]
        public string? settleType { get; set; }//": "净水器",

        [JsonProperty(PropertyName = "isMarking")]
        public string? isMarking { get; set; }//": "N",

        [JsonProperty(PropertyName = "scanBar")]
        public string? scanBar { get; set; }//": "Y"

        
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_ItemDetail_page
    {
        [JsonProperty(PropertyName = "begin")]
        public int? begin{ get; set; }//": 0,

        [JsonProperty(PropertyName = "length")]
        public int? length{ get; set; }//": 20,

        [JsonProperty(PropertyName = "count")]
        public int? count { get; set; }//": 1,

        [JsonProperty(PropertyName = "totalPage")]
        public int? totalPage { get; set; }//": 1,

        [JsonProperty(PropertyName = "currentPage")]
        public int? currentPage { get; set; }//": 1,

        [JsonProperty(PropertyName = "isCount")]
        public bool isCount{ get; set; }//": true,

        [JsonProperty(PropertyName = "isFirst")]
        public bool isFirst { get; set; }//": true,

        [JsonProperty(PropertyName = "isLast")]
        public bool isLast { get; set; }//": true,

        [JsonProperty(PropertyName = "size")]
        public int? size { get; set; }//": 1
    }
}
