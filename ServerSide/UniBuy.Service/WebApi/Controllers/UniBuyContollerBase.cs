// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.WebApi.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;

    public abstract class UniBuyContollerBase: ControllerBase
    {
        protected string GetUserId()
        {

            //string x1= this.User.Claims.First(i => i.Type == "UserId")?.Value;
            string x2 = this.User.Claims
                       .FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

            return x2;
        }
    }
}
