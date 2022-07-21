using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.ResourceParameters;
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
    public class JobSeekerController:ControllerBase
    {
        private readonly SeekerManager _jobSeekerManager;
        private readonly IAblityRepository _ablityManager;
        private readonly IJobSeekerAttributeRepository _seekerAttributeManager;
        private readonly IMapper _mapper;
        public JobSeekerController
            (IAblityRepository ablityRepository, 
            IJobSeekerAttributeRepository jobSeekerAttributeRepository, 
            IMapper mapper,
            SeekerManager userManager)
        {
            _ablityManager = ablityRepository;
            _seekerAttributeManager = jobSeekerAttributeRepository;
            _mapper = mapper;
            _jobSeekerManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetJobSeekerAttributes([FromQuery] JobSeekerParameter jobSeekerParameter,[FromQuery] PaginationParamaters paginationParamaters )
        {
            //关键字查询,分页,排序
            var AttributesFromRepo = await _seekerAttributeManager.GetSeekerAttributes(jobSeekerParameter, paginationParamaters);




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
            if (result == true)
            {
                return Ok("创建成功");
            }
            return BadRequest("创建失败");
        }

    }
}
