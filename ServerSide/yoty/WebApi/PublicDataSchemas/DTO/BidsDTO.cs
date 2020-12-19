// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System.Collections.Generic;

    public class BidsDTO
    {

        public int PageSize { get; set; }

        public int numberOfPages { get; set; }

        //value between 0 to n-1 when n = numberOfPages
        public int PageNumber { get; set; }

        public List<BidDTO> bidsPage {get;set;}
    }
}
