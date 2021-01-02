// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Suppliers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public interface ISuppliersManager
    {
        Task<Response> CreateSupplier(NewUserRequest newSupplierRequest); // c0
        Task<Response> ModifySupplierDetails(ModifySupplierDetailsRequest request); // c0
        Task<Response> DeleteSupplier(string supplierId); // c0
        Task<Response<SupplierDTO>> GetSupplier(string supplierId); //c0
        Task<Response<List<BidDTO>>> GetSupplierLiveBids(string supplierId); //c0
        Task<Response<List<BidDTO>>> GetSupplierOldBids(string supplierId); //c1
    }

}
