using BriefResume.Dtos;
using BriefResume.JwtModel;
using BriefResume.Models;
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
        private readonly UserManager<Seeker> _seekerUserManager;
        private readonly IOptionsSnapshot<JwtSettings> _jwtSettings;
        public AuthenticController( UserManager<Seeker> UserManager, IOptionsSnapshot<JwtSettings> optionsSnapshot)
        {
            _seekerUserManager = UserManager;
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
            var SeekerFromRepo = _seekerUserManager.FindByEmailAsync(seekerRegisterDto.Email);
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
            return Ok();
        }

        [HttpGet]
        public IActionResult TryController()
        {
            return Ok("试验成功");
        }

    }

}
