// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Core.Managers.Buyers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UniBuy.WebApi.PublicDataSchemas;

    public interface IBuyersManager
    {
        Task<Response<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest); // c0
        Task<Response<BuyerDTO>> ModifyBuyerDetails(); // c0
        Task<Response> DeleteBuyer(string buyerId); // c0
        Task<Response<BuyerDTO>> GetBuyer(string buyerId); //c0

        Task<Response<BidsDTO>> GetBuyerBids(string buyerId, BidsTime timeFiler);

        Task<Response<BidsDTO>> GetBidsCreatedByBuyer(string buyerId, BidsTime timeFiler);//c1

    }
}
