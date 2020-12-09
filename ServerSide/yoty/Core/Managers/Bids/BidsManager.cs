// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using YOTY.Service.Data;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class BidsManager : IBidsManager
    {
        private const string BidNotFoundFailString = "Failed, Bid not found";
        private static YotyContext _context = new YotyContext();

        public async Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(bidBuyerJoinRequest.bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<BuyerDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.CurrentParticipancies.Add(new ParticipancyEntity {
                BidId = bidBuyerJoinRequest.bidId,
                BuyerId = bidBuyerJoinRequest.buyerId,
                NumOfUnits = bidBuyerJoinRequest.Items
            });
            bid.UnitsCounter += bidBuyerJoinRequest.Items;
            using (var new_context = new YotyContext())
            {
                try
                {
                    new_context.Bids.Update(bid);
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response<BuyerDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            //TODO dropping the dto obj from the response will save us the second db access 
            var buyer_ent = await _context.Buyers.FindAsync(bidBuyerJoinRequest.buyerId).ConfigureAwait(false);
            //TODO map ent to dto
            return new Response<BuyerDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposal)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(supplierProposal.BidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.CurrentOffers.Add(new SellerOfferEntity {
                BidId = bid.Id,
                SellerId = supplierProposal.SupplierId,
                PublishedTime = supplierProposal.PublishedTime,
                MinimumUnits = supplierProposal.MinimumUnits,
                OfferedPrice = supplierProposal.ProposedPrice,
                OfferDescription = supplierProposal.Description,
            });
            bid.PotenialSuplliersCounter += 1;
            using (var new_context = new YotyContext())
            {
                try
                {
                    new_context.Bids.Update(bid);
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            //TODO dropping the dto obj from the response will save us the second db access 
            var supplier_ent = await _context.Sellers.FindAsync(supplierProposal.SupplierId).ConfigureAwait(false);
            //TODO map ent to dto
            return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> CreateNewBid(NewBidRequst productBidRequest)
        {
            ProductBidEntity newBid = new ProductBidEntity() {
                Name = productBidRequest.Name,
                OwnerId = productBidRequest.OwnerId,
                Category = productBidRequest.Category,
                SubCategory = productBidRequest.SubCategory,
                MaxPrice = productBidRequest.MaxPrice,
                ExpirationDate = productBidRequest.ExpirationDate,
                Description = productBidRequest.Description,
                ProductImage = productBidRequest.ProductImage,
                //TODO is this the time we want? (or global).
                CreationDate = DateTime.Now,
                CurrentOffers = new List<SellerOfferEntity>(),
                CurrentParticipancies = new List<ParticipancyEntity>(),
                UnitsCounter = 0,
                Id = this.GenerateBidId().ToString(),
                PotenialSuplliersCounter = 0,
            };

            using (var new_context = new YotyContext())
            {
                new_context.Bids.Add(newBid);
                try
                {
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            BidDTO dto = null;
            //TODO map ent to dto
            return new Response<BidDTO>() { DTOObject = dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        public async Task<Response> DeleteBid(string bidId)
        {
            var bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            using (var new_context = new YotyContext())
            {
                new_context.Bids.Remove(bid);
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

        public async Task<Response> DeleteBuyer(string bidId, string buyerId)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            ParticipancyEntity participancy = bid.CurrentParticipancies.Find(p => p.BuyerId == buyerId);
            bid.UnitsCounter -= participancy.NumOfUnits;
            bid.CurrentParticipancies.Remove(participancy);
            using (var new_context = new YotyContext())
            {
                try
                {
                    new_context.Bids.Update(bid);
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> DeleteSupplierProposal(string bidId, string supplierId)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SellerOfferEntity proposal = bid.CurrentOffers.Find(p => p.SellerId == supplierId);
            bid.CurrentOffers.Remove(proposal);
            bid.PotenialSuplliersCounter--;
            using (var new_context = new YotyContext())
            {
                try
                {
                    new_context.Bids.Update(bid);
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(editBidRequest.BidId).ConfigureAwait(false);
            if(bid == null)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.Name = editBidRequest.NewName;
            bid.ProductImage = editBidRequest.NewProductImage;
            bid.Description = editBidRequest.NewDescription;
            bid.Category = editBidRequest.NewCategory;
            bid.SubCategory = editBidRequest.NewSubCategory;
            using (var new_context = new YotyContext())
            {
                try
                {
                    new_context.Bids.Update(bid);
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            //TODO map ent to dto
            return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> GetBid(string bidId)
        {
            ProductBidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            //TODO map ent to dto
            return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        public async Task<Response<IList<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            var bid_ent = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response<IList<BuyerDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };

            }

            IList<BuyerDTO> buyers = new List<BuyerDTO>();
            // map p.Buyer to dto
            bid_ent.CurrentParticipancies.ForEach(p => buyers.Add(new BuyerDTO() { }));
            return new Response<IList<BuyerDTO>>() { DTOObject = buyers, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }

        public Task<Response<IList<BidDTO>>> GetBids(BidsQueryOptions bidsFilters)
        {
            //TODO
            throw new NotImplementedException();
        }

        public async Task<Response<IList<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            var bid_ent = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response<IList<SupplierProposalDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            IList<SupplierProposalDTO> proposals = new List<SupplierProposalDTO>();
            foreach(SellerOfferEntity proposal_ent in bid_ent.CurrentOffers)
            {
                // TODO map ent to dto
                proposals.Add(new SupplierProposalDTO());
            }

            return new Response<IList<SupplierProposalDTO>>() { DTOObject = proposals, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private Guid GenerateBidId()
        {
            Guid id = Guid.NewGuid();
            while (_context.Bids.Find(id) != null)
            {
                id = Guid.NewGuid();
            }
            return id;
        }
        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
    }
}
