using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    public class DIS_ShopDetail
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_ShopDetail_response response { get; set; }
        public DIS_ShopDetail_profile profile { get; set; }
        public string uuid { get; set; }
        public int status { get; set; }
    }
    public class DIS_ShopDetail_response
    {
        public bool successValue { get; set; }
        public DIS_ShopDetail_response_data data { get; set; }
        public DIS_ShopDetail_response_action_list action_list { get; set; }
        public string description { get; set; }

    }
    public class DIS_ShopDetail_response_action_list
    {
        public string btnEdit { get; set; }


    }
    public class DIS_ShopDetail_response_action_list_category_list
    {
        public string category_description { get; set; }
        public decimal category_quantity { get; set; }
        public string store_no { get; set; }
        public string category_no { get; set; }
        public string operation_organization { get; set; }
    }
    public class DIS_ShopDetail_response_action_list_productsample_list
    {
        public string store_no { get; set; }
        public decimal module_quantity { get; set; }
        public string module_no { get; set; }
        public string operation_organization { get; set; }
    }
    public class DIS_ShopDetail_response_action_list_contact_address_list
    {
        public string detailed_address{ get; set; }
        public int address_no { get; set; }
        public string address_type{ get; set; }
        public string city{ get; set; }
        public string country_region_name{ get; set; }
        public string is_primary_address{ get; set; }
        public string postcode{ get; set; }
        public string language{ get; set; }
        public string country_region{ get; set; }
        public string street_name{ get; set; }
        public string province_name{ get; set; }
        public string county_district_name{ get; set; }
        public string county_district{ get; set; }
        public string city_name{ get; set; }
        public string province{ get; set; }
        public string street{ get; set; }
        public string organization{ get; set; }

    }
    public class DIS_ShopDetail_response_data
    {

        public string is_real_store { get; set; }
        public string jc_v50 { get; set; }
        public string operation_organization { get; set; }
        public List<DIS_ShopDetail_response_action_list_category_list> category_list { get; set; } = new List<DIS_ShopDetail_response_action_list_category_list>();
        public string province { get; set; }
        public string invoice_phone { get; set; }
        public string contact_no { get; set; }
        public string register_capital { get; set; }
        public string number_amount_format { get; set; }
        public string store_class { get; set; }
        public string exclusive_national_functions { get; set; }
        public int counter_quantity { get; set; }
        public string bank_account { get; set; }
        public string other_attribute9_description { get; set; }
        public string store_top { get; set; }
        public string organization_name { get; set; }
        public string store_type { get; set; }
        public string? data_created_by { get; set; }
        public string province_name { get; set; }
        public string organization_function { get; set; }
        public string billing_organization { get; set; }
        public string other_attribute9 { get; set; }
        public string open_bank { get; set; }
        public string project_no { get; set; }
        public string? warehouse_name { get; set; }
        public string INNOVATIVE_STORE_TYPES { get; set; }
        public List<string> other_attribute7 { get; set; }
        public string other_attribute8 { get; set; }
        public string other_attribute5 { get; set; }
        public List<string> other_attribute6 { get; set; }
        public List<string> other_attribute3 { get; set; } = new List<string>();
        public string other_attribute4 { get; set; }
        public string other_attribute1 { get; set; }
        public string other_attribute2 { get; set; }
        public string other_attribute10 { get; set; }
        public string? representational_city { get; set; }
        public string? gift_warehouse_name { get; set; }
        public List<string> information_list = new List<string>();
        public string time_zone_no { get; set; }
        public string city { get; set; }
        public string other_attribute10_description { get; set; }
        public string customer_level { get; set; }
        public string mobile_no { get; set; }
        public string other_attribute1_description { get; set; }
        public string data_confirmed_name { get; set; }
        public string country_region { get; set; }
        public List<string> other_attribute3_description { get; set; } = new List<string>();
        public string other_attribute2_description { get; set; }
        public string other_attribute5_description { get; set; }
        public string agency_company { get; set; }
        public string invoice_company_taxid { get; set; }
        public string other_attribute4_description { get; set; }
        public string other_attribute8_description { get; set; }
        public string is_take_inventory { get; set; }
        public string region_name { get; set; }
        public List<string> other_attribute7_description { get; set; }
        public string ka_v60 { get; set; }
        public string email { get; set; }
        public List<string> other_attribute6_description { get; set; }
        public string INNOVATIVE_STORE_TYPES_NAME { get; set; }
        public string legal_person { get; set; }
        public string upper_channel_description { get; set; }

        public int lock_version { get; set; }
        public string legal_representative { get; set; }
        public string department_name { get; set; }
        public string is_auto_confirm { get; set; }
        public string organization { get; set; }
        public string sales_area_name { get; set; }
        public string order_confirms_mode { get; set; }
        public string agency_company_name { get; set; }
        public string listed_company_no { get; set; }
        public string detailed_address { get; set; }
        public string time_zone_description { get; set; }

        public string? region_no { get; set; }
        public string sales_area { get; set; }
        public string is_straight_shop { get; set; }
        public string belonged_channel { get; set; }
        public string old_salesman_name { get; set; }
        public string contact { get; set; }
        public string is_auto_send { get; set; }
        public string is_opened_invoice { get; set; }
        public string customer_vendor_no { get; set; }
        public string? sales_target { get; set; }
        public string reference_table_no { get; set; }
        public string postcode { get; set; }
        public string is_legal_person { get; set; }
        public string data_confirmed_by { get; set; }
        public string customer_vendor_name { get; set; }
        public string organization_full_name { get; set; }
        public string performance_organization_name { get; set; }
        public string store_manager_name { get; set; }
        public string invoice_header { get; set; }
        public string business_registration_no { get; set; }
        public string reference_table_description { get; set; }
        public string market_level { get; set; }
        public string store_state { get; set; }
        public string? closing_date { get; set; }
        public decimal store_area { get; set; }
        public string sample_forsales_method { get; set; }
        public string operating_method { get; set; }
        public string status_code { get; set; }
        public string store_salesman_name { get; set; }
        public string industry_type { get; set; }
        public string street_name { get; set; }
        public string platform { get; set; }
        public string upper_channel { get; set; }
        public string company_tax_id { get; set; }
        public List<DIS_ShopDetail_response_action_list_contact_address_list> contact_address_list { get; set; } = new List<DIS_ShopDetail_response_action_list_contact_address_list>();

        public string city_name { get; set; }
        public string county_district { get; set; }
        public string performance_organization { get; set; }
        public string warehouse_no { get; set; }
        public string street { get; set; }
        public List<DIS_ShopDetail_response_action_list_productsample_list> productsample_list { get; set; } = new List<DIS_ShopDetail_response_action_list_productsample_list>();
        public string platform_name { get; set; }
        public string department { get; set; }
        public string store_salesman { get; set; }
        public string opening_date { get; set; }
        public string store_manager { get; set; }
        public string? data_created_name { get; set; }
        public string operation_organization_name { get; set; }
        public string outside_store { get; set; }
        public string country_region_name { get; set; }
        public string main_currency_description { get; set; }
        public string gift_warehouse { get; set; }
        public string old_salesman { get; set; }
        public string county_district_name { get; set; }
        public string date_display_format { get; set; }
        public string invoice_address { get; set; }
        public string main_currency { get; set; }
        public string is_with_deposit { get; set; }
        public string legal_person_name { get; set; }
        public string ka_top { get; set; }
        public string organization_simple_code { get; set; }
        public string belonged_channel_name { get; set; }
    }
    public class DIS_ShopDetail_profile
    {
        public string OrgId{ get; set; }

        public string user_type{ get; set; }

        public string UserName{ get; set; }

        public string role_list{ get; set; }
        public string primerKey{ get; set; }

        public string UserId{ get; set; }

        public string DeptUri{ get; set; }

        public string OrgName{ get; set; }

        public string DeptName{ get; set; }

        public string OrgUri{ get; set; }

        public string DeptId{ get; set; }
        [JsonProperty(PropertyName = "Program-Code")]

        public string Program_Code{ get; set; }

    }
}
