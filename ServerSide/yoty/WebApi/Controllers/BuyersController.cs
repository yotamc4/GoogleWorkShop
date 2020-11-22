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
        public async Task<ActionResult<IList<BuyerDTO>>> GetBuyers([FromBody] IList<string> buyersIds)
        {
            try
            {
                var result = await buyersManager.GetBuyers(buyersIds);
                return Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
