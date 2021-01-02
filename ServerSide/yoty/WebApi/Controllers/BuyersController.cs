// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using YOTY.Service.Core.Managers.Buyers;
    using YOTY.Service.WebApi.PublicDataSchemas;

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
        public async Task<ActionResult> CreateBuyer(NewUserRequest newUserRequest)
        {
            Response response = await this.buyersManager.CreateBuyer(newUserRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        public async Task<ActionResult<BuyerDTO>> Getbuyer(string buyerId)
        {
            Response<BuyerDTO> response = await this.buyersManager.GetBuyer(buyerId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);           
        }

        [HttpGet]
        [Route("{buyerId}/bids")]
        public async Task<ActionResult<BidsDTO>> GetBuyerBids(string buyerId, [FromQuery] BuyerBidsRequestOptions buyerBidsRequestOptions)
        {
            Response<BidsDTO> response = buyerBidsRequestOptions.IsCreatedByBuyer ?
                await this.buyersManager.GetBidsCreatedByBuyer(buyerId, buyerBidsRequestOptions.BidsTime).ConfigureAwait(false) :
                await this.buyersManager.GetBuyerBids(buyerId, buyerBidsRequestOptions.BidsTime).ConfigureAwait(false);

            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }

            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("details")]
        public async Task<ActionResult> ModifyBuyerDetails(ModifyBuyerDetailsRequest request)
        {
            Response response = await this.buyersManager.ModifyBuyerDetails(request);

            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }

            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }
    }
}
