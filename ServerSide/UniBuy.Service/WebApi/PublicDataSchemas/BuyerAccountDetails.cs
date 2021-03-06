﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.WebApi.PublicDataSchemas
{
    public class BuyerAccountDetails
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; } // string at the moment
    }
}