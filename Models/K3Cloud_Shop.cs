using Newtonsoft.Json.Linq;

namespace Models
{
    public class K3Cloud_Shop
    {
        public JArray NeedUpDateFields { get; set; }
        public JArray NeedReturnFields { get; set; }
        public bool IsDeleteEntry{ get; set; } = true;
        public string SubSystemId { get; set; }
        public bool IsVerifyBaseDataField{ get; set; } = false;
        public bool IsEntryBatchFill{ get; set; } = true;
        public bool ValidateFlag{ get; set; } = true;
        public bool NumberSearch{ get; set; } = true;
        public bool IsAutoAdjustField{ get; set; } = false;
        public string InterationFlags { get; set; }  
        public bool IgnoreInterationFlag { get; set; }
        public bool IsControlPrecision{ get; set; } = false;
        public bool ValidateRepeatJson{ get; set; } = false;
        public K3Cloud_Shop_Model Model { get; set; } = new K3Cloud_Shop_Model();
    }
    public class K3Cloud_Shop_Model
    {
        public int FID { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public string FCreateDate { get; set; }
        public K3Cloud_Common.K3Cloud_FNumber FGroup { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FProvince { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FCity { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FArea { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();
        public K3Cloud_Common.K3Cloud_FNumber FSecondChannel { get; set; } = new K3Cloud_Common.K3Cloud_FNumber();

    }
}
