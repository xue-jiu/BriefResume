using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Helper;
using BriefResume.Helpers;
using BriefResume.Models;
using BriefResume.Parameters;
using BriefResume.ResourceParameters;
using BriefResume.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Security.Policy;
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

        [HttpGet(Name =nameof(GetJobSeekerAttributes))]
        [Authorize]
        public async Task<IActionResult> GetJobSeekerAttributes(
            [FromQuery] JobSeekerParameter jobSeekerParameter,
            [FromQuery] PaginationParamaters paginationParamaters )
        {
            //关键字查询,分页,排序
            var AttributesFromRepo = await _seekerAttributeManager.GetSeekerAttributes(jobSeekerParameter, paginationParamaters);
            if (AttributesFromRepo == null)
            {
                return NotFound("没有符合的人选");
            }
            var JobSeekerAttributeDto = _mapper.Map<IEnumerable<SeekerAttributeDto>>(AttributesFromRepo);
            //输出元数据,上下页数据url
            var previousPageLink = AttributesFromRepo.HasPrevious? CreateJobseekerAttributeUri(jobSeekerParameter, paginationParamaters, ResourceUriType.PreviousPage): null;
            var nextPageLink = AttributesFromRepo.HasNext ? CreateJobseekerAttributeUri( jobSeekerParameter, paginationParamaters, ResourceUriType.NextPage) : null;
            var paginationMetadata = new
            {
                pageNumber = paginationParamaters.PageNumber - 1,
                pageSize = paginationParamaters.PageSize,
                Colleage = jobSeekerParameter.Colleage,
                OrderBy = jobSeekerParameter.OrderBy,
                MaxExpadSalary = jobSeekerParameter.MaxExpadSalary,
                MinExpadSalary = jobSeekerParameter.MinExpadSalary
            };
            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            JobSeekerAttributeDto.ShapeData(jobSeekerParameter.Fields);

            var AttributeAfterShape = JobSeekerAttributeDto.ShapeData(jobSeekerParameter.Fields);
            return Ok(AttributeAfterShape);
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

        [HttpPatch("{seekerId}")]
        public async Task<IActionResult> ParticalUpdateJobSeekerAttribute(
            [FromRoute]string seekerId,
            [FromBody] JsonPatchDocument<SeekerAttributeUpdateDto> jsonPatchDocument)
        {
            var JobseekerFormRepo = await _jobSeekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            var jobseekerAttributeFromRepo = await _seekerAttributeManager.GetSeekerAttributeBySeekerIdAsync(seekerId);
            if (jobseekerAttributeFromRepo != null)
            {
                return NotFound("该用户特征信息不存在");
            }
            var AttributeToApply = _mapper.Map<SeekerAttributeUpdateDto>(jobseekerAttributeFromRepo);
            jsonPatchDocument.ApplyTo(AttributeToApply,ModelState);
            if (!TryValidateModel(jsonPatchDocument))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(AttributeToApply, jobseekerAttributeFromRepo);
            var result = await _seekerAttributeManager.SaveChangesAsync();
            if (result == true)
            {
                return Ok("创建成功");
            }
            return BadRequest("创建失败");
        }


        private string CreateJobseekerAttributeUri
            (JobSeekerParameter jobSeekerParameter,
            PaginationParamaters paginationParamaters,
            ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetJobSeekerAttributes), new
                    {
                        pageNumber = paginationParamaters.PageNumber - 1,
                        pageSize = paginationParamaters.PageSize,
                        Colleage = jobSeekerParameter.Colleage,
                        OrderBy = jobSeekerParameter.OrderBy,
                        MaxExpadSalary= jobSeekerParameter.MaxExpadSalary,
                        MinExpadSalary=jobSeekerParameter.MinExpadSalary
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetJobSeekerAttributes), new//controllerbase自带urlhelper
                    {
                        pageNumber = paginationParamaters.PageNumber + 1,
                        pageSize = paginationParamaters.PageSize,
                        Colleage = jobSeekerParameter.Colleage,
                        OrderBy = jobSeekerParameter.OrderBy,
                        MaxExpadSalary = jobSeekerParameter.MaxExpadSalary,
                        MinExpadSalary = jobSeekerParameter.MinExpadSalary

                    });

                default:
                    return Url.Link(nameof(GetJobSeekerAttributes), new
                    {
                        pageNumber = paginationParamaters.PageNumber,
                        pageSize = paginationParamaters.PageSize,
                        Colleage = jobSeekerParameter.Colleage,
                        OrderBy = jobSeekerParameter.OrderBy,
                        MaxExpadSalary = jobSeekerParameter.MaxExpadSalary,
                        MinExpadSalary = jobSeekerParameter.MinExpadSalary
                    });
            }
        }
    }
}