using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Profiles
{
    public class SeekerFile : Profile
    {
        public SeekerFile()
        {
            CreateMap<SeekerUpdateDto, Seeker>();
            CreateMap<Seeker, SeekerDto>();
        }
    }
}
