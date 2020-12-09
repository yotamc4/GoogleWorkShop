using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YOTY.Service.Data;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Suppliers
{
    public class SuppliersManager : ISuppliersManager
    {
        private const string SupplierNotFoundFailString = "Failed, Supplier not found";
        private static YotyContext _context = new YotyContext();

        public Task<Response<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteSupplier(string supplierId)
        {
            var supplier = await _context.Sellers.FindAsync(supplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response() { IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            using (var new_context = new YotyContext())
            {
                new_context.Sellers.Remove(supplier);
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

        public async Task<Response<SupplierDTO>> GetSupplier(string supplierId)
        {
            var supplier = await _context.Sellers.FindAsync(supplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<SupplierDTO>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            //TODO add mapper from ent to dto  
            return new Response<SupplierDTO>() { DTOObject = null, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<IList<BidDTO>>> GetSupplierLiveBids(string supplierId)
        {
            DateTime current_time = DateTime.Now;
            var supplier = await _context.Sellers.FindAsync(supplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<IList<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            //TODO add mapper from bid_ent to dto       ------------------------------------------------------------->                        here
            IList<BidDTO> liveBids = supplier.CurrentOffers.Select(o => o.Bid).Where(b => b.ExpirationDate > current_time).Select(bid => new BidDTO()).ToList();
            return new Response<IList<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
        }

        public async Task<Response<IList<BidDTO>>> GetSupplierOldBids(string supplierId)
        {
            DateTime current_time = DateTime.Now;
            var supplier = await _context.Sellers.FindAsync(supplierId).ConfigureAwait(false);
            if (supplier == null)
            {
                return new Response<IList<BidDTO>>() { DTOObject = null, IsOperationSucceeded = false, SuccessOrFailureMessage = SupplierNotFoundFailString };
            }
            //TODO add mapper from bid_ent to dto       ------------------------------------------------------------->                        here
            IList<BidDTO> liveBids = supplier.CurrentOffers.Select(o => o.Bid).Where(b => b.ExpirationDate <= current_time).Select(bid => new BidDTO()).ToList();
            return new Response<IList<BidDTO>>() { DTOObject = liveBids, IsOperationSucceeded = true, SuccessOrFailureMessage = this.getSuccessMessage() };
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
