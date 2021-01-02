// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Data.Entities;
    using UniBuy.WebApi.PublicDataSchemas;

    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductDTO>(MemberList.Destination);
            CreateMap<ProductRequest, ProductEntity>(MemberList.Source);
        }
    }
}
