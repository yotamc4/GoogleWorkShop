using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YOTY.Service.Data;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Buyers
{
    public class BuyersManager : IBuyersManager
    {
        private const string BuyerNotFoundFailString = "Failed, Buyer not found";
        private readonly YotyContext _context;
        private readonly IMapper _mapper;

        public BuyersManager(IMapper mapper, YotyContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Response> CreateBuyer(NewUserRequest request)
        {
            BuyerEntity newBuyer = _mapper.Map<BuyerEntity>(request);
            try
            {
                _context.Buyers.Add(newBuyer);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> DeleteBuyer(string buyerId)
        {
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
            //var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).Include(b => b.BuyerAccountDetails).Include(b => b.FacebookAccount).FirstOrDefaultAsync().ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }
            _context.Buyers.Remove(buyer);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }
        public async Task<Response<BidsDTO>> GetBuyerBids(string buyerId, BidsTime timeFilter)
        {
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).ThenInclude(p => p.Bid).FirstOrDefaultAsync().ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<BidsDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }

            List<BidDTO> liveBids = buyer.CurrentParticipancies.Select(p => p.Bid).Where(bid=> FilterBuyerBids(bid, timeFilter)).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<BidsDTO>() { DTOObject = BidsDTO.CreateDefaultBidsPage(liveBids), IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }


        public async Task<Response<BidsDTO>> GetBidsCreatedByBuyer(string buyerId, BidsTime timeFilter)
        {
            //validate existence
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<BidsDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }

            var ownedBids = _context.Bids.Where(b => b.OwnerId == buyerId).Where(bid => FilterBuyerBids(bid, timeFilter)).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<BidsDTO>() { DTOObject = BidsDTO.CreateDefaultBidsPage(ownedBids), IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private static bool FilterBuyerBids(BidEntity buyerBid, BidsTime timeFilter)
        {
            HashSet<BidPhase> livePhases = new HashSet<BidPhase>() { BidPhase.Join, BidPhase.Vote, BidPhase.Payment};
            switch (timeFilter)
            {
                case BidsTime.Old:
                    return !livePhases.Contains(buyerBid.Phase);
                case BidsTime.Live:
                    return livePhases.Contains(buyerBid.Phase);
                default: //null
                    return true;
            }
        }

        public async Task<Response<BuyerDTO>> GetBuyer(string buyerId)
        {
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<BuyerDTO>() {DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }
            BuyerDTO buyerDTO = _mapper.Map<BuyerDTO>(buyer);
            return new Response<BuyerDTO>() { DTOObject = buyerDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> ModifyBuyerDetails(ModifyBuyerDetailsRequest request)
        {
            var buyer = await _context.Buyers.FindAsync(request.BuyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }
            if (request.Address != null)
            {
                buyer.Address = request.Address;
            }
            if (request.PostalCode != null)
            {
                buyer.PostalCode = request.PostalCode;
            }
            if (request.PhoneNumber != null)
            {
                buyer.PhoneNumber = request.PhoneNumber;
            }
            if (request.Email != null)
            {
                buyer.Email = request.Email;
            }
            if (request.ProfilePicture != null)
            {
                buyer.ProfilePicture = request.ProfilePicture;
            }
            try
            {
                _context.Buyers.Update(buyer);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
    }
}
