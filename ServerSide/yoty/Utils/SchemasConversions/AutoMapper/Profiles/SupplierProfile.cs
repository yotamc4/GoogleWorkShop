// Copyright (c) YOTY Corporation and contributors. All rights reserved.
using AutoMapper;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierEntity, SupplierDTO>(MemberList.Destination);
            CreateMap<NewUserRequest, SupplierEntity>(MemberList.Source);
        }
    }
}
