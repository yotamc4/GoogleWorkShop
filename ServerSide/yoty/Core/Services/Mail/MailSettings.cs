using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.Core.Services.Mail
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailSecrets
    {
        public string Password { get; set; }

        public string ConnectionString { get; set; }
    }
}
