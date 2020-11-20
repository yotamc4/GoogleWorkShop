//using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YOTY.Service.Managers.Bids;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BidsController: ControllerBase
    {
        private IBidsManager productsBidsManager;

        public BidsController(IBidsManager productsBidsManager)
        {
            this.productsBidsManager = productsBidsManager;
        }

        [HttpGet]
        [Route("Ping")]
        public async Task<string> Ping()
        {
            return "Hi";
        }

        [HttpPost]
        [Route("NewBid")]
        public async Task<ActionResult<string>> PostNewBid(Bid bid)
        {
            try
            {
                var newBidId = await this.productsBidsManager.CreateNewBid(bid).ConfigureAwait(false); ;
                return Ok(newBidId);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("{bidId}/Buyers")]
        public async Task<IList<Buyer>> GetProductBidBuyers(string bidId)
        {
            return await productsBidsManager.GetBuyers(bidId);
        }

        [HttpPost]
        [Route("{bidId}/Buyers/{buyerId}")]
        public async Task<ActionResult<string>> PostNewBuyerToBid(string productBidId, string buyerId, BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            try
            {
                await this.productsBidsManager.AddBuyerToBid(productBidId, buyerId, bidBuyerJoinRequest).ConfigureAwait(false);
                return Ok(8); // checking
            }

            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Inner Failure");
            }
        }

    }
}
