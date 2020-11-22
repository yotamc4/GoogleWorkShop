// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Microsoft.AspNetCore.Mvc;
    //[BindProperties(SupportsGet = true)]
    public class BidsFilters
    {
        // pagination
        public int? Page { get; set; }

        public int? Limit { get; set; }
        
        public BidsSortByOptions? SortBy { get; set; }

        public BidsSortByOrder? SortOrder { get; set; }

        // the client could pass here the most specifid category - we should save categories as a string that contains all the categories and sub catergories of the product 
        // see IHerb request for refference 
        [FromRoute]
        public string Category { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }
        //public BidsSortByOptions? SortBy { get; }

    }
}
