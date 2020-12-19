// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Bids
{
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
    using F23.StringSimilarity;

    public class BidsManager : IBidsManager
    {
        private const string BidNotFoundFailString = "Failed, Bid not found";
        private const string ProposalNotFoundFailString = "Failed, Proposal not found";
        private const string ParticipantNotFoundFailString = "Failed, Participant not found";

        private readonly YotyContext _context;
        private readonly IMapper _mapper;

        public BidsManager(IMapper mapper, YotyContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidBuyerJoinRequest.bidId).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
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
            try
            {
                _context.Bids.Update(bid);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response<BuyerDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            //TODO dropping the dto obj from the response will save us the second db access 
            BuyerEntity buyer_ent = await _context.Buyers.FindAsync(bidBuyerJoinRequest.buyerId).ConfigureAwait(false);
            BuyerDTO buyer_dto = _mapper.Map<BuyerDTO>(buyer_ent);
            return new Response<BuyerDTO>() { DTOObject = buyer_dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposalRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == supplierProposalRequest.BidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SupplierProposalEntity new_proposal_ent = _mapper.Map<SupplierProposalEntity>(supplierProposalRequest);
            bid.CurrentProposals.Add(new_proposal_ent);
            bid.PotenialSuplliersCounter += 1;

            try
            {
                _context.Bids.Update(bid);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            //TODO dropping the dto obj from the response will save us the second db access 
            SupplierEntity supplier_ent = await _context.Suppliers.FindAsync(supplierProposalRequest.SupplierId).ConfigureAwait(false);
            SupplierProposalDTO supplier_dto = _mapper.Map<SupplierProposalDTO>(supplier_ent);
            return new Response<SupplierProposalDTO>() { DTOObject = supplier_dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> CreateNewBid(NewBidRequst bidRequest)
        {
            BidEntity bidEntity = _mapper.Map<BidEntity>(bidRequest);
            //TODO is this the time we want? (or global).
            bidEntity.CreationDate = DateTime.Now;
            bidEntity.Id = Guid.NewGuid().ToString();
            bidEntity.UnitsCounter = 0;
            bidEntity.PotenialSuplliersCounter = 0;
            bidEntity.Product.Id = Guid.NewGuid().ToString();
            bidEntity.CurrentParticipancies = new List<ParticipancyEntity>();
            bidEntity.CurrentProposals = new List<SupplierProposalEntity>();
            ProductEntity existingProduct = FindExistingProduct(bidRequest.Product);
            if (existingProduct != null)
            {
                bidEntity.Product = existingProduct;
            }

            _context.Bids.Add(bidEntity);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //TODO log exception and return proper error message instead
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            BidDTO dto = _mapper.Map<BidDTO>(bidEntity);
            return new Response<BidDTO>() { DTOObject = dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        public async Task<Response> DeleteBid(string bidId)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentParticipancies).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            _context.Bids.Remove(bid);
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

        public async Task<Response> DeleteBuyer(string bidId, string buyerId)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            ParticipancyEntity participancy = bid.CurrentParticipancies.Find(p => p.BuyerId == buyerId);
            if (participancy == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ParticipantNotFoundFailString };
            }
            bid.UnitsCounter -= participancy.NumOfUnits;
            bid.CurrentParticipancies.Remove(participancy);
            _context.Set<ParticipancyEntity>().Remove(participancy);
            try
            {
                _context.Bids.Update(bid);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> DeleteSupplierProposal(string bidId, string supplierId)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SupplierProposalEntity proposal = bid.CurrentProposals.Find(p => p.SupplierId == supplierId);
            if (proposal == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ProposalNotFoundFailString };
            }
            bid.CurrentProposals.Remove(proposal);
            bid.PotenialSuplliersCounter--;
            _context.Set<SupplierProposalEntity>().Remove(proposal);
            try
            {
                _context.Bids.Update(bid);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> EditBid(EditBidRequest editBidRequest)
        {
            BidEntity bid = await _context.Bids.FindAsync(editBidRequest.BidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.Product.Name = editBidRequest.NewName;
            bid.Product.Image = editBidRequest.NewProductImage;
            bid.Product.Description = editBidRequest.NewDescription;
            bid.Category = editBidRequest.NewCategory;
            bid.SubCategory = editBidRequest.NewSubCategory;

            try
            {
                _context.Bids.Update(bid);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            BidDTO bidDTO = _mapper.Map<BidDTO>(bid);
            return new Response<BidDTO>() { DTOObject = bidDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> GetBid(string bidId)
        {
            BidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            BidDTO bidDTO = _mapper.Map<BidDTO>(bid);
            return new Response<BidDTO>() { DTOObject = bidDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        public async Task<Response<List<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            var bid_ent = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response<List<BuyerDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };

            }

            List<BuyerDTO> buyers = new List<BuyerDTO>();
            bid_ent.CurrentParticipancies.ForEach(p => buyers.Add(_mapper.Map<BuyerDTO>(p.Buyer)));
            return new Response<List<BuyerDTO>>() { DTOObject = buyers, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public Task<Response<List<BidDTO>>> GetBids(BidsQueryOptions bidsFilters)
        {
            //TODO
            throw new NotImplementedException();
        }

        public async Task<Response<List<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            try
            {
                List<SupplierProposalDTO> proposals = await _context.Set<SupplierProposalEntity>().Where(p => p.BidId == bidId).Select(p => _mapper.Map<SupplierProposalDTO>(p)).ToListAsync().ConfigureAwait(false);
                return new Response<List<SupplierProposalDTO>>() { DTOObject = proposals, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

            }
            catch
            {
                return new Response<List<SupplierProposalDTO>>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
            }
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }

        // enhancements (will not be private)
        private async Task<bool> IsBuyerParticipating(string buyerId, string bidId)
        {
            var p = await _context.Set<ParticipancyEntity>().FindAsync(bidId, buyerId).ConfigureAwait(false);
            return p != null;
        }

        private async Task<Response> VoteForProposal(string votingBuyerId, string bidId, string chosenSupplierId)
        {
            BidEntity bid_ent = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentProposals).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "bid not found" };
            }
            ParticipancyEntity participancy = bid_ent.CurrentParticipancies.Where(p => p.BuyerId == votingBuyerId).FirstOrDefault();
            if (participancy == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "participancy not found" };
            }
            SupplierProposalEntity proposal = bid_ent.CurrentProposals.Where(p => p.SupplierId == chosenSupplierId).FirstOrDefault();
            if (proposal == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "proposal not found" };
            }
            if (participancy.HasVoted)
            {
                // TODO enable change after vote
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "participant has voted" };
            }

            proposal.VotesCounter += 1;
            participancy.HasVoted = true;
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private SupplierProposalEntity GetProposalWithMaxVotes(string bidId)
        {
            SupplierProposalEntity proposalEntity = _context.Set<SupplierProposalEntity>().Where(p => p.BidId == bidId).Include(p => p.Supplier).Aggregate(
                (currWinner, x) => (currWinner == null || x.VotesCounter > currWinner.VotesCounter ? x : currWinner));
            // TODO set the chosen proposal?
            return proposalEntity;
        }

        private SupplierEntity GetSupplierWithMaxVotes(string bidId)
        {
            // TODO set the chosen supplier?
            return this.GetProposalWithMaxVotes(bidId).Supplier;
        }

        private async Task<Response> MarkPaid(string bidId, string buyerId, string markingUserId, bool hasPaid = true)
        {
            // TODO add validation that the marking user is the chosen supplier
            DbSet<ParticipancyEntity> participancies_db = _context.Set<ParticipancyEntity>();
            var p = await participancies_db.FindAsync(bidId, buyerId).ConfigureAwait(false);
            if (p == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "participation not found" };
            }
            p.HasPaid = hasPaid;
            try
            {
                participancies_db.Update(p);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private async Task<Response<List<SupplierProposalDTO>>> GetBidParticipations(string bidId)
        {
            try
            {
                List<ParticipancyEntity> participancies = await _context.Set<ParticipancyEntity>().Where(p => p.BidId == bidId).Select(p => _mapper.Map<ParticipancyEntity>(p)).ToListAsync().ConfigureAwait(false);
                // todo add participation DTO ------------------------------ here ------------------------------------------------------------------------and here ^
                return new Response<List<SupplierProposalDTO>>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }
            catch
            {
                return new Response<List<SupplierProposalDTO>>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
            }
        }

        private ProductEntity FindExistingProduct(ProductRequest productRequest)
        {
            var Products = _context.Set<ProductEntity>();
            var jw = new JaroWinkler();
            Func<string, string> normalize = s => s.ToLower().Replace(" ", string.Empty);
            string normalizedName = normalize(productRequest.Name);
            ProductEntity result = Products.Where(product => jw.Similarity(normalize(product.Name), normalizedName) > 0.9).FirstOrDefault();
            return result;
        }
    }
}
