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
            var seekerFromRepo = await _userManager.FindByEmailAsync(seelerId);
            if (seekerFromRepo == null)
            {
                return NotFound();
            }
            var SeekerId = await _userManager.GetUserIdAsync(seekerFromRepo);
            var AblitiesFromRepo = ((AblityRepository)_ablityManager).Find(seelerId);
            if (AblitiesFromRepo==null)
            {
                return NotFound();
            }
            return Ok(AblitiesFromRepo);
        }

    }
}
