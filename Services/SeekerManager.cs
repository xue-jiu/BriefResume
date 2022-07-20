using BriefResume.DataBase;
using BriefResume.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public class SeekerManager : UserManager<Seeker>
    {
        private readonly UserDbContext _userDbContext;
        public SeekerManager(IUserStore<Seeker> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<Seeker> passwordHasher, IEnumerable<IUserValidator<Seeker>> userValidators, IEnumerable<IPasswordValidator<Seeker>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Seeker>> logger, UserDbContext userDbContext) 
            :base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userDbContext = userDbContext;
        }
        //根据一对一关系找出对应的Id
        public async Task<Guid> FindJobseekerIdAttributeAsync(string seekerId)
        {
            return await _userDbContext.seekerAttributes.Where(c => c.JobSeekerId == seekerId).Select(c => c.SeekerAttributeId).FirstOrDefaultAsync();
        }
        public async Task<Guid> FindInterviewerIdAttributeAsync(string seekerId)
        {
            return await _userDbContext.interviewerAttributes.Where(c => c.InterviewerId == seekerId).Select(c => c.InterviewerAttributeId).FirstOrDefaultAsync();
        }


    }
}
