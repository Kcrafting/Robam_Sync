using System.Collections.Generic;

namespace Models
{

    public class CrmPriceLineVsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int? priceLineId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? priceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? materialId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? materialCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? materialType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? discount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? taxFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? unitCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? startQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? endQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? beginDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? endDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? createdBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? creationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? lastUpdatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? lastUpdateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? isPossFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? attribute1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? attribute2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? attribute3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? repairFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? repairType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? keyFlag { get; set; }
        /// <summary>
        /// 超级用户
        /// </summary>
        public string? creator { get; set; }
        /// <summary>
        /// 超级用户
        /// </summary>
        public string? updatetor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? productModelCode { get; set; }
        /// <summary>
        /// 238泡沫
        /// </summary>
        public string? materialName { get; set; }
        /// <summary>
        /// 238泡沫
        /// </summary>
        public string? materialDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? attribute1Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? applyPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? customerPrice { get; set; }
    }

    public class CRM_PartsPrice_Page
    {
        /// <summary>
        /// 
        /// </summary>
        public int? begin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? totalPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? currentPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? isCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? isFirst { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? isLast { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? size { get; set; }
    }

    public class CRM_PartsPrice
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CrmPriceLineVsItem> crmPriceLineVs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CRM_PartsPrice_Page page { get; set; }
    }

}
