using AutoMapper;
using BriefResume.Dtos;
using BriefResume.Models;
using BriefResume.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetAblityAsync(string seelerId)
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


    }
}
