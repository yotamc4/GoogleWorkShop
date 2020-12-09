// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Suppliers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public interface ISuppliersManager
    {
        Task<Response<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest); // c0
        Task<Response<SupplierDTO>> ModifySupplierDetails(); // c0
        Task<Response> DeleteSupplier(string buyerId); // c0
        Task<Response<SupplierDTO>> GetSupplier(string buyerId); //c0
        Task<Response<IList<BidDTO>>> GetSupplierLiveBids(string buyerId); //c0
        Task<Response<IList<BidDTO>>> GetSupplierOldBids(string buyerId); //c1
    }

}
