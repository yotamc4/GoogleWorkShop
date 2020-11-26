using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YOTY.Service.WebApi.PublicDataSchemas;
using YOTY.Service.Managers;

namespace YOTY.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController
    {
        private IBuyersManager buyersManager;
        public void BuyersController(IBuyersManager buyersManager)
        {
            this.buyersManager = buyersManager;
        }

        [HttpGet]
        [Route("Buyers")]
        public async Task<IList<Buyer>> GetBuyers([FromBody] IList<string> buyersIds)
        {
            return await buyersManager.GetBuyers(buyersIds);
        }

        [HttpGet]
        [Route("BuyersOfProductBid/{bidId}")]
        public async Task<IList<Buyer>> GetBuyers([FromQuery] string bidId)
        {
            return await buyersManager.GetBuyers(bidId);
        }
    }
}
