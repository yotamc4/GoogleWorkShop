// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Service.Data.Entities;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            CreateMap<BuyerEntity, BuyerDTO>(MemberList.Destination);
            CreateMap<NewBuyerRequest, BuyerEntity>(MemberList.Source);
        }
    }
}
