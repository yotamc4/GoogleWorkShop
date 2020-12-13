// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductDTO>(MemberList.Destination);
            CreateMap<ProductRequest, ProductEntity>(MemberList.Source);
        }
    }
}
