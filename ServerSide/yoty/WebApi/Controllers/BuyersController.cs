using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using YOTY.Service.WebApi.PublicDataSchemas;
using YOTY.Service.Managers;

namespace YOTY.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BuyersController: ControllerBase
    {
        private IBuyersManager buyersManager;
        public BuyersController(IBuyersManager buyersManager)
        {
            this.buyersManager = buyersManager;
        }

        [HttpGet]
        [Route("Buyers")]
        public async Task<IActionResult<IList<Buyer>>> GetBuyers([FromBody] IList<string> buyersIds)
        {
            try
            {
                var result = await buyersManager.GetBuyers(buyersIds);
                return Ok(result);
            }
            catch
            {
                return BadRequestResult();
            }
        }

        [HttpGet]
        [Route("BuyersOfProductBid/{bidId}")]
        public async Task<IList<Buyer>> GetProductBidBuyers(string bidId)
        {
            return await buyersManager.GetBuyers(bidId);
        }
    }
}
