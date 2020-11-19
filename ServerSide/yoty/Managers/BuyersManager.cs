using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.Managers;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace yoty.Managers
{
    public class BuyersManager : IBuyersManager
    {
        public Task<IList<Buyer>> GetBuyers(IList<string> buyersIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Buyer>> GetBuyers(string productBidId)
        {
            throw new NotImplementedException();
        }
    }
}
