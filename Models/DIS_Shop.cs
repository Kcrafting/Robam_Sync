using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class DIS_Shop
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_Shop_response response { get; set; }
        public DIS_Shop_profile profile { get; set; }
        public int uuid { get; set; }
        public string status { get; set; }

    }
    public class DIS_Shop_response
    {
        public bool successValue { get; set; }
        public DIS_Shop_response_data data { get; set; }
        public string description { get; set; }
    }
    public class DIS_Shop_response_data
    {
        public int rowCount { get; set; }
        public int pageCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public List<DIS_Shop_response_data_datas> datas { get; set; }
    }
    public class DIS_Shop_response_data_datas
    {
        public string market_level{ get; set; }
        public string detailed_address{ get; set; }
        public string operating_method{ get; set; }
        public string status_code{ get; set; }
        public string store_salesman_name{ get; set; }
        public string sales_area{ get; set; }
        public string is_straight_shop{ get; set; }
        public string operation_organization{ get; set; }
        public string platform{ get; set; }
        public string belonged_channel{ get; set; }
        public string warehouse_no{ get; set; }
        public string contact_no{ get; set; }
        public string? old_salesman_name{ get; set; }
        public string contact{ get; set; }
        public string store_class{ get; set; }
        public string platform_name{ get; set; }
        public string? region_name{ get; set; }
        public string department{ get; set; }
        public string store_salesman{ get; set; }
        public string is_auto_send{ get; set; }
        public string is_opened_invoice{ get; set; }
        public string customer_vendor_no{ get; set; }
        public string legal_person{ get; set; }
        public string? sales_target{ get; set; }
        public string operation_organization_name{ get; set; }
        public string? take_inventory_releasetime{ get; set; }
        public int lock_version{ get; set; }
        public string upper_channel_description{ get; set; }
        public string gift_warehouse{ get; set; }
        public string department_name{ get; set; }
        public string organization_name{ get; set; }
        public string store_type{ get; set; }
        public string is_auto_confirm{ get; set; }
        public string old_salesman{ get; set; }
        public string organization_function{ get; set; }
        public string customer_vendor_name{ get; set; }
        public string project_no{ get; set; }
        public string? warehouse_name{ get; set; }
        public string organization_full_name{ get; set; }
        public string legal_person_name{ get; set; }
        public string organization{ get; set; }
        public string sales_area_name{ get; set; }
        public string belonged_channel_name{ get; set; }
        public string order_confirms_mode{ get; set; }
        public string? gift_warehouse_name{ get; set; }
        public string agency_company_name{ get; set; }

    }
    public class DIS_Shop_profile
    {
        public string OrgId { get; set; }
        public string user_type { get; set; }
        public string UserName { get; set; }
        public string role_list { get; set; }
        public string primerKey { get; set; }
        public string UserId { get; set; }
        public string DeptUri { get; set; }
        public string OrgName { get; set; }
        public string DeptName { get; set; }
        public string OrgUri { get; set; }
        public string DeptId { get; set; }
        [JsonProperty(PropertyName = "Program-Code")]
        public string Program_Code{ get; set; }
}
}
