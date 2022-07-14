using BriefResume.DataBase;
using BriefResume.IService;
using BriefResume.Models;
using BriefResume.Service;
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



    }
}
