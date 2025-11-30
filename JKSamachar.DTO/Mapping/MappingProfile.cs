using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JKSamachar.DAL.Enitity;

namespace JKSamachar.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JkNewsDto, JKNews>().ReverseMap();
            CreateMap<JKNewsResponseDto, JKNews>().ReverseMap();
            CreateMap<JKSerachResponseDto, JKNews>().ReverseMap();
        }
    }
}
