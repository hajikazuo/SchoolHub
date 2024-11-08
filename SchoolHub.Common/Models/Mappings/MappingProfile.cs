using AutoMapper;
using SchoolHub.Common.Models.Dtos;
using SchoolHub.Common.Models.Entities;

namespace SchoolHub.Common.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tennant, TennantDto>();
            CreateMap<TennantDto, Tennant>();
            CreateMap<TennantDtoUpdate, Tennant>();

            CreateMap<ClassGroup, ClassGroupDto>();
            CreateMap<ClassGroupDto, ClassGroup>();
            CreateMap<ClassGroupDtoUpdate, ClassGroup>();

        }
    }
}
