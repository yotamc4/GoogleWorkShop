using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class Buyer
    {
        string Id { get; set; }

        string Name { get; set; }

        FacebookAccount FacebookAccount { get; set; }

        BuyerAccountDeatails BuyerAccountDeatails { get;set;}
     

    }
}
