using Newtonsoft.Json.Linq;

namespace Models
{
    public class K3Cloud_Push
    {
        public string Ids { get; set; } = "";
        public JArray Numbers{get;set;} = new JArray();
        public string EntryIds { get; set; } = "";
        public string RuleId { get; set; } = "";
        public string TargetBillTypeId { get; set; } = "";
        public string TargetFormId { get; set; } = "";
        public bool IsEnableDefaultRule { get; set; } = false;
        public bool IsDraftWhenSaveFail { get; set; } = false;
        public JObject CustomParams { get; set; } = new JObject();
    }
}
