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

namespace YOTY.Service.Core.Managers.Suppliers
{
    public class SuppliersManager : ISuppliersManager
    {
        private const string SupplierNotFoundFailString = "Failed, Supplier not found";
        private readonly YotyContext _context;
        private readonly IMapper _mapper;

        public SuppliersManager(IMapper mapper, YotyContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Response> CreateSupplier(NewUserRequest request)
        {
            SupplierEntity newSupplier = _mapper.Map<SupplierEntity>(request);
            try
            {
                _context.Suppliers.Add(newSupplier);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = ex.Message };
            }
            return new Response() { IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> DeleteSupplier(string supplierId)
        {
            var supplier = await _context.Suppliers.Where(s => s.Id == supplierId).Include(b => b.CurrentProposals).FirstOrDefaultAsync().ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            _context.Suppliers.Remove(supplier);
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

        public async Task<Response<BidsDTO>> GetBidsWhereChosen(string supplierId, BidsTime timeFilter)
        {
            //validate existence
            var supplier = await _context.Suppliers
                .Where(supplier => supplier.Id == supplierId)
                .Include(supplier => supplier.CurrentProposals)
                .ThenInclude(p => p.Bid)
                .ThenInclude(bid => bid.Product)
                .Include(supplier => supplier.CurrentProposals)
                .ThenInclude(proposal => proposal.Bid)
                .ThenInclude(b => b.ChosenProposal)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (supplier == null)
            {
                return new Response<BidsDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }

            var wonBids = supplier.CurrentProposals
                .Where(p => p.Bid.ChosenProposal?.SupplierId == supplierId)
                .Select(p => p.Bid).Where(bid => FilterBuyerBids(bid, timeFilter))
                .Select(bid => _mapper.Map<BidDTO>(bid))
                .ToList();

            return new Response<BidsDTO>() { DTOObject = BidsDTO.CreateDefaultBidsPage(wonBids), IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<BidsDTO>> GetBidsWhereProposed(string supplierId, BidsTime timeFilter)
        {
            //validate existence
            var supplier = await _context.Suppliers
                .Where(s => s.Id == supplierId)
                .Include(s => s.CurrentProposals)
                .ThenInclude(p => p.Bid)
                .ThenInclude(bid => bid.Product)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (supplier == null)
            {
                return new Response<BidsDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }

            var proposedBids = supplier.CurrentProposals
                .Select(p => p.Bid)
                .Where(bid => FilterBuyerBids(bid, timeFilter))
                .Select(bid => _mapper.Map<BidDTO>(bid))
                .ToList();

            return new Response<BidsDTO>() { DTOObject = BidsDTO.CreateDefaultBidsPage(proposedBids), IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<SupplierDTO>> GetSupplier(string supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<SupplierDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            SupplierDTO supplierDTO = _mapper.Map<SupplierDTO>(supplier);
            return new Response<SupplierDTO>() { DTOObject = supplierDTO, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<List<BidDTO>>> GetSupplierLiveBids(string supplierId)
        {
            DateTime current_time = DateTime.Now;
            var supplier = await _context.Suppliers.Where(s => s.Id == supplierId).Include(b => b.CurrentProposals).ThenInclude(p => p.Bid).FirstOrDefaultAsync().ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<List<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            List<BidDTO> liveBids = supplier.CurrentProposals.Select(o => o.Bid).Where(b => b.ExpirationDate > current_time).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<List<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<List<BidDTO>>> GetSupplierOldBids(string supplierId)
        {
            DateTime current_time = DateTime.Now;
            var supplier = await _context.Suppliers.Where(s => s.Id == supplierId).Include(b => b.CurrentProposals).ThenInclude(p => p.Bid).FirstOrDefaultAsync().ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<List<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            List<BidDTO> liveBids = supplier.CurrentProposals.Select(o => o.Bid).Where(b => b.ExpirationDate <= current_time).Select(bid => _mapper.Map<BidDTO>(bid)).ToList();
            return new Response<List<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response> ModifySupplierDetails(ModifySupplierDetailsRequest request)
        {
            var supplier = await _context.Suppliers.FindAsync(request.SupplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            if (request.Description != null)
            {
                supplier.Description = request.Description;
            }
            if (request.PaymentLink != null)
            {
                supplier.PaymentLink = request.PaymentLink;
            }
            if (request.PhoneNumber != null)
            {
                supplier.PhoneNumber = request.PhoneNumber;
            }
            if (request.Email != null)
            {
                supplier.Email = request.Email;
            }
            if (request.ProfilePicture != null)
            {
                supplier.ProfilePicture = request.ProfilePicture;
            }
            try
            {
                _context.Suppliers.Update(supplier);
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

        private static bool FilterBuyerBids(BidEntity buyerBid, BidsTime timeFilter)
        {
            // TODO remove duplicate code
            HashSet<BidPhase> livePhases = new HashSet<BidPhase>() { BidPhase.Join, BidPhase.Vote, BidPhase.Payment };
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
    }
}
