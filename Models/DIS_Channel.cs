using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Channel
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_Channel_Response response { get; set; }
        public DIS_Channel_Profile profile { get; set; }
        public string uuid { get; set; }
        public int status { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Channel_Response
    {
        public DIS_Channel_Response_Data data { get; set; }
        public bool successValue { get; set; }
        public DIS_Channel_Response_Action_list action_list { get; set; }
        public string description { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Channel_Response_Data
    {
            public string account_payable_category{ get; set; }//": "",
            public string legal_person_type{ get; set; }//": "1",
            public string sales_department_name{ get; set; }//": "创新渠道部",
            public string top_channel_description{ get; set; }//": "创新渠道",
            public string customer_vendor_type{ get; set; }//": "1",
            public string higher_merge_settlement{ get; set; }//": "Y",
            public string customer_currency_description{ get; set; }//": "人民币",
            public string channel{ get; set; }//": "020401",
            public string customer_attribute9_description{ get; set; }//": "",
            public string customer_attribute8_description{ get; set; }//": "",
            public string operation_organization{ get; set; }//": "130103",
            public string quota_currency{ get; set; }//": "CNY",
            public string collection_type{ get; set; }//": "1",
            public string quota_customer{ get; set; }//": "",
            public string customer_attribute5_description{ get; set; }//": "",
            public string customer_attribute4_description{ get; set; }//": "",
            public string customer_attribute6_description{ get; set; }//": "",
            public string province{ get; set; }//": "440000",
            public string customer_attribute3_description{ get; set; }//": "",
            public string customer_attribute7_description{ get; set; }//": "",
            public string invoice_phone{ get; set; }//": "",
            public string contact_no{ get; set; }//": "",
            public string default_operating_method{ get; set; }//": "2",
            public string customer_vendor{ get; set; }//": "1000013677",
            public string fax{ get; set; }//": "",
            public string top_channel{ get; set; }//": "02",
            public string bank_account{ get; set; }//": "812781161110001",
            public string customer_importance_grade{ get; set; }//": "",
            public string group_name{ get; set; }//": "",
            public string customer_importance_description{ get; set; }//": "",
            public string customer_currency{ get; set; }//": "CNY",
            public string default_sales_department{ get; set; }//": "111L2000",
            public string province_name{ get; set; }//": "广东省",
            public string channel_property{ get; set; }//": "1",
            public string created_organization_name{ get; set; }//": "总公司",
            public string open_bank{ get; set; }//": "招商银行深南中路支行",
            public string customer_attribute10{ get; set; }//": "",
            public string arm_summary_method{ get; set; }//": "3",
            public string refused_reason_description{ get; set; }//": "",
            public string organization_external_no{ get; set; }//": "",
            public string customer_lifecycle_description{ get; set; }//": "",
            public string issue_object{ get; set; }//": "2",
            public string delivery_object_description{ get; set; }//": "",
            public string account_object_description{ get; set; }//": "怡亚通",
            public string account_receivable_type{ get; set; }//": "",
            public string is_settlement_price{ get; set; }//": "N",
            public string city{ get; set; }//": "440300",
            public string revenue_confirmation_document{ get; set; }//": "2",
            public string mobile_no{ get; set; }//": "13430859391",
            public string customer_level{ get; set; }//": "平台&设计师",
            public string invoice_time{ get; set; }//": "3",
            public string country_region{ get; set; }//": "CN",
            public string clearing_reconciliation{ get; set; }//": "1",
            public string default_salesman{ get; set; }//": "201881",
            public string price_settlement_method{ get; set; }//": "2",
            public string credit_rating_formula{ get; set; }//": "",
            public string is_stockout{ get; set; }//": "N",
            public string enterprise_credit_quota{ get; set; }//": 0.000000,
            public string customer_vendor_shortname{ get; set; }//": "怡亚通",
            public string collection_term{ get; set; }//": "",
            public string balance_settlement{ get; set; }//": "Y",
            public string income_organization{ get; set; }//": "100001",
            public string ka_v60{ get; set; }//": "",
            public string email{ get; set; }//": "",
            public string invocing_method{ get; set; }//": "1",
            public string channel_name{ get; set; }//": "平台客户",
            public string legal_person{ get; set; }//": "",
            public string express_platform_id{ get; set; }//": "",
            public string address{ get; set; }//": "海旺社区N26区海秀路2021号荣超滨海大厦 A 座2111",
            public string lock_version{ get; set; }//": 1,
            public string quick_appsalesorder{ get; set; }//": "N",
            public string creditrating_formula_description{ get; set; }//": "",
            public string customer_attribute1_description{ get; set; }//": "",
            public string customer_attribute2_description{ get; set; }//": "",
            public string invoice_mailing_method{ get; set; }//": "2",
            public string expense_by_order{ get; set; }//": "",
            public string settlement_template_description{ get; set; }//": "创新渠道（总部/跨区域大盘）",
            public string invoiced_before_delivery{ get; set; }//": "N",
            public string customer_attribute1{ get; set; }//": "",
            public string customer_attribute2{ get; set; }//": "",
            public string customer_external_no{ get; set; }//": "",
            public string customer_attribute5{ get; set; }//": "",
            public string responsible_organization{ get; set; }//": "",
            public string default_billing_organization{ get; set; }//": "100001",
            public string customer_attribute6{ get; set; }//": "",
            public string quota_currency_description{ get; set; }//": "人民币",
            public string customer_attribute3{ get; set; }//": "",
            public string customer_attribute4{ get; set; }//": "",
            public string customer_attribute9{ get; set; }//": "",
            public string income_organization_name{ get; set; }//": "总公司",
            public string customer_attribute7{ get; set; }//": "",
            public string customer_attribute8{ get; set; }//": "",
            public string order_check_characteristic{ get; set; }//": "",
            public string language{ get; set; }//": "zh_CN",
            public string option_name{ get; set; }//": null,
            public string credit_check_mode{ get; set; }//": "1",
            public string quota_customer_name{ get; set; }//": "怡亚通",
            public string belonged_group{ get; set; }//": "",
            public string customer_billingtype_description{ get; set; }//": "",
            public string account_object{ get; set; }//": "1000013677",
            public string entrust_type_description{ get; set; }//": "零售",
            public string settlement_template{ get; set; }//": "6",
            public string old_salesman_name{ get; set; }//": null,
            public string contact{ get; set; }//": "蔡金鹏",
            public string customer_tax_rate{ get; set; }//": 13.00,
            public string default_salesman_name{ get; set; }//": "方芸",
            public string settlement_rate{ get; set; }//": 0.000000,
            public string customer_group_description{ get; set; }//": "",
            public string customer_billing_type{ get; set; }//": "2",
            public string distributionserviceno_is_required{ get; set; }//": "N",
            public string is_reconciliation_settlement{ get; set; }//": "1",
            public string regulation_inventory{ get; set; }//": "Y",
            public string postcode{ get; set; }//": "",
            public string customer_attribute10_description{ get; set; }//": "",
            public string is_contract_management{ get; set; }//": "N",
            public string customer_rate_base{ get; set; }//": "1",
            public string collection_term_description{ get; set; }//": "",
            public string is_collection_inbehalf{ get; set; }//": "N",
            public decimal credit_used{ get; set; }//": 0.000000,
            public string customer_tax_description{ get; set; }//": "增值税",
            public string accountpayable_category_description{ get; set; }//": "",
            public string customer_group{ get; set; }//": "",
            public string is_tax{ get; set; }//": "Y",
            public string created_organization{ get; set; }//": "100001",
            public string responsible_organization_name{ get; set; }//": "",
            public string accounting_entrust_type{ get; set; }//": "2",
            public string invoice_header{ get; set; }//": "深圳市怡亚通供应链股份有限公司",
            public string customer_type{ get; set; }//": "",
            public string note{ get; set; }//": "",
            public string status_code{ get; set; }//": "Y",
            public string jc_s50{ get; set; }//": "",
            public string is_invoice_entrust{ get; set; }//": "Y",
            public string contract_autoextension_months{ get; set; }//": null,
            public string street_name{ get; set; }//": "新安街道",
            public string ordercollection_confirmedby_financialrecognition{ get; set; }//": "",
            public string company_tax_id{ get; set; }//": "91440300279398406U",
            public string customer_nature{ get; set; }//": "2",
            public string city_name{ get; set; }//": "深圳市",
            public string county_district{ get; set; }//": "440306",
            public string strike_balance_document{ get; set; }//": "2",
            public string take_inventory{ get; set; }//": "",
            public string street{ get; set; }//": "4403064",
            public string appsalesorder_by_sku{ get; set; }//": "",
            public string business_type{ get; set; }//": "",
            public string customer_type_description{ get; set; }//": "",
            public string c_embed_soft{ get; set; }//": "N",
            public string customer_lifecycle_maintenance{ get; set; }//": "2",
            public string operation_organization_name{ get; set; }//": "沧州",
            public string customer_tax_code{ get; set; }//": "A9",
            public string country_region_name{ get; set; }//": "中国",
            public string issue_source_document{ get; set; }//": "2",
            public string underwriting_machine_appsalesorder{ get; set; }//": "N",
            public string check_verification_code{ get; set; }//": "",
            public string old_salesman{ get; set; }//": "",
            public string county_district_name{ get; set; }//": "宝安区",
            public string invoice_address{ get; set; }//": "",
            public string legal_person_name{ get; set; }//": "",
            public string customer_vendor_fullname{ get; set; }//": "深圳市怡亚通供应链股份有限公司",
            public string ar_type_description{ get; set; }//": "",
            public string report_app_expenses{ get; set; }//": "",
            public string delivery_object{ get; set; }//": "",
            public string billing_organization_name{ get; set; }//": "总公司",
            public string entrust_type{ get; set; }//": "01"
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Channel_Response_Action_list
    {
        public string btnCopy { get; set; }//": "Y",
        public string btnEdit { get; set; }//": "Y",
        public string btnRejectAudit { get; set; }//": "N"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_Channel_Profile
    {
        public string OrgId { get; set; }//": "130103",
        public string user_type { get; set; }//": "3",
        public string UserName { get; set; }//": "李玉胜",
        public string role_list { get; set; }//": "{role_name=一般员工, role_no=0001}",
        public string primerKey { get; set; }//": "1301031002",
        public string UserId { get; set; }//": "1301031002",
        public string DeptUri { get; set; }//": "",
        public string OrgName { get; set; }//": "沧州",
        public string DeptName { get; set; }//": "财务部",
        public string OrgUri { get; set; }//": "",
        public string DeptId { get; set; }//": "13010302",
        [JsonProperty(PropertyName = "Program-Code")]
        public string Program_Code { get; set; }//": "cnm/drp_cnm_s05/drp_cnm_s05_s01"
    }
}
