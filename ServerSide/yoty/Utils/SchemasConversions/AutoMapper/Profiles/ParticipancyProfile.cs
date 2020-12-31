// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Utils.SchemasConversions.AutoMapper.Profiles
{
    using global::AutoMapper;
    using YOTY.Service.Data.Entities;
    using YOTY.Service.WebApi.PublicDataSchemas;

    public class ParticipancyProfile
    {
        public ParticipancyProfile()
        {
            CreateMap<ParticipancyEntity, ParticipancyDTO>(MemberList.Destination);
        }
    }
}
