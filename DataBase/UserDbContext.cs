using BriefResume.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.DataBase
{
    public class UserDbContext : IdentityDbContext<JobSeeker, RoleExtension, Guid>
    {
    }
}
