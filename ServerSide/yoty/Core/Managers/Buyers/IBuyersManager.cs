// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Core.Managers.Buyers
{
    using System.Threading.Tasks;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public interface IBuyersManager
    {
        Task<Response> CreateBuyer(NewUserRequest newUserRequest); // c0
        Task<Response> ModifyBuyerDetails(ModifyBuyerDetailsRequest request); // c0
        Task<Response> DeleteBuyer(string buyerId); // c0
        Task<Response<BuyerDTO>> GetBuyer(string buyerId); //c0

        Task<Response<BidsDTO>> GetBuyerBids(string buyerId, BidsTime timeFiler);

        Task<Response<BidsDTO>> GetBidsCreatedByBuyer(string buyerId, BidsTime timeFiler);//c1

    }
}
