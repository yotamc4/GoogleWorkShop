// Copyright (c) YOTY Corporation and contributors. All rights reserved.

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
        private IBidsManager bidsManager;

        public BidsController(IBidsManager bidsManager)
        {
            this.bidsManager = bidsManager;
        }

        [HttpGet]
        [Route("{Category}")]
        public async Task<ActionResult<IList<BidDTO>>> GetBids([FromQuery] BidsFilters bidsFilters)
        {
            var str = "2";
            return this.Ok(str);
        }

        [HttpGet]
        [Route("Ping")]
        public async Task<string> Ping()
        {
            return "Hi";
        }

        [HttpPost]
        [Route("NewBid")]
        public async Task<ActionResult<string>> PostNewBid(NewBidRequst bid)
        {
            try
            {
                var newBidId = await this.bidsManager.CreateNewBid(bid).ConfigureAwait(false); ;
                return Ok(newBidId);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("{bidId}/Buyers")]
        public async Task<IList<BuyerDTO>> GetProductBidBuyers(string bidId)
        {
            return await bidsManager.GetBuyers(bidId);
        }
        /*
        [HttpPost]
        [Route("{bidId}/Buyers/{buyerId}")]
        public async Task<ActionResult<string>> PostNewBuyerToBid(string productBidId, string buyerId, BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            try
            {
                await this.bidsManager.AddBuyerToBid(productBidId, buyerId, bidBuyerJoinRequest).ConfigureAwait(false);
                return Ok(8); // checking
            }

            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Inner Failure");
            }
        }
        */
    }
}
