using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class K3Cloud_RobamAccount
    {
        public string FAccount { get; set; }
        public string FPWD { get; set; }
        public string FAccountType { get; set; }
        public string FAccountArea { get; set; }
        public string FAccountRight { get; set; }
        public string FCompany { get; set; }
    }
}
