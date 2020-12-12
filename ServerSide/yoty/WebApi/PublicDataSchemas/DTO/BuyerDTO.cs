// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class BuyerDTO: BaseDTO
    {
        public string Id { get; set; }

        FacebookAccount FacebookAccount { get; set; }

        public BuyerAccountDetails BuyerAccountDeatails { get;set;}
    
    }
}
