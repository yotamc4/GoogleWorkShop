// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.WebApi.PublicDataSchemas
{
    using System;

    //[BindProperties(SupportsGet = true)]
    public class BidsQueryOptions
    {
        // pagination
        public int Page { get; set; }

        public int Limit { get; set; } // how many items returned, should determined 

        // sorting     
        public BidsSortByOptions SortBy { get; set; }

        public BidsSortByOrder SortOrder { get; set; }

        // the client could pass here the most specified category - we should save categories as a string that contains all the categories and sub categories of the product 
        // see IHerb request for reference 
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; } = Int32.MaxValue;
        //public BidsSortByOptions? SortBy { get; }

        public string Search { get; set; }
    }
}
