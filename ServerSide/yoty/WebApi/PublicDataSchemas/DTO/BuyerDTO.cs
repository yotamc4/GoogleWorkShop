// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class BuyerDTO: BaseDTO
    {
        string Id { get; set; }

        FacebookAccount FacebookAccount { get; set; }

        BuyerAccountDeatails BuyerAccountDeatails { get;set;}
    
    }
}
