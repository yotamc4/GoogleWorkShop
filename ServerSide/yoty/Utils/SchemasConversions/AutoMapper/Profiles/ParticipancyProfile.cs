// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class ParticipancyProfile : Profile
    {
        public ParticipancyProfile()
        {
            CreateMap<ParticipancyEntity, ParticipancyDTO>(MemberList.None);
            CreateMap<ParticipancyEntity, ParticipancyFullDetailsDTO>(MemberList.None);
            CreateMap<BidBuyerJoinRequest, ParticipancyEntity>(MemberList.Source);
        }
    }
}
