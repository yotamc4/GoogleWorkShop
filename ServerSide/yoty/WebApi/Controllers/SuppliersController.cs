// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using YOTY.Service.Core.Managers.Suppliers;
    using YOTY.Service.WebApi.PublicDataSchemas;
    
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class SuppliersController: ControllerBase
    {
        private ISuppliersManager suppliersManager;

        public SuppliersController(ISuppliersManager suppliersManager)
        {
            this.suppliersManager = suppliersManager;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSupplier(NewUserRequest newSupplierRequest)
        {
            Response response = await this.suppliersManager.CreateSupplier(newSupplierRequest).ConfigureAwait(false);
            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.SuccessOrFailureMessage);
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

        [HttpPost]
        [Route("details")]
        public async Task<ActionResult> ModifyBuyerDetails(ModifySupplierDetailsRequest request)
        {
            Response response = await this.suppliersManager.ModifySupplierDetails(request);

            if (response.IsOperationSucceeded)
            {
                return this.StatusCode(StatusCodes.Status200OK, response.SuccessOrFailureMessage);
            }

            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }
    }
}
