using BriefResume.DataBase;
using BriefResume.IService;
using BriefResume.Models;
using BriefResume.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public class JobSeekerAttributeRepository : BaseService<SeekerAttribute>, IJobSeekerAttributeRepository
    {
        private readonly UserDbContext _userDbContext;
        public JobSeekerAttributeRepository(UserDbContext userDbContext) :base(userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<SeekerAttribute> GetSeekerAttributeBySeekerIdAsync(string seekerId)
        {
            return await _userDbContext.Users.Where(c => c.Id == seekerId).Select(c => c.SeekerAttribute).FirstOrDefaultAsync();
        }

        public async Task CreateSeekerAttributeAsync(SeekerAttribute seekerAttribute,string seekerId)
        {
            seekerAttribute.JobSeekerId = seekerId;
            await _userDbContext.seekerAttributes.AddAsync(seekerAttribute);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _userDbContext.SaveChangesAsync() >= 0);
        }



    }
}
