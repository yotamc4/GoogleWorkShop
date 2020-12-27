// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Data.Entities;
    using UniBuy.WebApi.PublicDataSchemas;

    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<SupplierEntity, SupplierDTO>(MemberList.Destination);
            CreateMap<NewSupplierRequest, SupplierEntity>(MemberList.Source);
        }
    }
}
