using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class BuyerDTO: BaseDTO
    {
        string Id { get; set; }

        FacebookAccount FacebookAccount { get; set; }

        BuyerAccountDeatails BuyerAccountDeatails { get;set;}
    
    }
}
