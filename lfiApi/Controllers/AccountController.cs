using AutoMapper;
using lfiApi.DTOs;
using lfiApi.Entities;
using lfiApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace lfiApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new TokenDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
        {

            // Get the user from the database.
            // FirstOrDefaultAsync() method is used to get the first element of a sequence or a default value if the sequence contains no elements.
            // SingleOrDefaultAsync() method is used to get the only element of a sequence, or a default value if the sequence is empty; 
            // this method throws an exception if there is more than one element in the sequence.
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            // if user is not found, return Unauthorized.
            if (user == null) return Unauthorized("Invalid Username");

            // if user exists then check the password.
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid password");

            // if the user is found and password is correct, return the UserTokenDto.
            return new TokenDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        // [HttpPost("changePassword")]
        // public async Task<ActionResult<TokenDto>> ChangePassword(PasswordChangeDto pcd)
        // {
        //     // Get the user from the database.
        //     var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == pcd.Username);

        //     // if user is not found, return Unauthorized.
        //     if (user == null) return Unauthorized("Invalid Username");

        //     // if user exists then check the password.
        //     var result = await _userManager.CheckPasswordAsync(user, pcd.OldPassword);
        //     if (!result) return Unauthorized("Invalid password");

        //     user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pcd.NewPassword));

        //     // if the user is found and password is correct, return the UserTokenDto.
        //     _context.Entry(user).State = EntityState.Modified;

        //     if (await _context.SaveChangesAsync() > 0)
        //     {
        //         return new TokenDto
        //         {
        //             Username = user.UserName,
        //             Token = _tokenService.CreateToken(user)
        //         };
        //     }
        //     return BadRequest("Failed to change password!");
        // }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
