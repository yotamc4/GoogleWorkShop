using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class Response<DTObject>: Response  
    {
        DTObject DTOObject { get; set; }
    }

    public class Response
    {
        bool IsOperationSucceded { get; set; }

        string SuccessFailureMessage { get; set; }

    }
}
