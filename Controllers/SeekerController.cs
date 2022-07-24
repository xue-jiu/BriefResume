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
        private readonly SeekerManager _jobSeekerManager;
        private readonly IAblityRepository _ablityManager;
        private readonly IJobSeekerAttributeRepository _seekerAttributeManager;
        private readonly IMapper _mapper;
        public SeekerController(
            SeekerManager userManager, 
            IAblityRepository ablityManager, 
            IJobSeekerAttributeRepository seekerAttributeManager, 
            IMapper mapper)
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
        [Authorize(Roles = "manager")]
        public IActionResult GetJobSeekers()
        {
            var Users = _jobSeekerManager.Users.ToList();
            return Ok(Users);
        }

        //找出指定Jobseeker
        [HttpGet("{seekerId}", Name = nameof(GetJobseeker))]
        [Authorize]
        public async Task<IActionResult> GetJobseeker([FromRoute] string seekerId) 
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo==null)
            {
                return NotFound("没有找到该用户");
            }
            return Ok(JobseekerFormRepo);
        }

        //更新操作 如果要用patch需要重写usermanager,不太方便
        [HttpPut("{seekerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateSeeker(
            [FromRoute]string seekerId,
            [FromBody] SeekerUpdateDto seekerUpdateDto)
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            //var UpdateDto =  _mapper.Map<Seeker>(seekerUpdateDto);
            _mapper.Map(seekerUpdateDto,JobseekerFormRepo);
            var result = await _jobSeekerManager.UpdateAsync(JobseekerFormRepo);
            if (!result.Succeeded)
            {
                return BadRequest("更新失败");
            }
            return NoContent();
        }

        [HttpDelete("{seekerId}")]
        public async Task<IActionResult> DeleteSeeker(string seekerId)
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            var result = await _jobSeekerManager.DeleteAsync(JobseekerFormRepo);
            if (!result.Succeeded)
            {
                return BadRequest("删除失败");
            }
            return Ok("删除成功");
        }

    }
}
