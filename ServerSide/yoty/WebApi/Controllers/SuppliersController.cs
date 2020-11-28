using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YOTY.Service.WebApi.PublicDataSchemas;
using YOTY.Service.Core.Managers.Suppliers;
using Microsoft.AspNetCore.Http;

namespace YOTY.Service.WebApi.Controllers
{
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
            if (response.IsOperationSuccsseded)
            {
                return this.StatusCode(StatusCodes.Status201Created, response.DTOObject);
            }
            return this.StatusCode(StatusCodes.Status403Forbidden, response.SuccessOrFailureMessage);
        }

        [HttpGet]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(string supplierId)
        {
            Response<SupplierDTO> response = await this.suppliersManager.GetSupplier(supplierId).ConfigureAwait(false);
            if (response.IsOperationSuccsseded)
            {

                return response.DTOObject;
            }
            // at the moment
            return this.StatusCode(StatusCodes.Status404NotFound, response.SuccessOrFailureMessage);
        }
    }
}
