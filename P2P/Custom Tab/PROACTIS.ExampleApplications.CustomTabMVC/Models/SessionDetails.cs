using System;

namespace PROACTIS.ExampleApplications.CustomTabMVC.Models
{
    public class SessionDetails
    {
        public Guid UserGUID { get; set; }
        public string LoginID { get; set; }
        public Guid CompanyGUID { get; set; }
        public string CompanyCode { get; set; }
        public Guid DepartmentGUID { get; set; }
        public string DepartmentCode { get; set; }
        public Guid StoreGUID { get; set; }
        public string StoreCode { get; set; }
        public string SessionID { get; set; }
        public DateTime Expires { get; set; }

    }
}