// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Data.Entities;
    using UniBuy.WebApi.PublicDataSchemas;

    public class SupplierProposalProfile : Profile
    {
        public SupplierProposalProfile()
        {
            CreateMap<SupplierProposalEntity, SupplierProposalDTO>(MemberList.Destination);
            CreateMap<SupplierProposalRequest, SupplierProposalEntity>(MemberList.Source);
        }
    }
}
