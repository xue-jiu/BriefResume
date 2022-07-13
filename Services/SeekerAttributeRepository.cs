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
    public class SeekerAttributeRepository : BaseService<SeekerAttribute>
    {
        private readonly UserDbContext _userDbContext;
        public SeekerAttributeRepository(UserDbContext userDbContext) :base(userDbContext)
        {
            _userDbContext = userDbContext;
        }

    }
}
