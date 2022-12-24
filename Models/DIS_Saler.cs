using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    public class DIS_Saler
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_Saler_response response { get; set; }
        public DIS_Saler_profile profile { get; set; }
        public string uuid { get; set; }
        public int status { get; set; }
    }
    public class DIS_Saler_response
    {
        public bool successValue { get; set; }
        public DIS_Saler_response_data data { get; set; }
        public string description { get; set; }
    }
    public class DIS_Saler_response_data
    {
        public int rowCount{ get; set; }

        public int pageCount{ get; set; }

        public int pageSize{ get; set; }

        public int currentPage{ get; set; }
        public List<DIS_Saler_response_data_datas> datas { get; set; }

    }
    public class DIS_Saler_response_data_datas
    {
        public string detailed_address{ get; set; }
        public string status_code{ get; set; }
        public string education{ get; set; }
        public string city{ get; set; }
        public string store_no{ get; set; }
        public string date_of_birth{ get; set; }
        public string mobile_no{ get; set; }
        public string employee_no{ get; set; }
        public string graduate_institutions{ get; set; }
        public string operation_organization{ get; set; }
        public string country_region{ get; set; }
        public string street_name{ get; set; }
        public string role_description{ get; set; }
        public string city_name{ get; set; }
        public string county_district{ get; set; }
        public string province{ get; set; }
        public string major{ get; set; }
        public string contact_no{ get; set; }
        public string street{ get; set; }
        public string store_name{ get; set; }
        public string emergency_contact_phone{ get; set; }
        public string email{ get; set; }
        public string employee_job_no{ get; set; }
        public string id_number{ get; set; }
        public string operation_organization_name{ get; set; }
        public string belonged_department{ get; set; }
        public int lock_version{ get; set; }
        public string country_region_name{ get; set; }
        public string sex{ get; set; }
        public string wechat{ get; set; }
        public string postcode{ get; set; }
        public string data_created_by_name{ get; set; }
        public string employee_name{ get; set; }
        public string data_created_by{ get; set; }
        public string province_name{ get; set; }
        public string county_district_name{ get; set; }
        public string is_default_guidepurchaser{ get; set; }
        public string job_status{ get; set; }
        public string on_board_date{ get; set; }
        public string role_no{ get; set; }
        public string belonged_department_name{ get; set; }
        public string? contract_signing_date{ get; set; }
        public string emergency_contact{ get; set; }

    }
    public class DIS_Saler_profile
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
