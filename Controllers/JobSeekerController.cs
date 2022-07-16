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
        private readonly IMapper _mapper;
        public JobSeekerController(UserManager<Seeker> userManager, IAblityRepository ablityManager, IJobSeekerAttributeRepository seekerAttributeManager, IMapper mapper)
        {
            _jobSeekerManager = userManager;
            _ablityManager = ablityManager;
            _seekerAttributeManager = seekerAttributeManager;
            _mapper = mapper;
        }

        //查出所有jobseeker
        //此方法不全
        [HttpGet]
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

        [HttpPost("{seekerId}")]
        [Authorize]
        public async Task<IActionResult> CreateJobSeekerAttribute([FromBody] SeekerAttributeCreateDto seekerCreateAttribute, string seekerId)
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            var jobseekerAttributeFromRepo = await _seekerAttributeManager.GetSeekerAttributeBySeekerIdAsync(seekerId);
            if (jobseekerAttributeFromRepo != null)
            {
                return NotFound("该用户特征信息不存在");//应当让客户在初次打开App时注册
            }
            await _seekerAttributeManager.CreateSeekerAttributeAsync(_mapper.Map<SeekerAttribute>(seekerCreateAttribute), seekerId);
            var result = await _seekerAttributeManager.SaveChangesAsync();
            if (result==true)
            {
                return Ok("创建成功");
            }
            return BadRequest("创建失败");
        }

        //因为特征删除一般都是软删除,所以这里delete方法就没写,只写更新操作
        //更新操作 没有选择用put 直接用patch
        [HttpPatch]
        public async Task<IActionResult> UpdateJobSeekerAttribute([FromBody] SeekerAttributeCreateDto seekerCreateAttribute, string seekerId)
        {
            return Ok();
        }




        }
}
