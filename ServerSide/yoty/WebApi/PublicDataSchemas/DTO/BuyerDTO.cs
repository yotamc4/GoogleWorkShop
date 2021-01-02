// Copyright (c) YOTY Corporation and contributors. All rights reserved.

using System;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class BuyerDTO: BaseDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public Uri ProfilePicture { get; set; }

    }
}
