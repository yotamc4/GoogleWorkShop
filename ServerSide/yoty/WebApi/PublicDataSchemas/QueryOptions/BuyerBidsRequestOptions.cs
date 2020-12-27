// Copyright (c) YOTY Corporation and contributors. All rights reserved.


namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Newtonsoft.Json;

    public class BuyerBidsRequestOptions
    {        
        public bool IsCreatedByBuyer { get; set; }

        public BidsTime BidsTime { get; set; }
    }

    public enum BidsTime
    {
        None,
        Live,
        Old
    }
}
