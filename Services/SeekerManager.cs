using BriefResume.DataBase;
using BriefResume.Dtos;
using BriefResume.Helper;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.ResourceParameters;
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
        private readonly IPropertyMappingService _propertyMappingService;
        public SeekerManager(
            IUserStore<Seeker> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<Seeker> passwordHasher, 
            IEnumerable<IUserValidator<Seeker>> userValidators, 
            IEnumerable<IPasswordValidator<Seeker>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<Seeker>> logger, 
            UserDbContext userDbContext,
            IPropertyMappingService propertyMappingService
            ) 
            :base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userDbContext = userDbContext;
            _propertyMappingService = propertyMappingService;
        }
        //根据一对一关系找出对应的Id
        public async Task<Guid> FindJobseekerIdAttributeAsync(string seekerId)
        {
            return await _userDbContext.seekerAttributes
                .Where(c => c.JobSeekerId == seekerId)
                .Select(c => c.SeekerAttributeId)
                .FirstOrDefaultAsync();
        }
        public async Task<Guid> FindInterviewerIdAttributeAsync(string seekerId)
        {
            return await _userDbContext.interviewerAttributes
                .Where(c => c.InterviewerId == seekerId)
                .Select(c => c.InterviewerAttributeId)
                .FirstOrDefaultAsync();
        }
        //对Users进行分页,排序,关键字搜索
        public async Task<PagedList<Seeker>> FindSeekerAsync
            (SeekerParameter seekerParameter,
            PaginationParamaters paginationParamaters)
        {
            if (seekerParameter == null)
            {
                throw new ArgumentNullException(nameof(seekerParameter));
            }
            var IQuaryResult = base.Users;
            if (!string.IsNullOrWhiteSpace(seekerParameter.Email))
            {
                var KeyParam = seekerParameter.Email.Trim();
                IQuaryResult = IQuaryResult.Where(t => t.Email.Contains(KeyParam));
            }
            if (!string.IsNullOrWhiteSpace(seekerParameter.UserName))
            {
                var KeyParam = seekerParameter.UserName.Trim();
                IQuaryResult = IQuaryResult.Where(t => t.UserName.Contains(KeyParam));
            }
            if (!string.IsNullOrWhiteSpace(seekerParameter.PhoneNumber))
            {
                var KeyParam = seekerParameter.PhoneNumber.Trim();
                IQuaryResult = IQuaryResult.Where(t => t.UserName.Contains(KeyParam));
            }
            //排序
            if (!string.IsNullOrWhiteSpace(seekerParameter.OrderBy))
            {
                var SeekerMap = _propertyMappingService.GetPropertyMapping<SeekerDto, Seeker>();
                IQuaryResult = IQuaryResult.ApplySort(seekerParameter.OrderBy, SeekerMap);
            }
            //多表链接
            if (IQuaryResult==null)
            {
                return null;
            }
            IQuaryResult = IQuaryResult.Include(c => c.SeekerAttribute);
            IQuaryResult = IQuaryResult.Include(c => c.InterviewerAttribute);
            //分页
            return await PagedList<Seeker>.CreateAsync(IQuaryResult, paginationParamaters.PageNumber, paginationParamaters.PageSize);
        }

    }
}
