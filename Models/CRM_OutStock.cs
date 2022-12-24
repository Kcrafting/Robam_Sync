using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ProductQrcodeMap
    {
        [JsonProperty(PropertyName = "FItemNumber")]
        public string FItemNumber { get; set; }
        [JsonProperty(PropertyName = "FQrcode")]
        public string FQrcode { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class crminvexportheaderss
    {
        [JsonProperty(PropertyName = "exOrderHeadersId")]
        public int exOrderHeadersId{ get; set; }

        [JsonProperty(PropertyName = "orderNo")]
        public string orderNo{ get; set; }

        [JsonProperty(PropertyName = "orderTypeId")]
        public int orderTypeId{ get; set; }

        [JsonProperty(PropertyName = "orderTypeCode")]
        public string orderTypeCode{ get; set; }

        [JsonProperty(PropertyName = "orderTypeName")]
        public string orderTypeName{ get; set; }

        [JsonProperty(PropertyName = "orderDate")]
        public string orderDate{ get; set; }

        [JsonProperty(PropertyName = "status")]
        public string status{ get; set; }

        [JsonProperty(PropertyName = "statusName")]
        public string statusName{ get; set; }

        [JsonProperty(PropertyName = "exportFlag")]
        public string exportFlag{ get; set; }

        [JsonProperty(PropertyName = "exportConfirmDate")]
        public string exportConfirmDate{ get; set; }

        [JsonProperty(PropertyName = "exportConfirmByName")]
        public string exportConfirmByName{ get; set; }

        [JsonProperty(PropertyName = "receiveFlag")]
        public string receiveFlag{ get; set; }

        [JsonProperty(PropertyName = "receiveByName")]
        public string receiveByName{ get; set; }

        [JsonProperty(PropertyName = "receiveDate")]
        public string receiveDate{ get; set; }

        [JsonProperty(PropertyName = "printDate")]
        public string printDate{ get; set; }

        [JsonProperty(PropertyName = "printCount")]
        public int printCount{ get; set; }

        [JsonProperty(PropertyName = "printLock")]
        public string printLock{ get; set; }

        [JsonProperty(PropertyName = "orgName")]
        public string orgName{ get; set; }

        [JsonProperty(PropertyName = "customerName")]
        public string customerName{ get; set; }

        [JsonProperty(PropertyName = "createdByName")]
        public string createdByName{ get; set; }

        [JsonProperty(PropertyName = "lastUpdatedByName")]
        public string lastUpdatedByName{ get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public string creationDate{ get; set; }

        [JsonProperty(PropertyName = "lastUpdateDate")]
        public string lastUpdateDate{ get; set; }

        [JsonProperty(PropertyName = "inventoryCode")]
        public string inventoryCode{ get; set; }

        [JsonProperty(PropertyName = "inventoryDesc")]
        public string inventoryDesc{ get; set; }

        [JsonProperty(PropertyName = "customerReceiveFlag")]
        public string customerReceiveFlag{ get; set; }

        [JsonProperty(PropertyName = "InceptAddress")]
        public string InceptAddress{ get; set; }

        [JsonProperty(PropertyName = "contactTel")]
        public string contactTel{ get; set; }

        [JsonProperty(PropertyName = "contactName")]
        public string contactName{ get; set; }

        [JsonProperty(PropertyName = "storeName")]
        public string storeName{ get; set; }

        [JsonProperty(PropertyName = "deliveryCustomerCode")]
        public string deliveryCustomerCode{ get; set; }

        [JsonProperty(PropertyName = "deliveryCustomerName")]
        public string deliveryCustomerName{ get; set; }

        [JsonProperty(PropertyName = "fxOrderNo")]
        public string fxOrderNo{ get; set; }

        [JsonProperty(PropertyName = "sourceNo")]
        public string sourceNo{ get; set; }

        [JsonProperty(PropertyName = "transportTypeName")]
        public string transportTypeName{ get; set; }

        [JsonProperty(PropertyName = "partorgood")]
        public string partorgood{ get; set; }

        [JsonProperty(PropertyName = "singlePerson")]
        public string singlePerson{ get; set; }

        [JsonProperty(PropertyName = "singleDate")]
        public string singleDate{ get; set; }

        [JsonProperty(PropertyName = "reservationDeliveryDate")]
        public string reservationDeliveryDate{ get; set; }

        [JsonProperty(PropertyName = "channelName")]
        public string channelName{ get; set; }

        [JsonProperty(PropertyName = "channelId")]
        public string channelId{ get; set; }

        [JsonProperty(PropertyName = "receiptFlag")]
        public string receiptFlag{ get; set; }

        [JsonProperty(PropertyName = "timeSlot")]
        public string timeSlot{ get; set; }

        [JsonProperty(PropertyName = "reconciliationFlag")]
        public string reconciliationFlag{ get; set; }

        [JsonProperty(PropertyName = "inceptAreaName")]
        public string inceptAreaName{ get; set; }

        [JsonProperty(PropertyName = "dealInInfo")]
        public string dealInInfo{ get; set; }
        //
        
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class page
    {
        [JsonProperty(PropertyName = "begin")]
        public int begin{ get; set; }

        [JsonProperty(PropertyName = "length")]
        public int length{ get; set; }

        [JsonProperty(PropertyName = "count")]
        public int count{ get; set; }

        [JsonProperty(PropertyName = "totalPage")]
        public int totalPage{ get; set; }

        [JsonProperty(PropertyName = "currentPage")]
        public int currentPage{ get; set; }

        [JsonProperty(PropertyName = "isCount")]
        public bool isCount{ get; set; }

        [JsonProperty(PropertyName = "isFirst")]
        public bool isFirst { get; set; }

        [JsonProperty(PropertyName = "isLast")]
        public bool isLast { get; set; }

        [JsonProperty(PropertyName = "size")]
        public int size{ get; set; }

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class CRM_OutstockList
    {
        [JsonProperty(PropertyName = "page")]
        public page page { get; set; }
        [JsonProperty(PropertyName = "crminvexportheaderss")]
        public List<crminvexportheaderss> crminvexportheaderss { get; set; }
    }
}
