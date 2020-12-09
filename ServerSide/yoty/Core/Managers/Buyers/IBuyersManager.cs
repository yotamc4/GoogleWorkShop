// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Buyers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public interface IBuyersManager
    {
        Task<Response<BuyerDTO>> CreateBuyer(NewBuyerRequest newBuyerRequest); // c0
        Task<Response<BuyerDTO>> ModifyBuyerDetails(); // c0
        Task<Response> DeleteBuyer(string buyerId); // c0
        Task<Response<BuyerDTO>> GetBuyer(string buyerId); //c0

        Task<Response<IList<BidDTO>>> GetBuyerLiveBids(string buyerId); //c1

        Task<Response<IList<BidDTO>>> GetBuyerOldBids(string buyerId); //c1

        Task<Response<IList<BidDTO>>> GetBidsCreatedByBuyer(string buyerId);//c1

    }
}
