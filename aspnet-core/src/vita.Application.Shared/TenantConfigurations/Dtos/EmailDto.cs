using System;
using System.Collections.Generic;
using System.Text;

namespace vita.TenantConfigurations.Dtos
{
    public class EmailDto
    {
        public int smtpPort { get; set; }
        public string fromAddress { get; set; }
        public string defaultFromDisplayName { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string smtpHost { get; set; }
        public string smtpDomain { get; set; }
        public string smtpUserName { get; set; }
        public string smtpPassword { get; set; }
        public string ccEmails { get; set; }
        public bool isenableemail { get; set; }
    }
}
