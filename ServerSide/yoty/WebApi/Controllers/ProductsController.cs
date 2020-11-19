using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YOTY.Service.WebApi.PublicDataSchemas;
using YOTY.Service.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace YOTY.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController: ControllerBase
    {
        private IProductsBidsManager productsBidsManager;

        public ProductsController(IProductsBidsManager productsBidsManager)
        {
            this.productsBidsManager = productsBidsManager;
        }

        [HttpPost]
        [Route("NewBid")]
        public async Task<ActionResult<string>> PostNewBid(ProductBid productBid)
        {
            try
            {
                var newBidId = await this.productsBidsManager.CreateNewBid(productBid).ConfigureAwait(false); ;
                return Ok(newBidId);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("BuyersOfProductBid/{bidId}/PostBuyer/{buyerId}")]
        public async Task<ActionResult<string>> PostNewBuyerToBid(BuyerJoinToBidRequest buyerJoinToBidRequest)
        {
            try
            {
                var newBidId = await this.productsBidsManager.CreateNewBid(productBid).ConfigureAwait(false); ;
                return Ok(newBidId);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
