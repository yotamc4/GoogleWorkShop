using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YOTY.Service.Data;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Buyers
{
    public class BuyersManager : IBuyersManager
    {
        private static YotyContext _context = new YotyContext();

        public Task<Response<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteBuyer(string buyerId)
        {
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "buyer not found" };
            }
            using (var new_context = new YotyContext())
            {
                new_context.Buyers.Remove(buyer);
                try
                {
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<IList<BidDTO>>> GetBidsCreatedByBuyer(string buyerId)
        {
            //TODO add mapper from bid_ent to dto      -------------------->        here
            var bids = _context.Bids.Where(b => b.OwnerId == buyerId).Select(bid => new BidDTO()).ToList();
            return new Response<IList<BidDTO>>() { DTOObject = bids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BuyerDTO>> GetBuyer(string buyerId)
        {
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<BuyerDTO>() {DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = "buyer not found" };
            }
            //TODO add mapper from buyer_ent to buyer_dto  
            return new Response<BuyerDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<IList<BidDTO>>> GetBuyerLiveBids(string buyerId)
        {
            DateTime current_time = DateTime.Now;
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<IList<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = "buyer not found" };
            }
            //TODO add mapper from bid_ent to dto       ------------------------------------------------------------->                        here
            IList<BidDTO> liveBids = buyer.CurrentParticipancies.Select(p => p.Bid).Where(b => b.ExpirationDate > current_time).Select(bid => new BidDTO()).ToList();
            return new Response<IList<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<IList<BidDTO>>> GetBuyerOldBids(string buyerId)
        {
            DateTime current_time = DateTime.Now;
            var buyer = await _context.Buyers.FindAsync(buyerId).ConfigureAwait(false);
            if (buyer == null)
            {
                return new Response<IList<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = "buyer not found" };
            }
            //TODO add mapper from bid_ent to dto       ------------------------------------------------------------->                        here
            IList<BidDTO> liveBids = buyer.CurrentParticipancies.Select(p => p.Bid).Where(b => b.ExpirationDate <= current_time).Select(bid => new BidDTO()).ToList();
            return new Response<IList<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
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
