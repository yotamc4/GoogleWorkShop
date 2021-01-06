// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;

    public class YotyController: ControllerBase
    {
        protected string GetUserId()
        {
            return 
                User
                .Claims
                .FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)
                ?.Value ;
        }

        protected string GetRequestUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
