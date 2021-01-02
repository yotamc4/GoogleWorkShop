// Copyright (c) YOTY Corporation and contributors. All rights reserved.
using AutoMapper;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            CreateMap<BuyerEntity, BuyerDTO>(MemberList.Destination);
            CreateMap<NewUserRequest, BuyerEntity>(MemberList.Source);
        }
    }
}
