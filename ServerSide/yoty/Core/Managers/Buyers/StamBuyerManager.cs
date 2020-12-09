using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Buyers
{
    public class StamBuyerManager : IBuyersManager
    {
        public Task<IList<BuyerDTO>> GetBuyers(IList<string> buyersIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BuyerDTO>> GetBuyers(string productBidId)
        {
            throw new NotImplementedException();
        }
    }
}
