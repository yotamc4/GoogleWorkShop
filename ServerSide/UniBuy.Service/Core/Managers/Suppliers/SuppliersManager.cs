// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Core.Managers.Suppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using UniBuy.Service.Data;
    using UniBuy.Service.WebApi.PublicDataSchemas;

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
        public Task<Response<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest)
        {
            throw new NotImplementedException();
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

        public Task<Response<SupplierDTO>> ModifySupplierDetails()
        {
            throw new NotImplementedException();
        }

        private string getSuccessMessage([CallerMemberName] string callerName = "")
        {
            return $"{callerName} success";
        }
    }
}
