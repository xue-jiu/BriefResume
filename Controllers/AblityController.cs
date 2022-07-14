using BriefResume.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AblityController:ControllerBase
    {
        private readonly IAblityRepository _ablityManager;
        public AblityController(IAblityRepository ablityRepository)
        {
            _ablityManager = ablityRepository;
        }

        [HttpGet("{JobseekerId}")]
        public IActionResult JobseekerAblity(Guid JobseekerId)
        {
            var AblitiesFromRepo = ((AblityRepository)_ablityManager).Find(JobseekerId);
            return Ok(AblitiesFromRepo);
        }



    }
}
