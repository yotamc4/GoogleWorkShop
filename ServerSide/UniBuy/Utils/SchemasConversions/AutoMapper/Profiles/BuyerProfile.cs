// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Data.Entities;
    using UniBuy.WebApi.PublicDataSchemas;

    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            CreateMap<BuyerEntity, BuyerDTO>(MemberList.Destination);
            CreateMap<NewBuyerRequest, BuyerEntity>(MemberList.Source);
        }
    }
}
