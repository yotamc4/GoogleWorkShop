// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Service.Data.Entities;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierEntity, SupplierDTO>(MemberList.Destination);
            CreateMap<NewSupplierRequest, SupplierEntity>(MemberList.Source);
        }
    }
}
