using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Managers
{
    public interface IBuyersManager
    {
        Task<IList<Buyer>> GetBuyers(IList<string> buyersIds);
        Task<IList<Buyer>> GetBuyers(string productBidId);

    }
}
