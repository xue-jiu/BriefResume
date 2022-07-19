using BriefResume.Models;
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
        Task<IEnumerable<SeekerAttribute>> GetSeekerAttributes();

    }
}
