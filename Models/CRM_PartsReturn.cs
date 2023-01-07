using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    public class CRM_PartsReturn
    {
        public List<CRM_PartsReturn_crmsoorderheaders> crmsoorderheaders { get; set; }
        public CRM_PartsReturn_page page { get; set; }
    }
    public class CRM_PartsReturn_page
    {
        public int begin { get; set; }//": 0,
        public int length { get; set; }//": 20,
        public int count { get; set; }//": 3335,
        public int totalPage { get; set; }//": 167,
        public int currentPage { get; set; }//": 1,
        public bool isCount { get; set; }//": true,
        public bool isFirst { get; set; }//": true,
        public bool isLast { get; set; }//": false,
        public int size { get; set; }//": 20
    }
    public class CRM_PartsReturn_crmsoorderheaders
    {
      public int orderId{get;set;}// 34069225,
      public string orderNum{get;set;}// "JSTH202301050098",
      public int salesAreaId {get;set;}// 66820,
      public string salesAreaName{get;set;}// "石家庄流巨家电销售有限公司",
      public int orderTypeId {get;set;}// 212,
      public string orderTypeName{get;set;}// "技师配件退回",
      public string status{get;set;}// "C",
      public string creationDate{get;set;}// "2023-01-05",
      public string customerCode{get;set;}// "JS-031130042",
      public string customerName{get;set;}// "崔文星",
      public string customerTypeName{get;set;}// "技师",
      public string deliveryCustomerCode{get;set;}// "ORG-sjzb003",
      public string deliveryCustomerName{get;set;}// "沧州市世纪厨具销售有限公司",
      public string deliveryCustomerTypeName{get;set;}// "办事处-经销商",
      public string deliveryWarehouseName{get;set;}// "崔文星-技师仓",
      public string receiveInventoryName{get;set;}// "沧州仓库",
      public string deliveryWarehouseCode{get;set;}// "INV-031130042",
      public string receiveInventoryCode{get;set;}// "sjzb003001",
      public string exportAreaName{get;set;}// null,
      public string inceptAreaName{get;set;}// null,
      public string writeRaIfFlag{get;set;}// null,
      public string exportFlag{get;set;}// "Y",
      public string exportFinishFlag{get;set;}// "Y",
      public string needErpFlag{get;set;}// "N",
      public string erpFlag{get;set;}// "N",
      public string isInTransit{get;set;}// "Y",
      public string reservationDeliveryDate{get;set;}// null,
      public string customerContact{get;set;}// null,
      public string contactTel{get;set;}// null,
      public string remark{get;set;}// "APP技师配件退回",
      public string createdName{get;set;}// "ROBAM_APP",
      public string lastUpdatedName{get;set;}// "ROBAM_APP",
      public string lastUpdateDate{get;set;}// "2023-01-05",
      public string goodsTypeName{get;set;}// "赊账额度池",
      public string approvalName{get;set;}// "超级用户",
      public string functionType{get;set;}// "PARTS_RETURN",
      public int acUserId {get;set;}// 4964588,
      public string branchApprovalTime{get;set;}// "2023-01-05",
      public string statusName{get;set;}// "审批通过",
      public string orderCommitTime{get;set;}// "2023-01-05",
      public string isInternationalTrade{get;set;}// null,
      public string queryEntityId{get;set;}// "$queryEntityId$"
    }
}
