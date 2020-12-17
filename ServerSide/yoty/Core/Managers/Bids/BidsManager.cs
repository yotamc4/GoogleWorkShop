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
            bidEntity.Product.Id = Guid.NewGuid().ToString();
            bidEntity.CurrentParticipancies = new List<ParticipancyEntity>();
            bidEntity.CurrentProposals = new List<SupplierProposalEntity>();

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
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
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
            if(bid == null)
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
            BidEntity bid_ent = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response<List<SupplierProposalDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            List<SupplierProposalDTO> proposals = new List<SupplierProposalDTO>();
            foreach(SupplierProposalEntity proposal_ent in bid_ent.CurrentProposals)
            {
                proposals.Add(_mapper.Map<SupplierProposalDTO>(proposal_ent));
            }

            return new Response<List<SupplierProposalDTO>>() { DTOObject = proposals, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
    }
}
