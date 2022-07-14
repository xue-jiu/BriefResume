using BriefResume.DataBase;
using BriefResume.Models;
using BriefResume.Service;
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
        



    }
}
