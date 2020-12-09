using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Core.Managers.Suppliers
{
    public class SuppliersManager : ISuppliersManager
    {
        public Task<Response<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteSupplier(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SupplierDTO>> GetSupplier(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetSupplierDeals(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetSupplierLiveBids(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BidDTO>> GetSupplierOldBids(string buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SupplierDTO>> ModifySupplierDetails()
        {
            throw new NotImplementedException();
        }
    }
}
