﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Core.Managers.Suppliers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UniBuy.WebApi.PublicDataSchemas;

    public interface ISuppliersManager
    {
        Task<Response<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest); // c0
        Task<Response<SupplierDTO>> ModifySupplierDetails(); // c0
        Task<Response> DeleteSupplier(string supplierId); // c0
        Task<Response<SupplierDTO>> GetSupplier(string supplierId); //c0
        Task<Response<List<BidDTO>>> GetSupplierLiveBids(string supplierId); //c0
        Task<Response<List<BidDTO>>> GetSupplierOldBids(string supplierId); //c1
    }

}
