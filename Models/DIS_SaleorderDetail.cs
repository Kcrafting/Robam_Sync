using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail
    {
        [JsonProperty(PropertyName = "duration")]
        public int? Duration { get; set; }
        [JsonProperty(PropertyName = "statusDescription")]
        public string? StatusDescription { get; set; }
        [JsonProperty(PropertyName = "response")]
        public DIS_SaleorderDetail_Response Response { get; set; }
        [JsonProperty(PropertyName = "profile")]
        public DIS_SaleorderDetail_Profile Profile { get; set; }
        [JsonProperty(PropertyName = "uuid")]
        public string? Uuid { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }
        
        
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail_Data
    {
        [JsonProperty(PropertyName = "detailData")]
        public List<DIS_SaleorderDetail_DataDetail> detailData { get; set; }

        [JsonProperty(PropertyName = "residential_quarter")]
        public string? residential_quarter{ get; set; }// "130103XQ-2208220001"

        [JsonProperty(PropertyName = "data_created_date")]
        public string? data_created_date{ get; set; }// "2022/08/22 13:47:03"

        [JsonProperty(PropertyName = "document_property")]
        public string? document_property{ get; set; }// "1"

        [JsonProperty(PropertyName = "delivery_organization")]
        public string? delivery_organization{ get; set; }// "130103"

        [JsonProperty(PropertyName = "is_exist")]
        public string? is_exist{ get; set; }// "Y"

        [JsonProperty(PropertyName = "operation_organization")]
        public string? operation_organization{ get; set; }// "130103"

        [JsonProperty(PropertyName = "old_customer_vendor")]
        public string? old_customer_vendor{ get; set; }// ""

        [JsonProperty(PropertyName = "collection_type")]
        public string? collection_type{ get; set; }// "1"

        [JsonProperty(PropertyName = "invoice_note")]
        public string? invoice_note{ get; set; }// ""

        [JsonProperty(PropertyName = "audit_supplement_status")]
        public string? audit_supplement_status{ get; set; }// ""

        [JsonProperty(PropertyName = "return_exchange_treatment")]
        public string? return_exchange_treatment{ get; set; }// ""

        [JsonProperty(PropertyName = "province")]
        public string? province{ get; set; }// "130000"

        [JsonProperty(PropertyName = "invoice_phone")]
        public string? invoice_phone{ get; set; }// ""

        [JsonProperty(PropertyName = "contact_no")]
        public string? contact_no{ get; set; }// ""

        [JsonProperty(PropertyName = "invoicing_organization_name")]
        public string? invoicing_organization_name{ get; set; }// "沧州"

        [JsonProperty(PropertyName = "sms_reply")]
        public string? sms_reply{ get; set; }// ""

        [JsonProperty(PropertyName = "remote_fee")]
        public decimal? remote_fee{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "expect_collected")]
        public decimal? expect_collected{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "voided_by")]
        public string? voided_by{ get; set; }// ""

        [JsonProperty(PropertyName = "bank_account")]
        public string? bank_account{ get; set; }// ""

        [JsonProperty(PropertyName = "belonged_area_name")]
        public string? belonged_area_name{ get; set; }// ""

        [JsonProperty(PropertyName = "exchange_rate")]
        public decimal? exchange_rate{ get; set; }// 1.000000

        [JsonProperty(PropertyName = "total_exchange_amount")]
        public decimal? total_exchange_amount{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "voided_name")]
        public string? voided_name{ get; set; }// null

        [JsonProperty(PropertyName = "data_confirmed_date")]
        public string? data_confirmed_date{ get; set; }// "Aug 23

        [JsonProperty(PropertyName = "send_ticket_method")]
        public string? send_ticket_method{ get; set; }// "1"

        [JsonProperty(PropertyName = "document_date")]
        public string? document_date{ get; set; }// "2022/08/22"

        [JsonProperty(PropertyName = "source_type")]
        public string? source_type{ get; set; }// "1"

        [JsonProperty(PropertyName = "couponid")]
        public string? couponid{ get; set; }// ""

        [JsonProperty(PropertyName = "belonged_channel_no")]
        public string? belonged_channel_no{ get; set; }// "010905"

        [JsonProperty(PropertyName = "data_created_by")]
        public string? data_created_by{ get; set; }// "130103000080"

        [JsonProperty(PropertyName = "province_name")]
        public string? province_name{ get; set; }// "河北省"

        [JsonProperty(PropertyName = "distribution_status")]
        public string? distribution_status{ get; set; }// "3"

        [JsonProperty(PropertyName = "channel_property")]
        public string? channel_property{ get; set; }// "1"

        [JsonProperty(PropertyName = "contract_type")]
        public string? contract_type{ get; set; }// ""

        [JsonProperty(PropertyName = "created_organization_name")]
        public string? created_organization_name{ get; set; }// "沧州"

        [JsonProperty(PropertyName = "open_bank")]
        public string? open_bank{ get; set; }// ""

        [JsonProperty(PropertyName = "refused_reason_description")]
        public string? refused_reason_description{ get; set; }// ""

        [JsonProperty(PropertyName = "deposit")]
        public decimal? deposit{ get; set; }// 6380.000000

        [JsonProperty(PropertyName = "issue_object")]
        public string? issue_object{ get; set; }// "1"

        [JsonProperty(PropertyName = "billing_method")]
        public string? billing_method{ get; set; }// "3"

        [JsonProperty(PropertyName = "is_settlement_price")]
        public string? is_settlement_price{ get; set; }// "N"

        [JsonProperty(PropertyName = "city")]
        public string? city{ get; set; }// "130900"

        [JsonProperty(PropertyName = "mobile_no")]
        public string? mobile_no{ get; set; }// "18832775458"

        [JsonProperty(PropertyName = "belonged_major_area")]
        public string? belonged_major_area{ get; set; }// "G001"

        [JsonProperty(PropertyName = "refused_reasoncode_description")]
        public string? refused_reasoncode_description{ get; set; }// ""

        [JsonProperty(PropertyName = "data_confirmed_name")]
        public string? data_confirmed_name{ get; set; }// "李玉胜"

        [JsonProperty(PropertyName = "country_region")]
        public string? country_region{ get; set; }// "CN"

        [JsonProperty(PropertyName = "clearing_reconciliation")]
        public string? clearing_reconciliation{ get; set; }// "2"

        [JsonProperty(PropertyName = "guide_purchaser_name")]
        public string? guide_purchaser_name{ get; set; }// "白真真"

        [JsonProperty(PropertyName = "reservation_delivery_date")]
        public string? reservation_delivery_date{ get; set; }// "2022/08/23"

        [JsonProperty(PropertyName = "is_stockout")]
        public string? is_stockout{ get; set; }// "N"

        [JsonProperty(PropertyName = "document_no")]
        public string? document_no{ get; set; }// "130103S99-2208220003"

        [JsonProperty(PropertyName = "is_take_inventory")]
        public string? is_take_inventory{ get; set; }// ""

        [JsonProperty(PropertyName = "old_customer_name")]
        public string? old_customer_name{ get; set; }// null

        [JsonProperty(PropertyName = "source_document")]
        public string? source_document{ get; set; }// "130103S01-2208220002"

        [JsonProperty(PropertyName = "currency")]
        public string? currency{ get; set; }// "CNY"

        [JsonProperty(PropertyName = "delivery_org_check")]
        public string? delivery_org_check{ get; set; }// ""

        [JsonProperty(PropertyName = "notconfirmed_crmintegration_status")]
        public string? notconfirmed_crmintegration_status{ get; set; }// "1"

        [JsonProperty(PropertyName = "email")]
        public string? email{ get; set; }// ""

        [JsonProperty(PropertyName = "document_type")]
        public string? document_type{ get; set; }// "1"

        [JsonProperty(PropertyName = "contract_no")]
        public string? contract_no{ get; set; }// ""

        [JsonProperty(PropertyName = "distribution_service_no")]
        public string? distribution_service_no{ get; set; }// "bai820"

        [JsonProperty(PropertyName = "delivery_organization_name")]
        public string? delivery_organization_name{ get; set; }// "沧州"

        [JsonProperty(PropertyName = "lock_version")]
        public int? lock_version{ get; set; }// 8

        [JsonProperty(PropertyName = "invoicing_organization")]
        public string? invoicing_organization{ get; set; }// "130103"

        [JsonProperty(PropertyName = "deduction_amount")]
        public decimal? deduction_amount{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "invoice_count")]
        public int? invoice_count{ get; set; }// 0

        [JsonProperty(PropertyName = "tax_description")]
        public string? tax_description{ get; set; }// "13"

        [JsonProperty(PropertyName = "expenseData")]
        public JArray expenseData{ get; set; }// []

        [JsonProperty(PropertyName = "total_document_amount")]
        public decimal? total_document_amount{ get; set; }// 6380.000000

        [JsonProperty(PropertyName = "is_recognized_income")]
        public string? is_recognized_income{ get; set; }// "N"

        [JsonProperty(PropertyName = "tax_code")]
        public string? tax_code{ get; set; }// "A9"

        [JsonProperty(PropertyName = "guide_purchaser")]
        public string? guide_purchaser{ get; set; }// "130103000080"

        [JsonProperty(PropertyName = "currency_description")]
        public string? currency_description{ get; set; }// "人民币"

        [JsonProperty(PropertyName = "introduction_sources")]
        public string? introduction_sources{ get; set; }// ""

        [JsonProperty(PropertyName = "belonged_area")]
        public string? belonged_area{ get; set; }// ""

        [JsonProperty(PropertyName = "customer_name")]
        public string? customer_name{ get; set; }// "南皮信和"

        [JsonProperty(PropertyName = "crm_integration_status")]
        public string? crm_integration_status{ get; set; }// "1"

        [JsonProperty(PropertyName = "belonged_department_name")]
        public string? belonged_department_name{ get; set; }// "县区销售部"

        [JsonProperty(PropertyName = "detailed_address")]
        public string? detailed_address{ get; set; }// "龙御墅"

        [JsonProperty(PropertyName = "store_no")]
        public string? store_no{ get; set; }// "1301030104"

        [JsonProperty(PropertyName = "guide_salary")]
        public string? guide_salary{ get; set; }// ""

        [JsonProperty(PropertyName = "belonged_top_channel")]
        public string? belonged_top_channel{ get; set; }// "01"

        [JsonProperty(PropertyName = "crm_integration_status2")]
        public string? crm_integration_status2{ get; set; }// "1"

        [JsonProperty(PropertyName = "tax_rate")]
        public decimal? tax_rate{ get; set; }// 13.00

        [JsonProperty(PropertyName = "difference_amount")]
        public decimal? difference_amount{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "distributionserviceno_is_required")]
        public string? distributionserviceno_is_required{ get; set; }// "Y"

        [JsonProperty(PropertyName = "customer_no")]
        public string? customer_no{ get; set; }// "1301030076"

        [JsonProperty(PropertyName = "regulation_inventory")]
        public string? regulation_inventory{ get; set; }// "Y"

        [JsonProperty(PropertyName = "belonged_department")]
        public string? belonged_department{ get; set; }// "13010305"

        [JsonProperty(PropertyName = "amount_received")]
        public decimal? amount_received{ get; set; }// 0.000000

        [JsonProperty(PropertyName = "postcode")]
        public string? postcode{ get; set; }// ""

        [JsonProperty(PropertyName = "data_confirmed_by")]
        public string? data_confirmed_by { get; set; }// "1301031002"
        [JsonProperty(PropertyName = "print_times")]
        public string? print_times { get; set; }// ": null

        [JsonProperty(PropertyName = "is_collection_inbehalf")]
        public string? is_collection_inbehalf { get; set; }// ": "N"

        [JsonProperty(PropertyName = "performance_organization_name")]
        public string? performance_organization_name { get; set; }// ": "石家庄"

        [JsonProperty(PropertyName = "road_sign_no")]
        public string? road_sign_no { get; set; }// ": ""

        [JsonProperty(PropertyName = "is_tax")]
        public string? is_tax { get; set; }// ": "Y"

        [JsonProperty(PropertyName = "promotion_program")]
        public string? promotion_program { get; set; }// ": ""

        [JsonProperty(PropertyName = "salesman")]
        public string? salesman { get; set; }// ": "190210"

        [JsonProperty(PropertyName = "created_organization")]
        public string? created_organization { get; set; }// ": "100001"

        [JsonProperty(PropertyName = "invoice_header")]
        public string? invoice_header { get; set; }// ": "志邦家居股份有限公司"

        [JsonProperty(PropertyName = "note")]
        public string? note { get; set; }// ": "null


        [JsonProperty(PropertyName = "salesman_name")]
        public string? salesman_name { get; set; }// ": "刘艺帆"

        [JsonProperty(PropertyName = "operating_method")]
        public string? operating_method { get; set; }// ": "2"

        [JsonProperty(PropertyName = "coupondetails")]
        public string? coupondetails { get; set; }// ": ""

        [JsonProperty(PropertyName = "status_code")]
        public string? status_code { get; set; }// ": "Y"

        [JsonProperty(PropertyName = "deposit_location")]
        public string? deposit_location { get; set; }// ": ""

        [JsonProperty(PropertyName = "street_name")]
        public string? street_name{ get; set; }// ": ""

        [JsonProperty(PropertyName = "couponname")]
        public string? couponname{ get; set; }// ": ""

        [JsonProperty(PropertyName = "company_tax_id")]
        public string? company_tax_id{ get; set; }// ": "91340100772816763N"

        [JsonProperty(PropertyName = "goods_return_status")]
        public string? goods_return_status{ get; set; }// ": "1"

        [JsonProperty(PropertyName = "city_name")]
        public string? city_name{ get; set; }// ": "廊坊市"

        [JsonProperty(PropertyName = "county_district")]
        public string? county_district{ get; set; }// ": "131024"

        [JsonProperty(PropertyName = "refused_reason_code")]
        public string? refused_reason_code{ get; set; }// ": ""

        [JsonProperty(PropertyName = "performance_organization")]
        public string? performance_organization{ get; set; }// ": "130101"

        [JsonProperty(PropertyName = "street")]
        public string? street{ get; set; }// ": ""

        [JsonProperty(PropertyName = "appsalesorder_by_sku")]
        public string? appsalesorder_by_sku{ get; set; }// ": ""

        [JsonProperty(PropertyName = "promotion_program_description")]
        public string? promotion_program_description{ get; set; }// ": ""

        [JsonProperty(PropertyName = "store_name")]
        public string? store_name{ get; set; }// ": ""

        [JsonProperty(PropertyName = "client_name")]
        public string? client_name{ get; set; }// ": "张海波"

        [JsonProperty(PropertyName = "data_created_name")]
        public string? data_created_name{ get; set; }// ": "章鑫涛"

        [JsonProperty(PropertyName = "other_note")]
        public string? other_note{ get; set; }// ": ""

        [JsonProperty(PropertyName = "operation_organization_name")]
        public string? operation_organization_name{ get; set; }// ": "总公司"

        [JsonProperty(PropertyName = "country_region_name")]
        public string? country_region_name{ get; set; }// ": "中国"

        [JsonProperty(PropertyName = "is_ka_channel")]
        public string? is_ka_channel{ get; set; }// ": "C"

        [JsonProperty(PropertyName = "source_order_no")]
        public string? source_order_no{ get; set; }// ": "100001S99-2209011602"

        [JsonProperty(PropertyName = "introduction_sources_phone")]
        public string? introduction_sources_phone{ get; set; }// ": ""

        [JsonProperty(PropertyName = "county_district_name")]
        public string? county_district_name{ get; set; }// ": "香河县"

        [JsonProperty(PropertyName = "designer_code")]
        public string? designer_code{ get; set; }// ": ""

        [JsonProperty(PropertyName = "invoice_address")]
        public string? invoice_address{ get; set; }// ": ""

        [JsonProperty(PropertyName = "is_innovation")]
        public string? is_innovation{ get; set; }// ": ""

        [JsonProperty(PropertyName = "min_total_salesprice")]
        public string? min_total_salesprice{ get; set; }// ": 4176.000000

        [JsonProperty(PropertyName = "belonged_major_areaname")]
        public string? belonged_major_areaname{ get; set; }// ": "总部"

        [JsonProperty(PropertyName = "belonged_channel_name")]
        public string? belonged_channel_name{ get; set; }// ": "战略定制"

        [JsonProperty(PropertyName = "residential_quarter_name")]
        public string? residential_quarter_name{ get; set; }// ": ""

        [JsonProperty(PropertyName = "top_channel_name")]
        public string? top_channel_name{ get; set; }// ": "创新渠道"

        [JsonProperty(PropertyName = "is_transfer_crm")]
        public string? is_transfer_crm{ get; set; }// ": "Y"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail_Response
    {
        [JsonProperty(PropertyName = "data")]
        public DIS_SaleorderDetail_Data Data { get; set; }
        [JsonProperty(PropertyName = "action_list")]
        public DIS_SaleorderDetail_ActionList Action_list { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string? description { get; set; }

    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail_DataDetail
    {
        [JsonProperty(PropertyName = "tax_amount")]
        public decimal? tax_amount{ get; set; }//": 4495.000000

        [JsonProperty(PropertyName = "actual_goods_characteristic")]
        public string? actual_goods_characteristic{ get; set; }//": "W"

        [JsonProperty(PropertyName = "deposit_rate")]
        public decimal? deposit_rate{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "expense_allocation")]
        public string? expense_allocation{ get; set; }//": null

        [JsonProperty(PropertyName = "characteristic_group")]
        public string? characteristic_group{ get; set; }//": "A"

        [JsonProperty(PropertyName = "category_no")]
        public string? category_no{ get; set; }//": "A1"

        [JsonProperty(PropertyName = "stockout_method")]
        public string? stockout_method{ get; set; }//": ""

        [JsonProperty(PropertyName = "inspection_accept_quantity")]
        public decimal? inspection_accept_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "management_characteristic_description")]
        public string? management_characteristic_description{ get; set; }//": null

        [JsonProperty(PropertyName = "is_since")]
        public string? is_since{ get; set; }//": "N"

        [JsonProperty(PropertyName = "lifecycle")]
        public string? lifecycle{ get; set; }//": "001"

        [JsonProperty(PropertyName = "not_shipped_quantity")]
        public decimal? not_shipped_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "balance_date")]
        public string? balance_date{ get; set; }//": null

        [JsonProperty(PropertyName = "warehouse_category")]
        public string? warehouse_category{ get; set; }//": "1"

        [JsonProperty(PropertyName = "promotion_rule_description")]
        public string? promotion_rule_description{ get; set; }//": ""

        [JsonProperty(PropertyName = "shipped_quantity")]
        public decimal? shipped_quantity{ get; set; }//": 1.000000

        [JsonProperty(PropertyName = "settlement_quantity")]
        public decimal? settlement_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "output_invoicing_quantity")]
        public decimal? output_invoicing_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "expense_no")]
        public string? expense_no{ get; set; }//": ""

        [JsonProperty(PropertyName = "bill_model_name")]
        public string? bill_model_name{ get; set; }//": "吸油烟机"

        [JsonProperty(PropertyName = "balance_no")]
        public string? balance_no{ get; set; }//": ""

        [JsonProperty(PropertyName = "balance_employee_no")]
        public string? balance_employee_no{ get; set; }//": ""

        [JsonProperty(PropertyName = "is_break_even")]
        public string? is_break_even{ get; set; }//": "N"

        [JsonProperty(PropertyName = "inspection_accept_history")]
        public string? inspection_accept_history{ get; set; }//": ""

        [JsonProperty(PropertyName = "lifecycle_description")]
        public string? lifecycle_description{ get; set; }//": "TR6"

        [JsonProperty(PropertyName = "package")]
        public string? package{ get; set; }//": ""

        [JsonProperty(PropertyName = "settlement_date")]
        public string? settlement_date{ get; set; }//": null

        [JsonProperty(PropertyName = "rebate_type")]
        public string? rebate_type{ get; set; }//": "1"

        [JsonProperty(PropertyName = "finance_received")]
        public decimal? finance_received{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "factory_price")]
        public decimal? factory_price{ get; set; }//": 1950.000000

        [JsonProperty(PropertyName = "bill_selling_goods")]
        public string? bill_selling_goods{ get; set; }//": "8333S.W"

        [JsonProperty(PropertyName = "unit_description")]
        public string? unit_description{ get; set; }//": "台"

        [JsonProperty(PropertyName = "sort")]
        public string? sort{ get; set; }//": null

        [JsonProperty(PropertyName = "expense_allocation2")]
        public string? expense_allocation2{ get; set; }//": null

        [JsonProperty(PropertyName = "reconciliation_no")]
        public string? reconciliation_no{ get; set; }//": ""

        [JsonProperty(PropertyName = "distribution_status")]
        public string? distribution_status{ get; set; }//": "3"

        [JsonProperty(PropertyName = "income_account_quantity")]
        public decimal? income_account_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "actual_model_no")]
        public string? actual_model_no{ get; set; }//": "8333S"

        [JsonProperty(PropertyName = "unit")]
        public string? unit{ get; set; }//": "PCS"

        [JsonProperty(PropertyName = "delivery_date")]
        public string? delivery_date{ get; set; }//": "2022/08/24"

        [JsonProperty(PropertyName = "warehouse_name")]
        public string? warehouse_name{ get; set; }//": "沧州仓库"

        [JsonProperty(PropertyName = "bill_model_no")]
        public string? bill_model_no{ get; set; }//": "8333S"

        [JsonProperty(PropertyName = "no_tax_amount")]
        public decimal? no_tax_amount{ get; set; }//": 3977.880000

        [JsonProperty(PropertyName = "minimum_selling_price")]
        public decimal? minimum_selling_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "finance_actual_received")]
        public decimal? finance_actual_received{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "promotion_program")]
        public string? promotion_program{ get; set; }//": ""

        [JsonProperty(PropertyName = "extend_warranty")]
        public int? extend_warranty{ get; set; }//": 0

        [JsonProperty(PropertyName = "balance_employee_name")]
        public string? balance_employee_name{ get; set; }//": ""

        [JsonProperty(PropertyName = "express_company_name")]
        public string? express_company_name{ get; set; }//": null

        [JsonProperty(PropertyName = "is_take_delivery")]
        public string? is_take_delivery{ get; set; }//": "N"

        [JsonProperty(PropertyName = "channel_sku")]
        public string? channel_sku{ get; set; }//": ""

        [JsonProperty(PropertyName = "return_date")]
        public string? return_date{ get; set; }//": null

        [JsonProperty(PropertyName = "actual_characteristic_description")]
        public string? actual_characteristic_description{ get; set; }//": "无"

        [JsonProperty(PropertyName = "note")]
        public string? note{ get; set; }//": ""

        [JsonProperty(PropertyName = "status_code")]
        public string? status_code{ get; set; }//": "1"

        [JsonProperty(PropertyName = "return_quantity")]
        public decimal? return_quantity{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "is_settlement_price")]
        public string? is_settlement_price{ get; set; }//": "N"

        [JsonProperty(PropertyName = "settlement_price")]
        public decimal? settlement_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "actual_selling_goods")]
        public string? actual_selling_goods{ get; set; }//": "8333S.W"

        [JsonProperty(PropertyName = "document_item")]
        public int? document_item{ get; set; }//": 1

        [JsonProperty(PropertyName = "supply_price")]
        public decimal? supply_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "suggested_selling_price")]
        public decimal? suggested_selling_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "category_description")]
        public string? category_description{ get; set; }//": "油烟机"

        [JsonProperty(PropertyName = "price_settlement_method")]
        public string? price_settlement_method{ get; set; }//": "2"

        [JsonProperty(PropertyName = "goods_return_status")]
        public string? goods_return_status{ get; set; }//": "1"

        [JsonProperty(PropertyName = "bill_modelcharacteristic_description")]
        public string? bill_modelcharacteristic_description{ get; set; }//": "无"

        [JsonProperty(PropertyName = "is_special_offer")]
        public string? is_special_offer{ get; set; }//": ""

        [JsonProperty(PropertyName = "warehouse_no")]
        public string? warehouse_no{ get; set; }//": "sjzb003001"

        [JsonProperty(PropertyName = "document_no")]
        public string? document_no{ get; set; }//": "130103S99-2208220003"

        [JsonProperty(PropertyName = "promotion_program_description")]
        public string? promotion_program_description{ get; set; }//": ""

        [JsonProperty(PropertyName = "bill_price")]
        public decimal? bill_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "suggested_retail_price")]
        public decimal? suggested_retail_price{ get; set; }//": 10000.000000

        [JsonProperty(PropertyName = "express_number")]
        public string? express_number{ get; set; }//": ""

        [JsonProperty(PropertyName = "actual_goods_name")]
        public string? actual_goods_name{ get; set; }//": "吸油烟机"

        [JsonProperty(PropertyName = "goods_status")]
        public string? goods_status{ get; set; }//": "1"

        [JsonProperty(PropertyName = "quantity")]
        public decimal? quantity{ get; set; }//": 1.000000

        [JsonProperty(PropertyName = "settlement_status")]
        public string? settlement_status{ get; set; }//": "1"

        [JsonProperty(PropertyName = "actual_price")]
        public decimal? actual_price{ get; set; }//": 4495.000000

        [JsonProperty(PropertyName = "lock_version")]
        public int? lock_version{ get; set; }//": 3

        [JsonProperty(PropertyName = "line_no")]
        public string? line_no{ get; set; }//": ""

        [JsonProperty(PropertyName = "promotion_rule")]
        public string? promotion_rule{ get; set; }//": ""

        [JsonProperty(PropertyName = "management_characteristic")]
        public string? management_characteristic{ get; set; }//": null

        [JsonProperty(PropertyName = "return_voucher_amount")]
        public decimal? return_voucher_amount{ get; set; }//": 0.000000

        [JsonProperty(PropertyName = "is_balance")]
        public string? is_balance{ get; set; }//": "N"

        [JsonProperty(PropertyName = "package_item")]
        public int? package_item{ get; set; }//": 0

        [JsonProperty(PropertyName = "bill_model_characteristic")]
        public string? bill_model_characteristic{ get; set; }//": "W"

        [JsonProperty(PropertyName = "is_zb_send")]
        public string? is_zb_send{ get; set; }//": "N"

        [JsonProperty(PropertyName = "express_company_id")]
        public string? express_company_id{ get; set; }//": ""

        [JsonProperty(PropertyName = "goods_type")]
        public string? goods_type{ get; set; }//": "1"

        [JsonProperty(PropertyName = "expense_no2")]
        public string? expense_no2{ get; set; }//": ""
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail_Profile
    {
        [JsonProperty(PropertyName = "OrgId")]
        public string? OrgId{ get; set; }//": "130103",

        [JsonProperty(PropertyName = "user_type")]
        public string? user_type{ get; set; }//": "3",

        [JsonProperty(PropertyName = "UserName")]
        public string? UserName{ get; set; }//": "李玉胜",

        [JsonProperty(PropertyName = "role_list")]
        public string? role_list{ get; set; }//": "{role_name=一般员工, role_no=0001}",

        [JsonProperty(PropertyName = "primerKey")]
        public string? primerKey{ get; set; }//": "1301031002",

        [JsonProperty(PropertyName = "UserId")]
        public string? UserId{ get; set; }//": "1301031002",

        [JsonProperty(PropertyName = "DeptUri")]
        public string? DeptUri{ get; set; }//": "",

        [JsonProperty(PropertyName = "OrgName")]
        public string? OrgName{ get; set; }//": "沧州",

        [JsonProperty(PropertyName = "DeptName")]
        public string? DeptName{ get; set; }//": "财务部",

        [JsonProperty(PropertyName = "OrgUri")]
        public string? OrgUri{ get; set; }//": "",

        [JsonProperty(PropertyName = "DeptId")]
        public string? DeptId{ get; set; }//": "13010302",

        [JsonProperty(PropertyName = "Program_Code")]
        public string? Program_Code{ get; set; }//": "drp_sam_s02_s01"
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleorderDetail_ActionList
    {
        [JsonProperty(PropertyName = "btnEditSNCode")]
        public string? btnEditSNCode{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnOtherNote")]
        public string? btnOtherNote{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnUpdateDeposit")]
        public string? btnUpdateDeposit{ get; set; }//": "N",

        [JsonProperty(PropertyName = "btnEdit")]
        public string? btnEdit{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "OrgId")]
        public string? btnGrantGift{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnGrantGift")]
        public string? btnEditForExecutor{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnSpecialData")]
        public string? btnSpecialData{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnEditInvoice")]
        public string? btnEditInvoice{ get; set; }//": "Y",

        [JsonProperty(PropertyName = "btnEditSalesDate")]
        public string? btnEditSalesDate{ get; set; }//": "N",

        [JsonProperty(PropertyName = "btnNumModify")]
        public string? btnNumModify{ get; set; }//": "Y"
    }
}
