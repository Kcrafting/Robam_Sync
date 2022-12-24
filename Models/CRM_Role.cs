using System.Collections.Generic;

namespace Models
{
    public class CRM_Roles
    {
        public List<CRM_Role> roles { get; set; }
    }
    public class CRM_Role
    {
        public int roleid { get; set; }
        public string rolecode { get; set; }
        public string rolename { get; set; }
        public string roletype { get; set; }
    }
}
