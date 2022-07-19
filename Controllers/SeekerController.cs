using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace BriefResume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerController : ControllerBase
    {
        private readonly UserManager<Seeker> _jobSeekerManager;
        private readonly IAblityRepository _ablityManager;
        private readonly IJobSeekerAttributeRepository _seekerAttributeManager;
        private readonly IMapper _mapper;
        public SeekerController(UserManager<Seeker> userManager, IAblityRepository ablityManager, IJobSeekerAttributeRepository seekerAttributeManager, IMapper mapper)
        {
            _jobSeekerManager = userManager;
            _ablityManager = ablityManager;
            _seekerAttributeManager = seekerAttributeManager;
            _mapper = mapper;
        }

        //查出所有jobseeker
        //此方法不全
        //创建一个字典,把seeker和jA他tribute的信息都传递进去
        [HttpGet]
        public IActionResult GetJobSeekers()
        {
            var Users = _jobSeekerManager.Users.ToList();
            return Ok(Users);
        }

        //找出指定Jobseeker
        [HttpGet("{seekerId}", Name = nameof(GetJobseeker))]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetJobseeker([FromRoute] string seekerId) 
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo==null)
            {
                return NotFound("没有找到该用户");
            }
            return Ok(JobseekerFormRepo);
        }

        //因为特征删除一般都是软删除,所以这里delete方法就没写,只写更新操作
        //更新操作 如果要用patch需要重写usermanager,不太方便
        [HttpPut("{seekerId}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateJobSeeker(
            [FromRoute]string seekerId,
            [FromBody] SeekerUpdateDto seekerUpdateDto)
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            var UpdateDto =  _mapper.Map<Seeker>(seekerUpdateDto);
            await _jobSeekerManager.UpdateAsync(UpdateDto);
            return NoContent();
        }

     }
}
