using BriefResume.Helper;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public interface IJobSeekerAttributeRepository
    {
        Task<SeekerAttribute> GetSeekerAttributeBySeekerIdAsync(string seekerId);
        Task CreateSeekerAttributeAsync(SeekerAttribute seekerAttribute, string seekerId);
        Task<bool> SaveChangesAsync();
        Task<PagedList<SeekerAttribute>> GetSeekerAttributes(JobSeekerParameter jobSeekerParameter, PaginationParamaters paginationParamaters);

    }
}
