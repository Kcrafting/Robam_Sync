using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class CRM_RealOutStockBillList
    {
        public List<CRM_RealOutStockBillList_crminvexportheaderss> crminvexportheaderss { get; set; }
        public CRM_RealOutStockBillList_page page { get; set; }
    }

    public class CRM_RealOutStockBillList_crminvexportheaderss
    {
        public int exOrderHeadersId { get; set; }//": 12892319,
        public string orderNo { get; set; }//": "ST20220902001351",
        public int orderTypeId { get; set; }//": 1000385,
        public string orderTypeCode { get; set; }//": "P_HAND_OUT_PLLS_CONFIRM",
        public string orderTypeName { get; set; }//": "客户零售出库单",
        public string orderDate { get; set; }//": "2022-09-02",
        public string status { get; set; }//": "D2",
        public string statusName { get; set; }//": "已接收",
        public string exportFlag { get; set; }//": "Y",
        public string exportConfirmDate { get; set; }//": "2022-09-03 11:02:25.0",
        public string exportConfirmByName { get; set; }//": "李全双",
        public string receiveFlag { get; set; }//": "Y",
        public string receiveByName { get; set; }//": "李全双",
        public string receiveDate { get; set; }//": "2022-09-03",
        public string printDate { get; set; }//": "2022-09-02 14:12:21.0",
        public int printCount { get; set; }//": 1,
        public string printLock { get; set; }//": "N",
        public string orgName { get; set; }//": "老板电器库存组织",
        public string customerName { get; set; }//": "李繁联",
        public string createdByName { get; set; }//": "李全双",
        public string creationDate { get; set; }//": "2022-09-02 14:13:44.0",
        public string lastUpdateDate { get; set; }//": "2022-09-03 11:02:40.0",
        public string inventoryCode { get; set; }//": "sjzb003001",
        public string inventoryDesc { get; set; }//": "沧州仓库",
        public string customerReceiveFlag { get; set; }//": "Y",
        public string InceptAddress { get; set; }//": "河北省沧州市运河区西环中街街道沧州市运河区颐和乐园平房颐和乐园平房2-4",
        public string contactTel { get; set; }//": "13831708955",
        public string contactName { get; set; }//": "李繁联",
        public string storeName { get; set; }//": "沧州永济路专卖店",
        public string deliveryCustomerCode { get; set; }//": "ORG-sjzb003",
        public string deliveryCustomerName { get; set; }//": "沧州市世纪厨具销售有限公司",
        public string fxOrderNo { get; set; }//": "130103S99-2209020010",
        public string sourceNo { get; set; }//": "1-6182138783",
        public string transportTypeName { get; set; }//": "自提",
        public string partorgood { get; set; }//": "GOOD",
        public string singlePerson { get; set; }//": "李玉胜",
        public string singleDate { get; set; }//": "2022-09-02 00:00:00.0",
        public string reservationDeliveryDate { get; set; }//": "2022-09-02 00:00:00.0",
        public string channelName { get; set; }//": "沧州永济路专卖店",
        public string channelId { get; set; }//": "444321",
        public string receiptFlag { get; set; }//": "N",
        public string timeSlot { get; set; }//": "-",
        public string reconciliationFlag { get; set; }//": "N",
        public string inceptAreaName { get; set; }//": "运河区",
        public string dealInInfo { get; set; }//": "处理成功"
    }
    public class CRM_RealOutStockBillList_page
    {
        public int begin { get; set; }//": 0,
        public int length { get; set; }//": 50,
        public int count { get; set; }//": 81,
        public int totalPage { get; set; }//": 2,
        public int currentPage { get; set; }//": 1,
        public bool isCount { get; set; }//": true,
        public bool isFirst { get; set; }//": true,
        public bool isLast { get; set; }//": false,
        public int size { get; set; }//": 50
    }
}
