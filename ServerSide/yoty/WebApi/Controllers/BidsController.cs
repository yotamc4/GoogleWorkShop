﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YOTY.Service.Core.Managers.Bids;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.WebApi.Controllers
{
    // The controller has designed by the API best-practises doc here:https://hackernoon.com/restful-api-designing-guidelines-the-best-practices-60e1d954e7c9
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
        public async Task<ActionResult<List<BidDTO>>> GetBids([FromQuery] BidsQueryOptions bidsFilters)
        {
            return new List<BidDTO> {
                new BidDTO {
                    Id = " first bid in list",
                    Category = bidsFilters.Category,
                },
            };
        }


        [HttpPost]
        public async Task<ActionResult<BidDTO>> PostNewBid(NewBidRequst bid)
        {
            Response<BidDTO> response = await this.bidsManager.CreateNewBid(bid).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessFailureMessage);
        }

        [HttpGet]
        public async Task<ActionResult<BidDTO>> GetBid(string bidId)
        {
            return new BidDTO {
                Id = "onebid",
            };
            /*
            Response<BidDTO> response = await this.bidsManager.GetBid(bidId).ConfigureAwait(false);
            if (response.IsOperationSuccsseded)
            {
                
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
            */
        }

        [HttpGet]
        [Route("{bidId}/buyers")]
        public async Task<ActionResult<IList<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            Response<IList<BuyerDTO>> response = await this.bidsManager.GetBidBuyers(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/proposals")]
        public async Task<ActionResult<IList<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            Response<IList<SupplierProposalDTO>> response = await this.bidsManager.GetBidSuplliersProposals(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessFailureMessage);
        }


        
        [HttpPost]
        [Route("{bidId}/Buyers")]
        public async Task<ActionResult<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {

            Response<BuyerDTO> response = await this.bidsManager.AddBuyer(bidBuyerJoinRequest).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/Proposals")]
        public async Task<ActionResult<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposalRequest)
        {

            Response<SupplierProposalDTO> response = await this.bidsManager.AddSupplierProposal(supplierProposalRequest).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessFailureMessage);
        }

        [HttpPut]
        [Route("{bidId}")]
        public async Task<ActionResult<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {

            Response<BidDTO> response = await this.bidsManager.EditBid(editBidRequest).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return response.DTOObject;
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessFailureMessage);
        }


        [HttpDelete]
        [Route("{bidId}/Buyers/{buyerId}")]
        public async Task<ActionResult> DeleteBuyer(string bidId, string buyerId)
        {

            Response response = await this.bidsManager.DeleteBuyer(bidId, buyerId).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessFailureMessage);
        }


        [HttpDelete]
        [Route("{bidId}/Proposals/{proposalId}")]
        public async Task<ActionResult> DeleteSupplierProposal(string bidId, string proposalId)
        {

            Response response = await this.bidsManager.DeleteSupplierProposal(bidId, proposalId).ConfigureAwait(false);
            if (response.IsOperationSucceded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessFailureMessage);
        }

        [HttpGet]
        [Route("Ping")]
        public async Task<string> Ping()
        {
            return "Hi";
        }
    }
}
