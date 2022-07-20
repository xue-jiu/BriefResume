using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Profiles
{
    public class AblityFile : Profile
    {
        public AblityFile()
        {
            CreateMap<AblityCreateDto, Ablity>();
            CreateMap<Ablity, AblityUpdateDto>();
            CreateMap<AblityUpdateDto, Ablity>();
        }
    }
}
