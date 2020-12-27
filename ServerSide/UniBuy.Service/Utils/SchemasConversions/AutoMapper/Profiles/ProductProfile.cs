// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Service.Data.Entities;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductDTO>(MemberList.Destination);
            CreateMap<ProductRequest, ProductEntity>(MemberList.Source);
        }
    }
}
