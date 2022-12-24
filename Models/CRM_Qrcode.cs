using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_Qrcode
    {
        [JsonProperty(PropertyName = "barcodeList")]
        public List<CRM_Qrcode_barcodeList> barcodeList { get; set; } = new List<CRM_Qrcode_barcodeList>();
        [JsonProperty(PropertyName = "page")]
        public CRM_Qrcode_page page { get; set; } = new CRM_Qrcode_page();
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_Qrcode_barcodeList
    {
        [JsonProperty(PropertyName = "exOrderLinesnId")]
        public int exOrderLinesnId { get; set; }

        [JsonProperty(PropertyName = "exOrderHeadersId")]
        public int exOrderHeadersId { get; set; }

        [JsonProperty(PropertyName = "materialCode")]
        public string materialCode { get; set; }

        [JsonProperty(PropertyName = "goodsStatus")]
        public string goodsStatus { get; set; }

        [JsonProperty(PropertyName = "barcode")]
        public string barcode { get; set; }

        [JsonProperty(PropertyName = "fxOrderNo")]
        public string fxOrderNo { get; set; }

        [JsonProperty(PropertyName = "invType")]
        public string invType { get; set; }

        [JsonProperty(PropertyName = "materialName")]
        public string materialName { get; set; }

        [JsonProperty(PropertyName = "orderNumber")]
        public string orderNumber { get; set; }

        [JsonProperty(PropertyName = "queryEntityId")]
        public string queryEntityId { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_Qrcode_page
    {
        [JsonProperty(PropertyName = "begin")]
        public int begin { get; set; }//": 0,
        [JsonProperty(PropertyName = "length")]
        public int length { get; set; }//": 20,
        [JsonProperty(PropertyName = "count")]
        public int count { get; set; }//": 0,
        [JsonProperty(PropertyName = "totalPage")]
        public int totalPage { get; set; }//": 0,
        [JsonProperty(PropertyName = "currentPage")]
        public int currentPage { get; set; }//": 0,
        [JsonProperty(PropertyName = "isCount")]
        public bool isCount { get; set; }//": true,
        [JsonProperty(PropertyName = "isFirst")]
        public bool isFirst { get; set; }//": true,
        [JsonProperty(PropertyName = "isLast")]
        public bool isLast { get; set; }//": true,
        [JsonProperty(PropertyName = "size")]
        public int size { get; set; }//": 0
    }
}