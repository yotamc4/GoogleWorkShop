using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using YOTY.Service.WebApi.PublicDataSchemas;
using YOTY.Service.Core.Managers.Buyers;

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

        [HttpPost]
        public async Task<ActionResult<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest)
        {
            Response<BuyerDTO> response = await this.buyersManager.CreateBuyer(newBuyerRequest).ConfigureAwait(false);
            if (response.IsOperationSuccsseded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        public async Task<ActionResult<BuyerDTO>> Getbuyer(string buyerId)
        {
            Response<BuyerDTO> response = await this.buyersManager.GetBuyer(buyerId).ConfigureAwait(false);
            if (response.IsOperationSuccsseded)
            {
                
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);           
        }
    }
}
