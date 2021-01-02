// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using UniBuy.Service.Data.Entities;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    public class SupplierProposalProfile : Profile
    {
        public SupplierProposalProfile()
        {
            CreateMap<SupplierProposalEntity, SupplierProposalDTO>(MemberList.Destination);
            CreateMap<SupplierProposalRequest, SupplierProposalEntity>(MemberList.Source);
        }
    }
}
