using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Profiles
{
    public class JobseekerFile : Profile
    {
        public JobseekerFile()
        {
            CreateMap<SeekerAttributeCreateDto, SeekerAttribute>().ForMember
                (
                    dest => dest.Degree,
                    opt => opt.MapFrom(src => src.Degree.ToString())
                );
        }
    }
}
