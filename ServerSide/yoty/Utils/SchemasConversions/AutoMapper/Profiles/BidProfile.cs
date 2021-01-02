// Copyright (c) YOTY Corporation and contributors. All rights reserved.
using AutoMapper;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    public class BidProfile: Profile
    {
        public BidProfile()
        {
            CreateMap<BidEntity, BidDTO>(MemberList.None);
            CreateMap<NewBidRequest, BidEntity>(MemberList.Source);
        }
    }
}
