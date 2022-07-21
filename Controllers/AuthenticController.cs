using BriefResume.Dtos;
using BriefResume.JwtModel;
using BriefResume.Models;
using BriefResume.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BriefResume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticController : ControllerBase
    {
        private readonly SeekerManager _seekerUserManager;
        private readonly RoleManager<RoleExtension> _seekerRoleManager;
        private readonly IOptionsSnapshot<JwtSettings> _jwtSettings;
        public AuthenticController(SeekerManager UserManager, IOptionsSnapshot<JwtSettings> optionsSnapshot, RoleManager<RoleExtension>  roleManager)
        {
            _seekerUserManager = UserManager;
            _seekerRoleManager = roleManager;
            _jwtSettings = optionsSnapshot;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLoginAsync([FromBody] SeekerLoginDto seeker)
        {
            // 1 验证用户名密码
            var user = await _seekerUserManager.FindByEmailAsync(seeker.Email);//tracking
            if (user == null)
                return NotFound($"用户不存在");
            if (await _seekerUserManager.IsLockedOutAsync(user))
                return BadRequest("LockedOut");
            var success = await _seekerUserManager.CheckPasswordAsync(user, seeker.Password);
            if (!success)
            {
                await _seekerUserManager.AccessFailedAsync(user);
            }
            await _seekerUserManager.ResetAccessFailedCountAsync(user);


            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;

            //payload
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email)//将user的信息传出到Token
            };
            var roleNames = await _seekerUserManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);//多角色验证,identity框架默认不是jwt
                claims.Add(roleClaim);//多角色验证,需要加上AuthenticionScamar
            }
            // signiture
            var secretByte = Encoding.UTF8.GetBytes(_jwtSettings.Value.ScrKey);
            var signingKey = new SymmetricSecurityKey(secretByte);//加密
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);//生成signiture
            //claim
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,//也是Claim,但是这两个可能需要频繁改动,所以写在这里
                audience: _jwtSettings.Value.Audience,
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            // 3 return 200 ok + jwt
            return Ok(tokenStr);
        }


        [HttpPost("register")]
        public async Task<IActionResult> UserRegisterAsync([FromBody] SeekerRegisterDto seekerRegisterDto) 
        {
            //预防用户重复注册问题没有解决
            //多方式登录问题没有解决
            var SeekerFromRepo = await _seekerUserManager.FindByEmailAsync(seekerRegisterDto.Email);
            if (SeekerFromRepo!=null)
            {
                return BadRequest("用户名已注册");
            }
            Seeker seeker = new Seeker()
            {
                UserName = seekerRegisterDto.Email,
                Email= seekerRegisterDto.Email
            };
            var result = await _seekerUserManager.CreateAsync(seeker, seekerRegisterDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            //await _seekerUserManager.AddToRoleAsync(seeker, RoleKey.customer.ToString());
            return Ok();
        }

        [HttpGet("AddRoleName")]
        [Authorize(Roles = "superMoster")]
        public async Task<IActionResult> AddRoleName([FromQuery] string role)
        {
            if (!Enum.IsDefined(typeof(RoleKey), role))
            {
                return NotFound("该身份不存在");
            }
            RoleExtension newRole = new RoleExtension()
            {
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpper()
            };
            var result = await _seekerRoleManager.CreateAsync(newRole);
            if (result.Succeeded!=true)
            {
                return BadRequest("创建角色失败");
            }
            return Ok("角色创建成功");
        }

        [HttpGet("AddRoleManager")]
       // [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "superMoster")]
        public async Task<IActionResult> AddRoleIdentity([FromQuery]string role, [FromQuery] string seekerId)
        {
            if (!Enum.IsDefined(typeof(RoleKey), role))
            {
                return NotFound("该身份不存在");
            }
            var seekerFromRepo = await _seekerUserManager.FindByIdAsync(seekerId);
            if (seekerFromRepo == null)
            {
                return NotFound("没有找到该用户");
            }
            if (await _seekerUserManager.IsInRoleAsync(seekerFromRepo, role))
            {
                return BadRequest($"该用户已经是{role}");
            }

            var result = await _seekerUserManager.AddToRoleAsync(seekerFromRepo, role);//这句好像没追踪
            if (!result.Succeeded)
            {
                return BadRequest("授权失败");
            }
            return Ok("授权成功");
        }


        //测试用
        [HttpGet]
        [Authorize(Roles = "superMoster")]
        public IActionResult TryController()
        {
            return Ok("是superMoster");
        }

    }

}
