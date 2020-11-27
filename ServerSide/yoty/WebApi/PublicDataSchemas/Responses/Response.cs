using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class Response<DTObject>: Response  
    {
        public DTObject DTOObject { get; set; }
    }

    public class Response
    {
        public bool IsOperationSucceded { get; set; }

        public string SuccessFailureMessage { get; set; }

    }
}
