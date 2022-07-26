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
using BriefResume.ResourceParameters;
using BriefResume.Helpers;
using BriefResume.Helper;
using Microsoft.Extensions.Caching.Memory;

namespace BriefResume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerController : ControllerBase
    {
        private readonly SeekerManager _seekerManager;
        private readonly IAblityRepository _ablityManager;
        private readonly IJobSeekerAttributeRepository _seekerAttributeManager;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IPropertyMappingService _propertyMappingService;
        public SeekerController(
            SeekerManager userManager, 
            IAblityRepository ablityManager, 
            IJobSeekerAttributeRepository seekerAttributeManager, 
            IMapper mapper,
            IMemoryCache memoryCache,
            IPropertyMappingService propertyMappingService)
        {
            _seekerManager = userManager;
            _ablityManager = ablityManager;
            _seekerAttributeManager = seekerAttributeManager;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _propertyMappingService = propertyMappingService;
        }

        //查出所有jobseeker
        //此方法不全
        //创建一个字典,把seeker和jA他tribute的信息都传递进去
        [HttpGet(Name =nameof(GetJobSeekersAsync))]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetJobSeekersAsync([FromQuery]SeekerParameter seekerParameter, [FromQuery] PaginationParamaters paginationParamaters)
        {
            if (!_propertyMappingService.IsMappingExists<SeekerDto, Seeker>(seekerParameter.OrderBy))
            {
                return BadRequest("请输入正确的排序参数");
            }

            var seekerFromRepo = await  _memoryCache.GetOrCreateAsync("JobSeekers", async e =>
            {
                e.SlidingExpiration = TimeSpan.FromMinutes(5);//设置5分钟缓存
                return await _seekerManager.FindSeekerAsync(seekerParameter, paginationParamaters);
            });
                
            //元数据
            var previousPageLink = seekerFromRepo.HasPrevious ? CreateSeekerPageUri(seekerParameter, paginationParamaters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = seekerFromRepo.HasNext ? CreateSeekerPageUri(seekerParameter, paginationParamaters, ResourceUriType.NextPage) : null;
            var paginationMetadata = new
            {
                pageNumber = paginationParamaters.PageNumber,
                pageSize = paginationParamaters.PageSize,
                Email = seekerParameter.Email,
                OrderBy = seekerParameter.OrderBy,
                UserName = seekerParameter.UserName,
                PhoneNumber = seekerParameter.PhoneNumber
            };
            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var SeekerDtoShows= _mapper.Map<IEnumerable<SeekerDto>>(seekerFromRepo);
            //var SeekerDtoShows = seekerFromRepo;
            return Ok(SeekerDtoShows);
        }

        //找出指定Jobseeker
        [HttpGet("{seekerId}", Name = nameof(GetJobseeker))]
        [Authorize]
        public async Task<IActionResult> GetJobseeker([FromRoute] string seekerId) 
        {
            var JobseekerFormRepo = await _seekerManager.FindByIdAsync(seekerId);
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
            var JobseekerFormRepo = await _seekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            //var UpdateDto =  _mapper.Map<Seeker>(seekerUpdateDto);
            _mapper.Map(seekerUpdateDto,JobseekerFormRepo);
            var result = await _seekerManager.UpdateAsync(JobseekerFormRepo);
            if (!result.Succeeded)
            {
                return BadRequest("更新失败");
            }
            return NoContent();
        }

        [HttpDelete("{seekerId}")]
        public async Task<IActionResult> DeleteSeeker(string seekerId)
        {
            var JobseekerFormRepo = await _seekerManager.FindByIdAsync(seekerId);
            if (JobseekerFormRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            var result = await _seekerManager.DeleteAsync(JobseekerFormRepo);
            if (!result.Succeeded)
            {
                return BadRequest("删除失败");
            }
            return Ok("删除成功");
        }

        private string CreateSeekerPageUri
            (SeekerParameter seekerParameter,
            PaginationParamaters paginationParamaters,
            ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetJobSeekersAsync), new
                    {
                        pageNumber = paginationParamaters.PageNumber - 1,
                        pageSize = paginationParamaters.PageSize,
                        Email = seekerParameter.Email,
                        OrderBy = seekerParameter.OrderBy,
                        UserName = seekerParameter.UserName,
                        PhoneNumber = seekerParameter.PhoneNumber
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetJobSeekersAsync), new//controllerbase自带urlhelper
                    {
                        pageNumber = paginationParamaters.PageNumber + 1,
                        pageSize = paginationParamaters.PageSize,
                        Email = seekerParameter.Email,
                        OrderBy = seekerParameter.OrderBy,
                        UserName = seekerParameter.UserName,
                        PhoneNumber = seekerParameter.PhoneNumber
                    });

                default:
                    return Url.Link(nameof(GetJobSeekersAsync), new
                    {
                        pageNumber = paginationParamaters.PageNumber,
                        pageSize = paginationParamaters.PageSize,
                        Email = seekerParameter.Email,
                        OrderBy = seekerParameter.OrderBy,
                        UserName = seekerParameter.UserName,
                        PhoneNumber = seekerParameter.PhoneNumber
                    });
            }
        }

    }
}
