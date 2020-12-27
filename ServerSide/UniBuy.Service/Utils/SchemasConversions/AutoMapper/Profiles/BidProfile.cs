// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Service.Data.Entities;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public class BidProfile: Profile
    {
        public BidProfile()
        {
            CreateMap<BidEntity, BidDTO>(MemberList.Destination);
            CreateMap<NewBidRequest, BidEntity>(MemberList.Source);
        }
    }
}
