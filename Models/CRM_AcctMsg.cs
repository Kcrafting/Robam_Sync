using System.Collections.Generic;

namespace Models
{
    public class CRM_AcctMsg
    {
        public List<CRM_AcctIns> sobs { get; set; }
    }
    public class CRM_AcctIns
    {
        public int roleid { get; set; }
        public int orgId { get; set; }
        public string orgName { get; set; }
        public string orgCode { get; set; }
        public string orgTypeName { get; set; }
    }
}
