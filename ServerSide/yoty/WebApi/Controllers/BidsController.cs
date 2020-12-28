// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Notifications;
    using YOTY.Service.Data;
    using YOTY.Service.Utils;
    using YOTY.Service.WebApi.PublicDataSchemas;
    using YOTY.Service.WebApi.PublicDataSchemas.ClientRequest;

    // The controller has designed by the API best-practises doc here:https://hackernoon.com/restful-api-designing-guidelines-the-best-practices-60e1d954e7c9
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BidsController: ControllerBase
    {
        private IBidsManager bidsManager;
        private INotificationsManager notificationsManager;

        public BidsController(IBidsManager bidsManager, INotificationsManager notificationsManager)
        {
            this.bidsManager = bidsManager;
            this.notificationsManager = notificationsManager;
        }

        [HttpGet]
        public async Task<ActionResult<BidsDTO>> GetBids([FromQuery] BidsQueryOptions bidsQueryOptions)
        {
            Response<BidsDTO> response = await this.bidsManager.GetBids(bidsQueryOptions).ConfigureAwait(false);
            if (response.IsOperationSucceeded )
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }


        [HttpPost]
        public async Task<ActionResult> PostNewBid(NewBidRequest bid)
        {
            Response response = await this.bidsManager.CreateNewBid(bid).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}")]
        public async Task<ActionResult<BidDTO>> GetBid(string bidId)
        {
            RecurringJob.AddOrUpdate(() => NotificationsManager.Ping(bidId), Cron.Hourly, TimeZoneInfo.Local);
            Console.WriteLine("InGetBid");
            Response<BidDTO> response = await this.bidsManager.GetBid(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded )
            {
                
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);            
        }

        [HttpGet]
        [Route("{bidId}/buyers")]
        public async Task<ActionResult<List<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            Response<List<BuyerDTO>> response = await this.bidsManager.GetBidBuyers(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/proposals")]
        public async Task<ActionResult<List<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            Response<List<SupplierProposalDTO>> response = await bidsManager.GetBidSuplliersProposals(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/buyers")]
        public async Task<ActionResult> AddBuyer(string bidId, BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            if (bidBuyerJoinRequest.BidId == null)
            {
                bidBuyerJoinRequest.BidId = bidId;
            }

            Response response = await this.bidsManager.AddBuyer(bidBuyerJoinRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/proposals")]
        public async Task<ActionResult> AddSupplierProposal(string bidId, SupplierProposalRequest supplierProposalRequest)
        {
            if (supplierProposalRequest.BidId == null)
            {
                supplierProposalRequest.BidId = bidId;
            }
            Response response = await this.bidsManager.AddSupplierProposal(supplierProposalRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }

        [HttpPut]
        public async Task<ActionResult<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {

            Response<BidDTO> response = await this.bidsManager.EditBid(editBidRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }


        [HttpDelete]
        [Route("{bidId}/buyers/{buyerId}")]
        public async Task<ActionResult> DeleteBuyer(string bidId, string buyerId)
        {

            Response response = await this.bidsManager.DeleteBuyer(bidId, buyerId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }


        [HttpDelete]
        [Route("{bidId}/proposals/{supplierId}")]
        public async Task<ActionResult> DeleteSupplierProposal(string bidId, string supplierId)
        {

            Response response = await this.bidsManager.DeleteSupplierProposal(bidId, supplierId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/vote")]
        public async Task<ActionResult> VoteForSupplier(VotingRequest votingRequest)
        {
            if (!votingRequest.BidId.IsValidId() || !votingRequest.BuyerId.IsValidId() || !votingRequest.VotedSupplierId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"one of the following: bidId: {votingRequest.BidId}, buyerId: {votingRequest.BuyerId} supplierId: {votingRequest.VotedSupplierId} are not legal id");
            }

            Response response = await this.bidsManager.VoteForSupplier(votingRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/cancel")]
        public async Task<ActionResult> CancelBid(CancellationRequest cancellationRequest)
        {
            if (!cancellationRequest.BidId.IsValidId() || !cancellationRequest.SupplierId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"one of the following: bidId: {cancellationRequest.BidId}, supplierId: {cancellationRequest.SupplierId} are not legal id");
            }

            Response response = await this.bidsManager.CancelBid(cancellationRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            Response notificationResponse = await this.notificationsManager.NotifyBidParticipantsSupplierCancellation(cancellationRequest.BidId).ConfigureAwait(false);
            if (notificationResponse.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, notificationResponse.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/complete")]
        public async Task<ActionResult> CompleteBid(CompletionRequest completionRequest)
        {
            if (!completionRequest.BidId.IsValidId() || !completionRequest.SupplierId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"one of the following: bidId: {completionRequest.BidId}, supplierId: {completionRequest.SupplierId} are not legal id");
            }

            Response response = await this.bidsManager.CompleteBid(completionRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            Response notificationResponse = await this.notificationsManager.NotifyBidAllCompletion(completionRequest.BidId).ConfigureAwait(false);
            if (notificationResponse.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, notificationResponse.SuccessOrFailureMessage);
        }


        [HttpGet]
        [Route("Ping")]
        public async Task<string> Ping()
        {
            return "Hi";
        }

        // TODO run this every 24 hours or by schedule
        private async Task<Response> TryUpdateBidPhaseAndNotify(string bidId)
        {
            var updatePhaseResponse = await this.bidsManager.TryUpdatePhase(bidId).ConfigureAwait(false);
            
            Response notificationResponse;
            Response updateProposalsResponse;
            if (!updatePhaseResponse.IsOperationSucceeded)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = updatePhaseResponse.SuccessOrFailureMessage };
            }
            switch (updatePhaseResponse.DTOObject)
            {
                case BidPhase.Vote:
                    notificationResponse = await this.notificationsManager.NotifyBidTimeToVote(bidId).ConfigureAwait(false);
                    updateProposalsResponse = await this.bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.Payment:
                    notificationResponse = await this.notificationsManager.NotifyBidTimeToPay(bidId).ConfigureAwait(false);
                    updateProposalsResponse = await this.bidsManager.UpdateBidProposalsToRelevant(bidId).ConfigureAwait(false);
                    break;
                case BidPhase.CancelledSupplierNotFound:
                    notificationResponse = await this.notificationsManager.NotifyBidParticipantsSupplierNotFoundCancellation(bidId).ConfigureAwait(false);
                    break;
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = "blabla" };
        }
    }
}
