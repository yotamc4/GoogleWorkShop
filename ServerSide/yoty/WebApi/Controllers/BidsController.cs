// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using YOTY.Service.Core.Services.Scheduling;
    using YOTY.Service.Core.Managers.Bids;
    using YOTY.Service.Core.Managers.Notifications;
    using YOTY.Service.Utils;
    using YOTY.Service.WebApi.Middlewares.Auth;
    using YOTY.Service.WebApi.PublicDataSchemas;

    // The controller has designed by the API best-practises doc here:https://hackernoon.com/restful-api-designing-guidelines-the-best-practices-60e1d954e7c9
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BidsController: YotyController
    {
        private IBidsManager bidsManager;
        private INotificationsManager notificationsManager;
        private IAuthorizationService authorizationService;

        public BidsController(IBidsManager bidsManager, INotificationsManager notificationsManager, IAuthorizationService authorizationService)
        {
            this.bidsManager = bidsManager;
            this.notificationsManager = notificationsManager;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<BidsDTO>> GetBids([FromQuery] BidsQueryOptions bidsQueryOptions)
        {
            Response<BidsDTO> response = await this.bidsManager.GetBids(bidsQueryOptions).ConfigureAwait(false);
            if (response.IsOperationSucceeded )
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, response.SuccessOrFailureMessage);
        }


        [HttpPost]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult> PostNewBid(NewBidRequest bid)
        {
            bid.OwnerId = GetRequestUserId();
            Response response = await this.bidsManager.CreateNewBid(bid).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status500InternalServerError, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}")]
        [AllowAnonymous]
        public async Task<ActionResult<BidDTO>> GetBid(string bidId, [FromQuery] string role)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = GetRequestUserId();
            }
            else if(!role?.Equals("anonymous", System.StringComparison.OrdinalIgnoreCase) ?? false)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            Response<BidDTO> response = await this.bidsManager.GetBid(bidId, userId, role).ConfigureAwait(false);
            if (response.IsOperationSucceeded )
            {
                return response.DTOObject;
            }
            // at the moment - we should change the boolean to enum such that we could differ between bad user input ( 400 ) to not found (404)  to unxepected failures ( 500) 
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);            
        }

        [HttpGet]
        [Route("{bidId}/buyers")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<ActionResult<List<SupplierProposalDTO>>> GetBidSuppliersProposals(string bidId)
        {
            Response<List<SupplierProposalDTO>> response = await bidsManager.GetBidSuppliersProposals(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/chosenProposal")]
        [AllowAnonymous]
        public async Task<ActionResult<SupplierProposalDTO>> GetBidChosenProposal(string bidId)
        {
            Response<SupplierProposalDTO> response = await bidsManager.GetBidChosenProposal(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/orderDetails")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult<List<OrderDetailsDTO>>> GetBidOrderDetails(string bidId)
        {
            var authResult = await this.authorizationService.AuthorizeAsync(User, bidId, PolicyNames.ChosenSupplierPolicy).ConfigureAwait(false);
            string userId = GetRequestUserId();
            if (!authResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }
            Response<List<OrderDetailsDTO>> response = await bidsManager.GetPaidCustomersFullOrderDetails(bidId, userId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/participants")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ParticipancyDTO>>> GetBidParticipations(string bidId)
        {
            Response<List<ParticipancyDTO>> response = await bidsManager.GetBidParticipations(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        [Route("{bidId}/participantsFullDetails")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult<List<ParticipancyFullDetailsDTO>>> GetBidParticipationsFullDetails(string bidId)
        {
            string requestUserId = GetRequestUserId();

            var authResult = await this.authorizationService.AuthorizeAsync(User, requestUserId, PolicyNames.ChosenSupplierPolicy).ConfigureAwait(false);
            if (!authResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }
            Response<List<ParticipancyFullDetailsDTO>> response = await bidsManager.GetBidParticipationsFullDetails(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/buyers")]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult> AddBuyer(string bidId, BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            bidBuyerJoinRequest.BuyerId = GetRequestUserId();
            if (bidBuyerJoinRequest.BidId == null)
            {
                bidBuyerJoinRequest.BidId = bidId;
            }
            else if (bidBuyerJoinRequest.BidId != bidId)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"bidId:{bidId} mentioned in request path dosen't match bidId:{bidBuyerJoinRequest.BidId} in request Body");
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
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult> AddSupplierProposal(string bidId, SupplierProposalRequest supplierProposalRequest)
        {
            supplierProposalRequest.SupplierId = GetRequestUserId();
            if (supplierProposalRequest.BidId == null)
            {
                supplierProposalRequest.BidId = bidId;
            }
            else if (supplierProposalRequest.BidId != bidId)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }

            Response response = await this.bidsManager.AddSupplierProposal(supplierProposalRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }

        // consider to movee it to buyer controller
        [HttpPut]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {
            AuthorizationResult authorizationResult = await this.authorizationService.AuthorizeAsync(User, editBidRequest.BidId, PolicyNames.BidOwnerPolicy).ConfigureAwait(false);
            if (!authorizationResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);

            }

            Response<BidDTO> response = await this.bidsManager.EditBid(editBidRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return response.DTOObject;
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }

        // consider to move it to buyer controller
        [HttpDelete]
        [Route("{bidId}")]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult> DeleteBid(string bidId)
        {
            AuthorizationResult authorizationResult = await this.authorizationService.AuthorizeAsync(User, bidId, PolicyNames.BidOwnerPolicy).ConfigureAwait(false);
            if (!authorizationResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            Response response = await this.bidsManager.DeleteBid(bidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status304NotModified, response.SuccessOrFailureMessage);
        }

        [HttpDelete]
        [Route("{bidId}/buyers")]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult> DeleteBuyer(string bidId) // buyer id should be removed from the controller
        {
            string buyerId = GetRequestUserId();
            Response response = await this.bidsManager.DeleteBuyer(bidId, buyerId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }


        [HttpDelete]
        [Route("{bidId}/proposals/{supplierId}")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult> DeleteSupplierProposal(string bidId)
        {
            string supplierId = GetRequestUserId();
            Response response = await this.bidsManager.DeleteSupplierProposal(bidId, supplierId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);

            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/vote")]
        [Authorize(Policy = PolicyNames.BuyerPolicy)]
        public async Task<ActionResult> VoteForSupplier(VotingRequest votingRequest)
        {
            if (!votingRequest.BidId.IsValidId() ||  !votingRequest.VotedSupplierId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"one of the following: bidId: {votingRequest.BidId}, supplierId: {votingRequest.VotedSupplierId} are not legal id");
            }

            votingRequest.BuyerId = GetRequestUserId();
            Response response = await this.bidsManager.VoteForSupplier(votingRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/markPayment")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult> MarkPayment(MarkPaidRequest markPaidRequest)
        {
            if (!markPaidRequest.BidId.IsValidId() || !markPaidRequest.BuyerId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"one of the following: bidId: {markPaidRequest.BidId}, buyerId: {markPaidRequest.BuyerId}");
            }
            markPaidRequest.MarkingUserId = GetRequestUserId();

            var authResult = await this.authorizationService.AuthorizeAsync(User, markPaidRequest.BidId, PolicyNames.ChosenSupplierPolicy).ConfigureAwait(false);
            if (!authResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            Response response = await this.bidsManager.MarkPaid(markPaidRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }

            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, response.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/cancel")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult> CancelBid(CancellationRequest cancellationRequest)
        {
            if (!cancellationRequest.BidId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Given bidId: {cancellationRequest.BidId}, is not legal id");
            }

            var authResult = await this.authorizationService.AuthorizeAsync(User, cancellationRequest.BidId, PolicyNames.ChosenSupplierPolicy).ConfigureAwait(false);
            if (!authResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }
            cancellationRequest.SupplierId = this.GetRequestUserId();

            Response response = await this.bidsManager.CancelBid(cancellationRequest.BidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            Response notificationResponse = await this.notificationsManager.NotifyBidAllMissingPaymentsCancellation(cancellationRequest.BidId).ConfigureAwait(false);
            if (notificationResponse.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, notificationResponse.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, notificationResponse.SuccessOrFailureMessage);
        }

        [HttpPost]
        [Route("{bidId}/complete")]
        [Authorize(Policy = PolicyNames.SupplierPolicy)]
        public async Task<ActionResult> CompleteBid(CompletionRequest completionRequest)
        {
            if (!completionRequest.BidId.IsValidId())
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Given bidId: {completionRequest.BidId} is not legal id");
            }

            var authResult = await this.authorizationService.AuthorizeAsync(User, completionRequest.BidId, PolicyNames.ChosenSupplierPolicy).ConfigureAwait(false);
            if (!authResult.Succeeded)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }
            completionRequest.SupplierId = this.GetRequestUserId();

            Response response = await this.bidsManager.CompleteBid(completionRequest.BidId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }
            Response notificationResponse = await this.notificationsManager.NotifyBidAllCompletion(completionRequest.BidId).ConfigureAwait(false);
            if (notificationResponse.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, notificationResponse.SuccessOrFailureMessage);
            }
            return this.StatusCode(StatusCodes.Status405MethodNotAllowed, notificationResponse.SuccessOrFailureMessage);
        }


        [HttpGet]
        [Route("Ping")]
        public async Task<string> Ping()
        {
            return "Hi";
        }
    }
}
