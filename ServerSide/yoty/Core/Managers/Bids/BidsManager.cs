﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

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
    using YOTY.Service.Utils;
    using YOTY.Service.WebApi.PublicDataSchemas;
    using F23.StringSimilarity;

    public class BidsManager : IBidsManager
    {
        private const string BidNotFoundFailString = "Failed, Bid not found";
        private const string ProposalNotFoundFailString = "Failed, Proposal not found";
        private const string ParticipantNotFoundFailString = "Failed, Participant not found";

        private readonly YotyContext _context;
        private readonly IMapper _mapper;
        private int _pageDefaultSize = 9;
        private int _maxPageSize = 20;

        public BidsManager(IMapper mapper, YotyContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Response> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidBuyerJoinRequest.BidId).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.CurrentParticipancies.Add(new ParticipancyEntity {
                BidId = bidBuyerJoinRequest.BidId,
                BuyerId = bidBuyerJoinRequest.BuyerId,
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
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() {IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> AddSupplierProposal(SupplierProposalRequest supplierProposalRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == supplierProposalRequest.BidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            // TODO? validate supplier
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
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() {IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> CreateNewBid(NewBidRequest bidRequest)
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
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            BidDTO dto = _mapper.Map<BidDTO>(bidEntity);
            return new Response() {IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

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
            // TODO this edits product! if we use this must fix
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
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.Product).FirstOrDefaultAsync().ConfigureAwait(false);
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

        public async Task<Response<BidsDTO>> GetBids(BidsQueryOptions bidsFilters)
        {

            // firts category and sub category 
            // then 
            string bidsSearchString = bidsFilters.Search;
            if (bidsFilters.Category == null && bidsSearchString == null)
            {
                return await this.GetDefaultHomePageBids(bidsFilters.Page);
            }
            else if (!ValidateBidsFilters(bidsFilters, out string validationErrorString))
            {
                return new Response<BidsDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = validationErrorString };
            }
            else // TODO : extract to sub methods
            {
                IEnumerable<BidEntity> filteredBids = this.GetFilteredBids(bidsFilters);

                IEnumerable<BidEntity> sortedBids = this.GetSortBids(filteredBids, bidsFilters.SortOrder , bidsFilters.SortBy);

                // return page
                int pageSize = bidsFilters.Limit == 0 || bidsFilters.Limit > _maxPageSize ? _pageDefaultSize : bidsFilters.Limit;
  
                var bidsPage = sortedBids
                    .Skip(bidsFilters.Page * pageSize)
                    .Take(pageSize)
                    .Select (bidEntity => _mapper.Map<BidDTO>(bidEntity))
                    .ToList();

                BidsDTO bidsDTO = new BidsDTO {
                    PageNumber = bidsFilters.Page,
                    NumberOfPages = (int)Math.Ceiling((double)filteredBids.Count() / pageSize),
                    PageSize = pageSize,
                    BidsPage = bidsPage,
                };

                return new Response<BidsDTO>() { DTOObject = bidsDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

            }
        }

        private async Task<Response<BidsDTO>> GetDefaultHomePageBids (int page)
        {
            List<BidDTO> bidsTask = await _context.Bids
                .OrderByDescending(bid => bid.UnitsCounter)
                .OrderByDescending(bid => bid.PotenialSuplliersCounter)
                .OrderBy(bid => bid.ExpirationDate)
                .Skip(page * _pageDefaultSize)
                .Take(_pageDefaultSize)
                .Include(bidEntity => bidEntity.Product)
                .Select(bidEntitiy => _mapper.Map<BidDTO>(bidEntitiy))
                .ToListAsync().ConfigureAwait(false);

            
            int numberOfBids = await _context.Bids.CountAsync().ConfigureAwait(false);


            BidsDTO bidsDTO = new BidsDTO {
                PageNumber = page,
                NumberOfPages = (int)Math.Ceiling((double)numberOfBids / _pageDefaultSize),
                PageSize = _pageDefaultSize,
                BidsPage = bidsTask,
            };

            return new Response<BidsDTO>() { DTOObject = bidsDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private bool  ValidateBidsFilters(BidsQueryOptions bidsFilters, out string validationErorrString)
        {
            validationErorrString = null;
            string demandedCategory = bidsFilters.Category;
            string demandedSubCategory = bidsFilters.SubCategory; 
            if (demandedSubCategory != null && demandedCategory == null)
            {
                validationErorrString = $"Sub catergory has set to :{bidsFilters.SubCategory } while category is null";
                return false;
            }
            /*
            // should implement it smarter with creating categories table and with kind of index on it 
            if (!_context.Bids.Select(bid =>bid.Category).ToHashSet().Contains(demandedCategory))
            {
                validationErorrString = $"Catergory has set to :{demandedCategory } while category is not exist in DB";
                return false;
            }
            */
            // should implemnt also table of Category-SubCategory with many to one relationship to vaildate....

            return true;
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
        private static bool FilterByCategories(BidEntity bid, string category, string subCategory)
        {
            if(category == null)
            {
                return true;
            }
            else if (bid.Category.Equals(category) && subCategory == null)
            {
                return true;
            }
            else if (bid.SubCategory != null)
            {
                return  bid.Category.Equals(category) && bid.SubCategory.Equals(subCategory);
            }

            return false;
        }
        private static bool FilterByPrices(BidEntity bid, int maxPriceFilter, int minPriceFilter)
        {
            return
                (maxPriceFilter < Int32.MaxValue) ? bid.MaxPrice < maxPriceFilter : false
                ||
                minPriceFilter <= 0 || bid.MaxPrice > minPriceFilter;
        }

        private static bool FilterByQueryString(BidEntity bid, string queryString)
        {
            return queryString == null || bid.Product.Name.Contains(queryString) || bid.Product.Description.Contains(queryString);
        }

        private IEnumerable<BidEntity> GetFilteredBids(BidsQueryOptions bidsFilters)
        {
            return _context.Bids
                .Include(bid => bid.Product)
                .AsEnumerable()
                .Where(bid => FilterByCategories(bid, bidsFilters.Category, bidsFilters.SubCategory))
                .Where(bid => FilterByPrices(bid, bidsFilters.MaxPrice, bidsFilters.MinPrice))
                .Where(bid => FilterByQueryString(bid, bidsFilters.Search));                  
        }

        private IEnumerable<BidEntity> GetSortBids(IEnumerable<BidEntity> bids, BidsSortByOrder sortOrder, BidsSortByOptions sortByyParameter)
        {
            switch (sortByyParameter)
            {
                case BidsSortByOptions.CreationDate:
                    return bids.Sort(bid => bid.CreationDate, sortOrder);
                case BidsSortByOptions.ExpirationDate:
                    return bids.Sort(bid => bid.ExpirationDate, sortOrder);
                case BidsSortByOptions.SupplierProposals:
                    return bids.Sort(bid => bid.PotenialSuplliersCounter, sortOrder);
                case BidsSortByOptions.DemandedItems:
                    return bids.Sort(bid => bid.UnitsCounter, sortOrder);
                case BidsSortByOptions.Price:
                    return bids.Sort(bid => bid.MaxPrice, sortOrder);
                default:
                    return bids.Sort(bid => bid.MaxPrice, sortOrder);
            }
        }

        public async Task<Response> VoteForSupplier(VotingRequest votingRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == votingRequest.BidId).Include(b => b.CurrentProposals).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }

            ParticipancyEntity participancy = bid.CurrentParticipancies.Where(p => p.BuyerId == votingRequest.BuyerId).FirstOrDefault();
            if (participancy == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = $"Buyer {votingRequest.BuyerId} is not part of the bids buyers." };
            }
            else if (participancy.HasVoted)
            {
                // TODO enable change after vote
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = $"Buyer {votingRequest.BuyerId} has already voted" };
            }

            SupplierProposalEntity proposal = bid.CurrentProposals.Where(p => p.SupplierId == votingRequest.VotedSupplierId).FirstOrDefault();
            if (proposal == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = $"no proposal for supplier: {votingRequest.VotedSupplierId} has found for the bid." };
            }

            participancy.HasVoted = true;
            proposal.Votes += 1;

            try
            {
                _context.Set<ParticipancyEntity>().Update(participancy);
                _context.Set<SupplierProposalEntity>().Update(proposal);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }

            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        // enhancements (will not be private)
        private async Task<bool> IsBuyerParticipating(string buyerId, string bidId)
        {
            var p = await _context.Set<ParticipancyEntity>().FindAsync(bidId, buyerId).ConfigureAwait(false);
            return p != null;
        }

        private SupplierProposalEntity GetProposalWithMaxVotes(string bidId)
        {
            SupplierProposalEntity proposalEntity = _context.Set<SupplierProposalEntity>().Where(p => p.BidId == bidId).Include(p => p.Supplier).Aggregate(
                (currWinner, x) => (currWinner == null || x.Votes > currWinner.Votes ? x : currWinner));
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

        private async Task<Response<List<ParticipancyDTO>>> GetBidParticipations(string bidId)
        {
            try
            {
                List<ParticipancyDTO> participancies = await _context.Set<ParticipancyEntity>().Where(p => p.BidId == bidId).Select(p => _mapper.Map<ParticipancyDTO>(p)).ToListAsync().ConfigureAwait(false);
                return new Response<List<ParticipancyDTO>>() { DTOObject = participancies, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }
            catch
            {
                return new Response<List<ParticipancyDTO>>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
            }
        }
    }
}
