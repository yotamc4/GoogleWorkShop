// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System.Collections.Generic;

    public class BidsDTO
    {

        public int PageSize { get; set; }

        public int NumberOfPages { get; set; }

        //value between 0 to n-1 when n = numberOfPages
        public int PageNumber { get; set; }

        public List<BidDTO> BidsPage {get;set;}

        public BidsDTO(int pageNumber, int numberOfPages, List<BidDTO> bidsPage)
        {        
            PageNumber = pageNumber;
            NumberOfPages = numberOfPages;
            PageSize = bidsPage.Count;
            BidsPage = bidsPage;
        }

        public static BidsDTO CreateDefaultBidsPage(List<BidDTO> bids)
        {
            return new BidsDTO(bids);
        }

        private BidsDTO(List<BidDTO> bids)
        {
            this.PageSize = bids.Count;
            this.NumberOfPages = 1;
            this.PageNumber = 0;
            this.BidsPage = bids;
        }
    }




}
