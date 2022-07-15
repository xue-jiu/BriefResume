using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : ControllerBase
    {
        private readonly UserManager<Seeker> _jobSeekerManager;
        private readonly IAblityRepository _ablityManager;
        private readonly IJobSeekerAttributeRepository _seekerAttributeManager;
        public JobSeekerController(UserManager<Seeker> userManager, IAblityRepository ablityManager, IJobSeekerAttributeRepository seekerAttributeManager)
        {
            _jobSeekerManager = userManager;
            _ablityManager = ablityManager;
            _seekerAttributeManager = seekerAttributeManager;
        }

        //找出指定Jobseeker
        [HttpGet("{JobseekerEmail}", Name = nameof(GetJobseeker))]
        [Authorize]
        public async Task<IActionResult> GetJobseeker([FromRoute] string seelerId, [FromQuery] string fields) 
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seelerId);
            if (JobseekerFormRepo==null)
            {
                return NotFound("没有找到该用户");
            }
            return Ok(JobseekerFormRepo);
        }

        //关键字查询Jobseeker
        [HttpGet]
        public IActionResult GetJobSeekers()
        {
            var Users = _jobSeekerManager.Users.ToList();
            return Ok(Users);
        }

    }
}
