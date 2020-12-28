// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Data.Entities;
    using UniBuy.WebApi.PublicDataSchemas;

    public class BidProfile: Profile
    {
        public BidProfile()
        {
            CreateMap<BidEntity, BidDTO>(MemberList.Destination);
            CreateMap<NewBidRequest, BidEntity>(MemberList.Source);
        }
    }
}
