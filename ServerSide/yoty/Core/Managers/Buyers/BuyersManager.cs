using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YOTY.Service.Data;
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

        public Task<Response<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteBuyer(string buyerId)
        {
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).Include(b => b.BuyerAccountDetails).Include(b => b.FacebookAccount).FirstOrDefaultAsync().ConfigureAwait(false);
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

        public async Task<Response<List<BidDTO>>> GetBidsCreatedByBuyer(string buyerId)
        {
            var bids = _context.Bids.Where(b => b.OwnerId == buyerId).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<List<BidDTO>>() { DTOObject = bids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
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

        public async Task<Response<List<BidDTO>>> GetBuyerLiveBids(string buyerId)
        {
            DateTime current_time = DateTime.Now;
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).ThenInclude(p => p.Bid).FirstOrDefaultAsync().ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<List<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }
            List<BidDTO> liveBids = buyer.CurrentParticipancies.Select(p => p.Bid).Where(b => b.ExpirationDate > current_time).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<List<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<List<BidDTO>>> GetBuyerOldBids(string buyerId)
        {
            DateTime current_time = DateTime.Now;
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).ThenInclude(p=>p.Bid).FirstOrDefaultAsync().ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<List<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BuyerNotFoundFailString };
            }
            List<BidDTO> liveBids = buyer.CurrentParticipancies.Select(p => p.Bid).Where(b => b.ExpirationDate <= current_time).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<List<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public Task<Response<BuyerDTO>> ModifyBuyerDetails()
        {
            throw new NotImplementedException();
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
    }
}
