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

        Task<Response<BidsDTO>> GetBuyerBids(string buyerId);

        Task<Response<BidsDTO>> GetBuyerLiveBids(string buyerId); //c1

        Task<Response<BidsDTO>> GetBuyerOldBids(string buyerId); //c1

        Task<Response<BidsDTO>> GetBidsCreatedByBuyer(string buyerId);//c1

    }
}
