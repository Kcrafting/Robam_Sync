using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    public class DIS_SecondChannel
    {
        public int duration { get; set; }
        public string statusDescription { get; set; }
        public DIS_SecondChannel_response response { get; set; }
        public DIS_SecondChannel_profile profile { get; set; }
        public string uuid { get; set; }
        public int status { get; set; }
    }
    public class DIS_SecondChannel_response
    {
        public bool successValue { get; set; }
        public List<DIS_SecondChannel_response_data> data { get; set; }
        public string description { get; set; }
    }
    public class DIS_SecondChannel_response_data
    {
        public string option_value { get; set; }
        public string option_name { get; set; }
        public int option_sequence { get; set; }
    }
    public class DIS_SecondChannel_profile
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
