using AutoMapper;
using onboarding.api.Movie.DTO;
using onboarding.api.Nation.DTO;
using onboarding.dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace onboarding.api
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<MovieDTO, MovieModel>();
            CreateMap<MovieModel, MovieDTO>();
            CreateMap<National, NationDTO>();
            CreateMap<NationDTO, National>();
                      //Source    //destination
            CreateMap<MovieModel, MovieWithNationalDTO>();
                
            /*CreateMap<MovieWithNationalDTO, MovieModel>()
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(x => x.Nation.Id))
                .ForMember(dest => dest.Nation.NationName, opt => opt.MapFrom(x => x.Nation.NationName));*/
        }
    }
}
