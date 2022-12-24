using System.Collections.Generic;

namespace Models
{
    //public class CRM_PartsInstock
    //{

    //}


    public class CrmsoorderheadersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int salesAreaId { get; set; }
        /// <summary>
        /// 石家庄流巨家电销售有限公司
        /// </summary>
        public string salesAreaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int orderTypeId { get; set; }
        /// <summary>
        /// 配件零售退回
        /// </summary>
        public string orderTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string creationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerCode { get; set; }
        /// <summary>
        /// 沧州市世纪厨具销售有限公司-零售客户
        /// </summary>
        public string customerName { get; set; }
        /// <summary>
        /// 零售客户
        /// </summary>
        public string customerTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliveryCustomerCode { get; set; }
        /// <summary>
        /// 沧州市世纪厨具销售有限公司
        /// </summary>
        public string deliveryCustomerName { get; set; }
        /// <summary>
        /// 办事处-经销商
        /// </summary>
        public string deliveryCustomerTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliveryWarehouseName { get; set; }
        /// <summary>
        /// 沧州仓库
        /// </summary>
        public string receiveInventoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deliveryWarehouseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string receiveInventoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exportAreaName { get; set; }
        /// <summary>
        /// 运河区
        /// </summary>
        public string inceptAreaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string writeRaIfFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exportFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exportFinishFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string needErpFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isInTransit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reservationDeliveryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerContact { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contactTel { get; set; }
        /// <summary>
        /// 青县信誉楼售后  未付款  沧州市运河区永济路水岸骏景2号门市老板电器 3040198
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 贾文静
        /// </summary>
        public string createdName { get; set; }
        /// <summary>
        /// 贾文静
        /// </summary>
        public string lastUpdatedName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastUpdateDate { get; set; }
        /// <summary>
        /// 付费配件池
        /// </summary>
        public string goodsTypeName { get; set; }
        /// <summary>
        /// 超级用户
        /// </summary>
        public string approvalName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string functionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int acUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string branchApprovalTime { get; set; }
        /// <summary>
        /// 审批通过
        /// </summary>
        public string statusName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderCommitTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isInternationalTrade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string queryEntityId { get; set; }
    }

    public class Page
    {
        /// <summary>
        /// 
        /// </summary>
        public int begin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isFirst { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isLast { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
    }

    public class CRM_PartsInstock
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CrmsoorderheadersItem> crmsoorderheaders { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Page page { get; set; }
    }

}
