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
    using YOTY.Service.Utils;
    using YOTY.Service.WebApi.PublicDataSchemas;

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
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidBuyerJoinRequest.BidId).Include(b => b.CurrentParticipancies).Include(b => b.Product).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            if(await this.isValidJoinAsync(bid.Product, bidBuyerJoinRequest.BuyerId))
            {
                // new Response Error Code
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "Buyer already participates in an active bid with this product" };
            }
            bid.CurrentParticipancies.Add(new ParticipancyEntity {
                BidId = bidBuyerJoinRequest.BidId,
                BuyerId = bidBuyerJoinRequest.BuyerId,
                NumOfUnits = bidBuyerJoinRequest.Items,
            });
            bid.UnitsCounter += bidBuyerJoinRequest.Items;
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

        private async Task<bool> isValidJoinAsync(ProductEntity product, string buyerId)
        {
            HashSet<BidPhase> livePhases = new HashSet<BidPhase>() { BidPhase.Join, BidPhase.Vote, BidPhase.Payment };
            var buyer = await _context.Buyers.Where(b => b.Id == buyerId).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync();
            HashSet<string> bid_ids = new HashSet<string>(buyer.CurrentParticipancies.Select(p => p.BidId).ToArray());
            var alreadyInBidWithThisProduct = _context.Bids.Where(b => bid_ids.Contains(b.Id)).Where(b=> livePhases.Contains(b.Phase)).Include(b => b.Product).Any(b => b.Product.Id == product.Id);
            return !alreadyInBidWithThisProduct;
        }

        public async Task<Response> AddSupplierProposal(SupplierProposalRequest supplierProposalRequest)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == supplierProposalRequest.BidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
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
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> CreateNewBid(NewBidRequest bidRequest)
        {
            BidEntity bidEntity = _mapper.Map<BidEntity>(bidRequest);
            PopulateBidEntity(bidEntity);
            
            ProductEntity existingProduct = await FindExistingProductAsync(bidRequest.Product);
            if (existingProduct != null)
            {
                bidEntity.Product = existingProduct;
            }
            if (await this.isValidNewBidAsync(bidEntity))
            {
                // new Response Error Code
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "Not Valid Group: This owner has an active bid of this product / There are too many groups for this product / There is an equivalent available group already" };
            }
            _context.Bids.Add(bidEntity);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //TODO log exception and return proper error message instead
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

        }

        private async Task<bool> isValidNewBidAsync(BidEntity bidEntity)
        {
            var activeBidsWithSameProduct = await _context.Bids.Include(b => b.Product).Where(b => b.Product.Id == bidEntity.Product.Id && b.Phase == BidPhase.Join).ToListAsync();
            if (activeBidsWithSameProduct.Count() > 3)
            {
                return false;
            }
            if (activeBidsWithSameProduct.Any(b=>b.MaxPrice == bidEntity.MaxPrice || b.OwnerId == bidEntity.OwnerId))
            {
                return false;
            }
            return true;
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
            BidEntity bid = await _context.Bids.Where(b => b.Id == editBidRequest.BidId).Include(bid => bid.Product).FirstOrDefaultAsync().ConfigureAwait(false);

            if (bid == null)
            {
                return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }

            bid.Product.Name = editBidRequest.NewName ?? bid.Product.Name;
            bid.Product.Image = editBidRequest.NewProductImage ?? bid.Product.Image;
            bid.Product.Description = editBidRequest.NewDescription ?? bid.Product.Description;
            bid.Category = editBidRequest.NewCategory ?? bid.Category;
            bid.SubCategory = editBidRequest.NewSubCategory ?? bid.SubCategory;

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

        public async Task<Response<BidDTO>> GetBid(string bidId, string userId, string userRole)
        {
            BidEntity bid;
            BidDTO bidDTO;
            if (userRole?.Equals("anonymous", StringComparison.OrdinalIgnoreCase) ?? true)
            {
                bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.Product).FirstOrDefaultAsync().ConfigureAwait(false);
                if (bid == null)
                {
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
                }
                bidDTO = _mapper.Map<BidDTO>(bid);
                return new Response<BidDTO>() { DTOObject = bidDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }
            else if (userRole.Equals("Consumer", StringComparison.OrdinalIgnoreCase))
            {
                bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.Product).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);
                if (bid == null)
                {
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
                }
                bidDTO = _mapper.Map<BidDTO>(bid);
                ParticipancyEntity participancy = bid.CurrentParticipancies.Where(p => p.BuyerId == userId).FirstOrDefault();
                bidDTO.IsUserInBid = participancy != null;
                bidDTO.HasVoted = participancy?.HasVoted ?? false;
                return new Response<BidDTO>() { DTOObject = bidDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };               
            }
            else if (userRole.Equals("Supplier", StringComparison.OrdinalIgnoreCase))
            {
                bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.Product).Include(b => b.CurrentProposals).Include(b => b.ChosenProposal).FirstOrDefaultAsync().ConfigureAwait(false);
                if (bid == null)
                {
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
                }
                bidDTO = _mapper.Map<BidDTO>(bid);
                bidDTO.IsUserInBid = bid.ChosenProposal != null ? bid.ChosenProposal.SupplierId == userId : bid.CurrentProposals.Any(proposal => proposal.SupplierId == userId);
                return  new Response<BidDTO>() { DTOObject = bidDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };       
            }
            return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = $"Bad input - unexpected user role: {userRole}" };
        }

        public async Task<Response<List<BuyerDTO>>> GetBidBuyers(string bidId)
        {
            var bid_ent = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentParticipancies).ThenInclude(p => p.Buyer).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response<List<BuyerDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };

            }

            List<BuyerDTO> buyers = bid_ent.CurrentParticipancies.Select(p => _mapper.Map<BuyerDTO>(p.Buyer)).ToList();
            return new Response<List<BuyerDTO>>() { DTOObject = buyers, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<List<SupplierProposalDTO>>> GetBidSuppliersProposals(string bidId)
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

        public async Task<Response<SupplierProposalDTO>> GetBidChosenProposal(string bidId)
        {
            try
            {
                var chosenProposal = await _context.Bids.Where(b => b.Id == bidId).Include(b=>b.ChosenProposal).Select(b => b.ChosenProposal).FirstOrDefaultAsync().ConfigureAwait(false);
                if(chosenProposal == null){
                    return new Response<SupplierProposalDTO>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
                }
                return new Response<SupplierProposalDTO>() { DTOObject = _mapper.Map<SupplierProposalDTO>(chosenProposal), IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }
            catch
            {
                return new Response<SupplierProposalDTO>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
            }
        }


        public async Task<Response<List<ParticipancyDTO>>> GetBidParticipations(string bidId)
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

        public async Task<Response<List<ParticipancyFullDetailsDTO>>> GetBidParticipationsFullDetails(string bidId)
        {
            try
            {
                List<ParticipancyFullDetailsDTO> participancies = await _context.Set<ParticipancyEntity>().Where(p => p.BidId == bidId).Select(p => _mapper.Map<ParticipancyFullDetailsDTO>(p)).ToListAsync().ConfigureAwait(false);
                return new Response<List<ParticipancyFullDetailsDTO>>() { DTOObject = participancies, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
            }
            catch
            {
                return new Response<List<ParticipancyFullDetailsDTO>>() { IsOperationSucceeded = false, SuccessOrFailureMessage = "error querying for proposals" };
            }
        }

        public async Task<Response<BidsDTO>> GetBids(BidsQueryOptions bidsFilters)
        {

            // first category and sub category 
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

                IEnumerable<BidEntity> sortedBids = this.GetSortBids(filteredBids, bidsFilters.SortOrder, bidsFilters.SortBy);

                // return page
                int pageSize = bidsFilters.Limit == 0 || bidsFilters.Limit > _maxPageSize ? _pageDefaultSize : bidsFilters.Limit;

                var bidsPage = sortedBids
                    .Skip(bidsFilters.Page * pageSize)
                    .Take(pageSize)
                    .Select(bidEntity => _mapper.Map<BidDTO>(bidEntity))
                    .ToList();

                BidsDTO bidsDTO = new BidsDTO(
                    pageNumber: bidsFilters.Page,
                    numberOfPages: (int)Math.Ceiling((double)filteredBids.Count() / pageSize),
                    bidsPage: bidsPage);

                return new Response<BidsDTO>() { DTOObject = bidsDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

            }
        }

        private async Task<Response<BidsDTO>> GetDefaultHomePageBids(int page)
        {
            List<BidDTO> bids = await _context.Bids
                .Where(bid => bid.Phase == BidPhase.Join && bid.Phase == BidPhase.Vote)
                .OrderByDescending(bid => bid.UnitsCounter)
                .OrderByDescending(bid => bid.PotenialSuplliersCounter)
                .OrderBy(bid => bid.ExpirationDate)
                .Skip(page * _pageDefaultSize)
                .Take(_pageDefaultSize)
                .Include(bidEntity => bidEntity.Product)
                .Select(bidEntitiy => _mapper.Map<BidDTO>(bidEntitiy))
                .ToListAsync().ConfigureAwait(false);


            int numberOfBids = await _context.Bids.CountAsync().ConfigureAwait(false);


            BidsDTO bidsDTO = new BidsDTO(
                pageNumber: page,
                numberOfPages: (int)Math.Ceiling((double)numberOfBids / _pageDefaultSize),
                bidsPage: bids);

            return new Response<BidsDTO>() { DTOObject = bidsDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        private bool ValidateBidsFilters(BidsQueryOptions bidsFilters, out string validationErrorString)
        {
            validationErrorString = null;
            string demandedCategory = bidsFilters.Category;
            string demandedSubCategory = bidsFilters.SubCategory;
            if (demandedSubCategory != null && demandedCategory == null)
            {
                validationErrorString = $"Sub catergory has set to :{bidsFilters.SubCategory } while category is null";
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

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }

        private static bool FilterByCategories(BidEntity bid, string category, string subCategory)
        {
            if (category == null)
            {
                return true;
            }
            else if (bid.Category.Equals(category) && subCategory == null)
            {
                return true;
            }
            else if (bid.SubCategory != null)
            {
                return bid.Category.Equals(category) && bid.SubCategory.Equals(subCategory);
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
            return queryString == null || bid.Product.Name.Contains(queryString, StringComparison.OrdinalIgnoreCase) || bid.Product.Description.Contains(queryString, StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<BidEntity> GetFilteredBids(BidsQueryOptions bidsFilters)
        {
            return _context.Bids
                .Where(bid => bid.Phase == BidPhase.Join && bid.Phase == BidPhase.Vote)
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

        public async Task<Response> GetProposalWithMaxVotes(string bidId)
        {
            BidEntity bid = await _context.Bids.Where(bid => bid.Id == bidId).Include(bid => bid.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SupplierProposalEntity chosenProposalEntity = bid.CurrentProposals.Where(proposal => proposal.MinimumUnits <= bid.UnitsCounter && proposal.ProposedPrice <= bid.MaxPrice).Aggregate(
                (currWinner, x) => (currWinner == null || x.Votes > currWinner.Votes ? x : currWinner));
            bid.ChosenProposal = chosenProposalEntity;
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

        public async Task<Response> MarkPaid(MarkPaidRequest request)
        {
            // TODO add validation that the marking user is the chosen supplier
            DbSet<ParticipancyEntity> participancies_db = _context.Set<ParticipancyEntity>();
            var p = await participancies_db.FindAsync(request.BidId, request.BuyerId).ConfigureAwait(false);
            if (p == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = "participation not found" };
            }
            p.HasPaid = request.HasPaid;
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

        public async Task<Response<BidPhase>> TryUpdatePhase(string bidId)
        {
            BidEntity bid_ent = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentProposals).Include(b => b.CurrentParticipancies).FirstOrDefaultAsync().ConfigureAwait(false);

            if (bid_ent == null)
            {
                return new Response<BidPhase>() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            BidPhase currentPhase = bid_ent.Phase;
            BidPhase newPhase = currentPhase;
            switch (currentPhase)
            {
                case BidPhase.Join:
                    if (bid_ent.ExpirationDate <= DateTime.Now)
                    {
                        List<SupplierProposalEntity> relevantProposals = bid_ent.CurrentProposals.Where(proposal => proposal.MinimumUnits <= bid_ent.UnitsCounter && proposal.ProposedPrice <= bid_ent.MaxPrice).ToList();
                        int numOfProposals = relevantProposals.Count();
                        if (numOfProposals == 0)
                        {
                            newPhase = BidPhase.CancelledSupplierNotFound;
                        }
                        else if (numOfProposals == 1)
                        {
                            newPhase = BidPhase.Payment;
                        }
                        else
                        {
                            newPhase = BidPhase.Vote;
                        }
                    }
                    break;
                case BidPhase.Vote:
                    if (bid_ent.ExpirationDate.AddHours(48) <= DateTime.Now)
                    {
                        newPhase = BidPhase.Payment;
                    }
                    break;
                case BidPhase.Payment:
                    if (bid_ent.ExpirationDate.AddDays(5) <= DateTime.Now)
                    {
                        if (bid_ent.CurrentParticipancies.Any(p => p.HasPaid == false))
                        {
                            newPhase = BidPhase.CancelledNotEnoughBuyersPayed;
                        }
                        else
                        {
                            newPhase = BidPhase.Completed;
                        }
                    }
                    break;
                // All others should update synchronously (with events)
                default:
                    break;
            }
            if (newPhase != currentPhase)
            {
                bid_ent.Phase = newPhase;
                _context.Bids.Update(bid_ent);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return new Response<BidPhase>() { IsOperationSucceeded = newPhase != currentPhase, SuccessOrFailureMessage = this.getSuccessMessage(), DTOObject = newPhase };
        }

        public async Task<Response> UpdateBidProposalsToRelevant(string bidId)
        {
            BidEntity bid_ent = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (bid_ent == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            try
            {
                bid_ent.CurrentProposals = bid_ent.CurrentProposals.Where(proposal => proposal.MinimumUnits <= bid_ent.UnitsCounter && proposal.ProposedPrice <= bid_ent.MaxPrice).ToList();
                if (bid_ent.CurrentProposals.Count() == 1)
                {
                    bid_ent.ChosenProposal = bid_ent.CurrentProposals.First();
                }
                _context.Bids.Update(bid_ent);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response<BidPhase>() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> CancelBid(string bidId)
        {
            return await this.ModifyBidPhase(bidId, BidPhase.CancelledNotEnoughBuyersPayed).ConfigureAwait(false);
        }

        public async Task<Response> CompleteBid(string bidId)
        {
            return await this.ModifyBidPhase(bidId, BidPhase.Completed).ConfigureAwait(false);
        }

        private async Task<Response> ModifyBidPhase(string bidId, BidPhase newPhase)
        {
            BidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            bid.Phase = newPhase;
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

        public async Task<Response<List<OrderDetailsDTO>>> GetPaidCustomersFullOrderDetails(string bidId, string userId)
        {
            BidEntity bid = await _context.Bids.Where(b => b.Id == bidId).Include(b => b.CurrentParticipancies).ThenInclude(p=>p.Buyer).Include(b => b.ChosenProposal).FirstOrDefaultAsync();
            if (bid == null)
            {
                return new Response<List<OrderDetailsDTO>>() { IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            List<OrderDetailsDTO> orderDetailsList = bid.CurrentParticipancies.Where(p => p.HasPaid).Select(p => new OrderDetailsDTO{
                BuyerName = p.Buyer.Name,
                BuyerEmail = p.Buyer.Email,
                BuyerAddress = p.Buyer.Address,
                BuyerPhoneNumber = p.Buyer.PhoneNumber,
                BuyerPostalCode = p.Buyer.PostalCode,
                NumOfOrderedUnits = p.NumOfUnits
            }).ToList();

            return new Response<List<OrderDetailsDTO>>() { DTOObject = orderDetailsList, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }
        private async Task<ProductEntity> FindExistingProductAsync(ProductRequest productRequest)
        {
            try
            {
                var Products = _context.Set<ProductEntity>();
                string lowerName = productRequest.Name.ToLower();
                ProductEntity result = await Products.Where(product => product.Name.ToLower() == lowerName).FirstOrDefaultAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }

        private static void PopulateBidEntity(BidEntity bidEntity)
        {
            //TODO is this the time we want? (or global).
            bidEntity.CreationDate = DateTime.Now;
            bidEntity.Id = Guid.NewGuid().ToString();
            bidEntity.UnitsCounter = 0;
            bidEntity.PotenialSuplliersCounter = 0;
            bidEntity.Product.Id = Guid.NewGuid().ToString();
            bidEntity.CurrentParticipancies = new List<ParticipancyEntity>();
            bidEntity.CurrentProposals = new List<SupplierProposalEntity>();
            bidEntity.Phase = BidPhase.Join;
        }
    }
}
