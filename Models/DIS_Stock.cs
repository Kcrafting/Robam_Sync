using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    //public class DIS_Stock
    //{

    //}

    public class DIS_Stock
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_Stock_response response { get; set; }
        public DIS_Stock_profile profile { get; set; }
        public string uuid { get; set; }
        public string status { get; set; }

    }
    public class DIS_Stock_response
    {
        public bool successValue { get; set; }
        public DIS_Stock_response_data data { get; set; }
        public string description { get; set; }
    }
    public class DIS_Stock_response_data
    {
        public int rowCount { get; set; }
        public int pageCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public List<DIS_Stock_response_data_datas> datas { get; set; }
    }
    public class DIS_Stock_response_data_datas
    {
        public string detailed_address { get; set; }//": "",
        public string status_code { get; set; }//": "Y",
        public string is_inventory_available { get; set; }//": "Y",
        public string city { get; set; }//": "",
        public string management_object { get; set; }//": "1301030198",
        public string logistics_area { get; set; }//": "",
        public string operation_organization { get; set; }//": "130103",
        public string country_region { get; set; }//": "CN",
        public string street_name { get; set; }//": "",
        public string warehouse_category { get; set; }//": "4",
        public string warehouse_application { get; set; }//": "0",
        public string county_district { get; set; }//": "",
        public string city_name { get; set; }//": "",
        public string province { get; set; }//": "",
        public string warehouse_no { get; set; }//": "1301030198",
        public string contact_no { get; set; }//": "",
        public string street { get; set; }//": "",
        public string contact { get; set; }//": "",
        public string management_object_description { get; set; }//": "沧州东光星艺佳专卖店",
        public string logistics_area_name { get; set; }//": null,
        public string operation_organization_name { get; set; }//": "沧州",
        public int lock_version { get; set; }//": 1,
        public string not_takeinto_inventory { get; set; }//": "",
        public string country_region_name { get; set; }//": "中国",
        public string source_type { get; set; }//": "2",
        public string is_participate_replenishment { get; set; }//": "N",
        public string province_name { get; set; }//": "",
        public string county_district_name { get; set; }//": "",
        public string warehouse_name { get; set; }//": "沧州东光星艺佳专卖店",
        public string is_transit_warehouse { get; set; }//": "N",
        public string purchase_channel { get; set; }//": "",
        public string purchase_channel_description { get; set; }//": null,
        public string is_default_warehouse { get; set; }//": "Y",
        public string entrust_type { get; set; }//": "01"

    }
    public class DIS_Stock_profile
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
        public string Program_Code { get; set; }
    }

}
