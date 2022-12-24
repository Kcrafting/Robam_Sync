using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Saleorder
    {
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [JsonProperty(PropertyName = "statusDescription")]
        public string StatusDescription { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "response")]
        public DIS_Saleorder_Response Response { get; set; }
        [JsonProperty(PropertyName = "profile")]
        public DIS_Saleorder_Profile Profile { get; set; }


    }
    public class DIS_Saleorder_Data
    {
        [JsonProperty(PropertyName = "detailed_address")]
        public string detailed_address;//": "献县",

        [JsonProperty(PropertyName = "store_no")]
        public string store_no;//": "",

        [JsonProperty(PropertyName = "document_property")]
        public string document_property;//": "1",

        [JsonProperty(PropertyName = "delivery_organization")]
        public string delivery_organization;//": "130103",

        [JsonProperty(PropertyName = "actual_models")]
        public string actual_models;//": "61A1,7B19,储恩刀具（10）",

        [JsonProperty(PropertyName = "operation_organization")]
        public string operation_organization;//": "130103",

        [JsonProperty(PropertyName = "belonged_top_channel")]
        public string belonged_top_channel;//": "01",

        [JsonProperty(PropertyName = "audit_supplement_status")]
        public string audit_supplement_status;//": "",

        [JsonProperty(PropertyName = "return_exchange_treatment")]
        public string return_exchange_treatment;//": "",

        [JsonProperty(PropertyName = "is_picture_upload")]
        public string is_picture_upload;//": "N",

        [JsonProperty(PropertyName = "expect_collected")]
        public decimal expect_collected;//": 0.000000,

        [JsonProperty(PropertyName = "customer_no")]
        public string customer_no;//": "1000013349",

        [JsonProperty(PropertyName = "document_date")]
        public string document_date;//": "2022/08/24",

        [JsonProperty(PropertyName = "source_type")]
        public string source_type;//": "2",

        [JsonProperty(PropertyName = "data_created_by")]
        public string data_created_by;//": "1301031002",

        [JsonProperty(PropertyName = "print_times")]
        public string print_times;//": null,

        [JsonProperty(PropertyName = "is_collection_inbehalf")]
        public string is_collection_inbehalf;//": "N",

        [JsonProperty(PropertyName = "province_name")]
        public string province_name;//": "河北省",

        [JsonProperty(PropertyName = "distribution_status")]
        public string distribution_status;//": "1",

        [JsonProperty(PropertyName = "refused_reason_description")]
        public string refused_reason_description;//": "",

        [JsonProperty(PropertyName = "road_sign_no")]
        public string road_sign_no;//": "",

        [JsonProperty(PropertyName = "operating_method")]
        public string operating_method;//": "1",

        [JsonProperty(PropertyName = "status_code")]
        public string status_code;//": "Y",

        [JsonProperty(PropertyName = "mobile_no")]
        public string mobile_no;//": "15128799251",

        [JsonProperty(PropertyName = "refused_reasoncode_description")]
        public string refused_reasoncode_description;//": "",

        [JsonProperty(PropertyName = "street_name")]
        public string street_name;//": "淮镇镇",

        [JsonProperty(PropertyName = "guide_purchaser_name")]
        public string guide_purchaser_name;//": "",

        [JsonProperty(PropertyName = "reservation_delivery_date")]
        public string reservation_delivery_date;//": "2022/08/25",

        [JsonProperty(PropertyName = "goods_return_status")]
        public string goods_return_status;//": "1",

        [JsonProperty(PropertyName = "city_name")]
        public string city_name;//": "沧州市",

        [JsonProperty(PropertyName = "refused_reason_code")]
        public string refused_reason_code;//": "",

        [JsonProperty(PropertyName = "is_stockout")]
        public string is_stockout;//": "N",

        [JsonProperty(PropertyName = "document_no")]
        public string document_no;//": "130103S99-2208240021",

        [JsonProperty(PropertyName = "store_name")]
        public string store_name;//": "",

        [JsonProperty(PropertyName = "source_document")]
        public string source_document;//": "",

        [JsonProperty(PropertyName = "currency")]
        public string currency;//": "CNY",

        [JsonProperty(PropertyName = "client_name")]
        public string client_name;//": "王强",

        [JsonProperty(PropertyName = "total_expense")]
        public decimal total_expense;//": 0.000000,

        [JsonProperty(PropertyName = "distribution_service_no")]
        public string distribution_service_no;//": "133446445",

        [JsonProperty(PropertyName = "operation_organization_name")]
        public string operation_organization_name;//": "沧州",

        [JsonProperty(PropertyName = "delivery_organization_name")]
        public string delivery_organization_name;//": "沧州",

        [JsonProperty(PropertyName = "lock_version")]
        public decimal lock_version;//": 5,

        [JsonProperty(PropertyName = "employee_name")]
        public string employee_name;//": "李玉胜",

        [JsonProperty(PropertyName = "source_order_no")]
        public string source_order_no;//": "130103S99-2208240021",

        [JsonProperty(PropertyName = "introduction_sources_phone")]
        public string introduction_sources_phone;//": "",

        [JsonProperty(PropertyName = "county_district_name")]
        public string county_district_name;//": "献县",

        [JsonProperty(PropertyName = "total_document_amount")]
        public decimal total_document_amount;//": 3900.000000,

        [JsonProperty(PropertyName = "guide_purchaser")]
        public string guide_purchaser;//": "",

        [JsonProperty(PropertyName = "min_total_salesprice")]
        public decimal min_total_salesprice;//": 3796.000000,

        [JsonProperty(PropertyName = "currency_description")]
        public string currency_description;//": "人民币",

        [JsonProperty(PropertyName = "introduction_sources")]
        public string introduction_sources;//": "",

        [JsonProperty(PropertyName = "customer_name")]
        public string customer_name;//": "服务终端带单销售客户"
    }
    public class DIS_Saleorder_Response
    {
        [JsonProperty(PropertyName = "successValue")]
        public bool SuccessValue { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "data")]
        public List<DIS_Saleorder_Data> Data { get; set; }
       
    }
    public class DIS_Saleorder_Profile
    {
        [JsonProperty(PropertyName = "OrgId")]
        public string OrgId;//": "130103",

        [JsonProperty(PropertyName = "user_type")]
        public string user_type;//": "3",

        [JsonProperty(PropertyName = "UserName")]
        public string UserName;//": "李玉胜",

        [JsonProperty(PropertyName = "role_list")]
        public string role_list;//": "{role_name=一般员工, role_no=0001}",

        [JsonProperty(PropertyName = "primerKey")]
        public string primerKey;//": "1301031002",

        [JsonProperty(PropertyName = "UserId")]
        public string UserId;//": "1301031002",

        [JsonProperty(PropertyName = "DeptUri")]
        public string DeptUri;//": "",

        [JsonProperty(PropertyName = "OrgName")]
        public string OrgName;//": "沧州",

        [JsonProperty(PropertyName = "DeptName")]
        public string DeptName;//": "财务部",

        [JsonProperty(PropertyName = "OrgUri")]
        public string OrgUri;//": "",

        [JsonProperty(PropertyName = "DeptId")]
        public string DeptId;//": "13010302",

        [JsonProperty(PropertyName = "Program_Code")]
        public string Program_Code;//": "drp_sam_s02_s01"
    }
}
