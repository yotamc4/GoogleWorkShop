// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Bids
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using AutoMapper;
    using YOTY.Service.Data;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.Utils;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class BidsManager : IBidsManager
    {
        private const string BidNotFoundFailString = "Failed, Bid not found";
        private static YotyContext _context = new YotyContext();
        private readonly IMapper _mapper;
        private int _pageDefaultSize = 9;
        private int _maxPageSize = 20;

        public BidsManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Response<BuyerDTO>> AddBuyer(BidBuyerJoinRequest bidBuyerJoinRequest)
        {
            BidEntity bid = await _context.Bids.FindAsync(bidBuyerJoinRequest.bidId).ConfigureAwait(false);
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
            BuyerEntity buyer_ent = await _context.Buyers.FindAsync(bidBuyerJoinRequest.buyerId).ConfigureAwait(false);
            BuyerDTO buyer_dto = _mapper.Map<BuyerDTO>(buyer_ent);
            return new Response<BuyerDTO>() { DTOObject = buyer_dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<SupplierProposalDTO>> AddSupplierProposal(SupplierProposalRequest supplierProposalRequest)
        {
            BidEntity bid = await _context.Bids.FindAsync(supplierProposalRequest.BidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response<SupplierProposalDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SupplierProposalEntity new_proposal_ent = _mapper.Map<SupplierProposalEntity>(supplierProposalRequest);
            bid.CurrentProposals.Add(new_proposal_ent);
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
            SupplierEntity supplier_ent = await _context.Suppliers.FindAsync(supplierProposalRequest.SupplierId).ConfigureAwait(false);
            SupplierProposalDTO supplier_dto = _mapper.Map<SupplierProposalDTO>(supplier_ent);
            return new Response<SupplierProposalDTO>() { DTOObject = supplier_dto, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidDTO>> CreateNewBid(NewBidRequst bidRequest)
        {
            BidEntity bidEnitity = _mapper.Map<BidEntity>(bidRequest);
            //TODO is this the time we want? (or global).
            bidEnitity.CreationDate = DateTime.Now;
            bidEnitity.Id = this.GenerateBidId().ToString();
            bidEnitity.UnitsCounter = 0;
            bidEnitity.PotenialSuplliersCounter = 0;

            using (var new_context = new YotyContext())
            {
                new_context.Bids.Add(bidEnitity);
                try
                {
                    await new_context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    //TODO log exception and return proper error message instead
                    return new Response<BidDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
                }
            }
            BidDTO dto = _mapper.Map<BidDTO>(bidEnitity);
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
            BidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
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
            BidEntity bid = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
            if (bid == null)
            {
                return new Response() {IsOperationSucceeded = false, SuccessOrFailureMessage = BidNotFoundFailString };
            }
            SupplierProposalEntity proposal = bid.CurrentProposals.Find(p => p.SupplierId == supplierId);
            bid.CurrentProposals.Remove(proposal);
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

        public async Task<Response<List<BidDTO>>> GetBids(BidsQueryOptions bidsFilters)
        {

            // firts category and sub category 
            // then 
            if (bidsFilters.Category == null && bidsFilters.Search == null)
            {
                return this.GetDefaultHomePageBids(bidsFilters.Page);
            }
            else if (!ValidateBidsFilters(bidsFilters, out string validationErorrString))
            {
                return new Response<List<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = validationErorrString };
            }
            else // TODO : extract to sub methods
            {
                IEnumerable<BidEntity> filteredBids = _context.Bids
                    // filter by Categories
                    .Where(bid => bid.Category == null ?
                                true :
                                bid.Category.Equals(bidsFilters.Category) &&
                                (bidsFilters.SubCategory == null) ?
                                    true :
                                    bid.SubCategory != null && bid.SubCategory.Equals(bidsFilters.Category))
                    //filter by prices
                    .Where(bid => bidsFilters.MaxPrice < Int32.MaxValue ? bid.MaxPrice < bidsFilters.MaxPrice : true &&
                            bidsFilters.MinPrice > 0 ? bid.MaxPrice > bidsFilters.MinPrice : true)
                    // filter by query string
                    .Where(bid => bid.Product.Name.Contains(bidsFilters.Search) || bid.Product.Description.Contains(bidsFilters.Search));
                
                //sort
                IEnumerable<BidEntity> sortedBids;
                BidsSortByOrder sortOrder = bidsFilters.SortOrder;
                switch (bidsFilters.SortBy)
                {
                    case BidsSortByOptions.CreationDate:
                        sortedBids = filteredBids.Sort(bid => bid.CreationDate, sortOrder);
                        break;
                    case BidsSortByOptions.ExpirationDate:
                        sortedBids = filteredBids.Sort(bid => bid.ExpirationDate, sortOrder);
                        break;
                    case BidsSortByOptions.SupplierProposals:
                        sortedBids = filteredBids.Sort(bid => bid.PotenialSuplliersCounter, sortOrder);
                        break;
                    case BidsSortByOptions.DemandedItems:
                        sortedBids = filteredBids.Sort(bid => bid.UnitsCounter, sortOrder);
                        break;
                    case BidsSortByOptions.Price:
                        sortedBids = filteredBids.Sort(bid => bid.MaxPrice, sortOrder);
                        break;
                    default:
                        sortedBids = filteredBids.Sort(bid => bid.MaxPrice, sortOrder);
                        break;
                }

                // return page
                int pageSize = bidsFilters.Limit == 0 || bidsFilters.Limit > _maxPageSize ? _pageDefaultSize : bidsFilters.Limit;

                var bidsPage = sortedBids
                    .Skip(bidsFilters.Page * pageSize)
                    .Take(pageSize)
                    .Select (bidEntity => _mapper.Map<BidDTO>(bidEntity))
                    .ToList();

                return new Response<List<BidDTO>>() { DTOObject = bidsPage, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };

            }
        }

        private Response<List<BidDTO>> GetDefaultHomePageBids (int page)
        {
            List<BidDTO> bids =  _context.Bids
                .OrderByDescending(bid => bid.UnitsCounter)
                .OrderByDescending(bid => bid.PotenialSuplliersCounter)
                .OrderBy(bid => bid.ExpirationDate)
                .Skip(page * _pageDefaultSize)
                .Take(_pageDefaultSize)
                .Select(bidEntitiy => _mapper.Map<BidDTO>(bidEntitiy))
                .ToList();

            return new Response<List<BidDTO>>() { DTOObject = bids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
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

            // should implement it smarter with creating categories table and with kind of index on it 
            if (!_context.Bids.Select(bid =>bid.Category).ToHashSet().Contains(demandedCategory))
            {
                validationErorrString = $"Catergory has set to :{demandedCategory } while category is not exist in DB";
                return false;
            }
            // should implemnt also table of Category-SubCategory with many to one relationship to vaildate....

            return true;
        }

        public async Task<Response<List<SupplierProposalDTO>>> GetBidSuplliersProposals(string bidId)
        {
            var bid_ent = await _context.Bids.FindAsync(bidId).ConfigureAwait(false);
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
