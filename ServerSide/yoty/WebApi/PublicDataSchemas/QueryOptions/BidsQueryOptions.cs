// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    //[BindProperties(SupportsGet = true)]
    public class BidsQueryOptions
    {
        // pagination
        public int? Page { get; set; }

        public int? Limit { get; set; } // how many items returned, should determined 

        // sorting     
        public BidsSortByOptions SortBy { get; set; }

        public BidsSortByOrder SortOrder { get; set; }

        // the client could pass here the most specified category - we should save categories as a string that contains all the categories and sub categories of the product 
        // see IHerb request for reference 
        public string Category { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }
        //public BidsSortByOptions? SortBy { get; }

        public string search { get; set; }
    }
}
