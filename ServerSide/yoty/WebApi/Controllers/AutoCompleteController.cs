// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YOTY.Service.Data;
    using YOTY.Service.Data.Entities;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AutoCompleteController : ControllerBase
    {
        private YotyContext _context;

        public AutoCompleteController(YotyContext context)
        {
            this._context = context;
        }
        [HttpGet]
        [Route("Products")]
        public async Task<object> GetrecordAsync()
        {
            List<string> data;
            try
            {
                data = await _context.Set<ProductEntity>().Select(product => product.Name).ToListAsync();
            }
            catch
            {
                data = new List<string>();
            }

            return data;
        }
    }
}
