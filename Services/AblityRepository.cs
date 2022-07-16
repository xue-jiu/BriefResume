using BriefResume.DataBase;
using BriefResume.Models;
using BriefResume.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public class AblityRepository : BaseService<Ablity>, IAblityRepository
    {
        private readonly UserDbContext _userDbContext;
        public AblityRepository(UserDbContext userDbContext) : base(userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<IEnumerable<Ablity>> GetAbilitiesAsync(string seekerId)
        {
            return await _userDbContext.ablities.Where(c => c.SeekerAttribute.JobSeekerId == seekerId).ToListAsync();
        }


    }
}
