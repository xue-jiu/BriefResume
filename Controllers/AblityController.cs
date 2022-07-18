using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using BriefResume.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using System.Threading.Tasks;

namespace BriefResume.Controllers
{
    [Route("api/seeker/{seelerId}/[controller]")]
    [ApiController]
    public class AblityController:ControllerBase
    {
        private readonly IAblityRepository _ablityManager;
        private readonly IMapper _mapper;
        private readonly UserManager<Seeker> _userManager;
        public AblityController(IAblityRepository ablityRepository, IMapper mapper, UserManager<Seeker> userManager)
        {
            _ablityManager = ablityRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAblityAsync([FromRoute]string seelerId)
        {
            var seekerFromRepo = await _userManager.FindByIdAsync(seelerId);
            if (seekerFromRepo == null)
            {
                return NotFound("没有对应的人");
            }
            var AblitiesFromRepo = await _ablityManager.GetAbilitiesAsync(seelerId);
            if (AblitiesFromRepo==null)
            {
                return NotFound("这个人一点用都没有");
            }
            return Ok(AblitiesFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> AddAblityAsync([FromRoute] string seelerId,[FromBody] AblityCreateDto ablityCreateDto )
        {
            var seekerFromRepo = await _userManager.FindByIdAsync(seelerId);
            if (seekerFromRepo == null)
            {
                return NotFound("没有对应的人");
            }
            var AbilityFromPage =  _mapper.Map<Ablity>(ablityCreateDto);
            var result = ((AblityRepository)_ablityManager).Create(AbilityFromPage);
            if (result==false)
            {
                return BadRequest("能力创建失败");
            }
            return Ok("创建成功");
        }

        [HttpPatch("ablityId")]
        public async Task<IActionResult> PartiallyUpdateAblityAsync([FromBody]JsonPatchDocument<AblityUpdateDto> patchDocument,[FromRoute]string ablityId)
        {
            Ablity ablityFromRepo = ((AblityRepository)_ablityManager).Find(ablityId);
            if (ablityFromRepo == null)
            {
                return BadRequest("该能力不存在");
            }
            var ablityToPatch = _mapper.Map<AblityUpdateDto>(ablityFromRepo);
            patchDocument.ApplyTo(ablityToPatch);//此处有bug
            if (!TryValidateModel(patchDocument))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(ablityToPatch, ablityFromRepo);
            await ((AblityRepository)_ablityManager).SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("ablityId")]
        public IActionResult DeleteAblity([FromRoute] string ablityId)
        {
            Ablity ablityFromRepo = ((AblityRepository)_ablityManager).Find(ablityId);
            if (ablityFromRepo == null)
            {
                return BadRequest("该能力不存在");
            }
            var result = ((AblityRepository)_ablityManager).Delete(ablityFromRepo);
            if (!result)
            {
                return BadRequest("删除失败");
            }
            return Ok("成功");
        }




    }
}
