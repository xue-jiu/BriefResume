using BriefResume.DataBase;
using BriefResume.Dtos;
using BriefResume.Helper;
using BriefResume.IService;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.ResourceParameters;
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
        private readonly IPropertyMappingService _propertyMappingService;
        public JobSeekerAttributeRepository(
            UserDbContext userDbContext,
            IPropertyMappingService propertyMappingService) :base(userDbContext)
        {
            _userDbContext = userDbContext;
            _propertyMappingService = propertyMappingService;
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

        public async Task<PagedList<SeekerAttribute>> GetSeekerAttributes(
            JobSeekerParameter jobSeekerParameter,
            PaginationParamaters paginationParamaters)
        {
            IQueryable<SeekerAttribute> IQuaryResult = _userDbContext.seekerAttributes;
            if (jobSeekerParameter==null)
            {
                throw new ArgumentNullException(nameof(jobSeekerParameter));
            }
            if (!string.IsNullOrWhiteSpace(jobSeekerParameter.Colleage))
            {
                var CollageContain = jobSeekerParameter.Colleage.Trim();
                IQuaryResult = IQuaryResult.Where(t => t.Colleage.Contains(CollageContain));
            }
            if (jobSeekerParameter.MinExpadSalary!=null)
            {
                IQuaryResult = IQuaryResult.Where(t => t.ExpadSalary > jobSeekerParameter.MinExpadSalary);
            }
            if (jobSeekerParameter.MaxExpadSalary != null)
            {
                IQuaryResult = IQuaryResult.Where(t => t.ExpadSalary < jobSeekerParameter.MaxExpadSalary);
            }
            //排序
            if (!string.IsNullOrWhiteSpace(jobSeekerParameter.OrderBy))
            {
                var SeekerMap = _propertyMappingService.GetPropertyMapping<SeekerAttributeDto, SeekerAttribute>();
                IQuaryResult = IQuaryResult.ApplySort(jobSeekerParameter.OrderBy, SeekerMap);
            }
            return await PagedList<SeekerAttribute>.CreateAsync(IQuaryResult, paginationParamaters.PageNumber, paginationParamaters.PageSize);

        }
    }
}
