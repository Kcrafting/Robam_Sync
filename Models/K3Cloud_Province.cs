using Newtonsoft.Json.Linq;

namespace Models
{
    public class K3Cloud_Province
    {
        public JArray NeedUpDateFields { get; set; } = new JArray();
        public JArray NeedReturnFields { get; set; } = new JArray();
        public bool IsDeleteEntry { get; set; } = true;
        public string SubSystemId { get; set; } = "";
        public bool IsVerifyBaseDataField { get; set; } = false;
        public bool IsEntryBatchFill { get; set; } = true;
        public bool ValidateFlag { get; set; } = true;
        public bool NumberSearch { get; set; } = true;
        public bool IsAutoAdjustField { get; set; } = false;
        public string InterationFlags { get; set; } = "";
        public string IgnoreInterationFlag { get; set; } = "";
        public bool IsControlPrecision { get; set; } = false;
        public bool ValidateRepeatJson { get; set; } = false;
        public K3Cloud_Province_Model Model { get; set; } = new K3Cloud_Province_Model();
    }
    public class K3Cloud_Province_Model
    {
        public int FID { get; set; } = 0;
        public string FNumber { get; set; }
        public string FName { get; set; }
    }
}
    

