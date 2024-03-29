﻿using BriefResume.Models;
using BriefResume.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Services
{
    public interface IAblityRepository
    {
        Task<IEnumerable<Ablity>> GetAbilitiesAsync(string seekerId);
        Task<Ablity> GetAblityAsync(Guid ablityId);
        Task CreateAblitities(Ablity ablity);
        Task<bool> SaveChangesAsync();
    }
}
