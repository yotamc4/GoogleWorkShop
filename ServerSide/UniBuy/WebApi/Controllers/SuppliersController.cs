﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using UniBuy.Core.Managers.Suppliers;
    using UniBuy.WebApi.PublicDataSchemas;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class SuppliersController: ControllerBase
    {
        private ISuppliersManager suppliersManager;

        public SuppliersController(ISuppliersManager suppliersManager)
        {
            this.suppliersManager = suppliersManager;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> CreateSupplier(NewSupplierRequest newSupplierRequest)
        {
            Response<SupplierDTO> response = await this.suppliersManager.CreateSupplier(newSupplierRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(string supplierId)
        {
            Response<SupplierDTO> response = await this.suppliersManager.GetSupplier(supplierId).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {

                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }
    }
}
