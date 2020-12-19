// Copyright (c) YOTY Corporation and contributors. All rights reserved.
using AutoMapper;
using YOTY.Service.Data.Entities;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    public class SupplierProposalProfile : Profile
    {
        public SupplierProposalProfile()
        {
            CreateMap<SupplierProposalEntity, SupplierProposalDTO>(MemberList.Destination);
            CreateMap<SupplierProposalRequest, SupplierProposalEntity>(MemberList.Source);
        }
    }
}
