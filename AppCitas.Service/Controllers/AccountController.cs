using AppCitas.Service.Data;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Cryptography;
using System.Text;

namespace AppCitas.Service.Controllers;

public class AccountController : BaseApiController
{
	private readonly DataContext _context;
	private readonly ITokenServices _tokenService;

	public AccountController(DataContext context, ITokenServices tokenService)
	{
		_context = context;
		_tokenService = tokenService;
	}



	[HttpPost("register")]
	public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
	{

		if(await UserExist(registerDto.Username))
		{
			return BadRequest("El usuario ya está en uso");
		}

		using var hmac = new HMACSHA512();

		var user = new AppUser()
		{
			UserName = registerDto.Username.ToLower(),
			PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
			PasswordSalt = hmac.Key
		};
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return new UserDto
		{
			Username = user.UserName,
			Token = _tokenService.CreateToken(user)
		};
	}

	[HttpPost("Login")]

	public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
	{
		var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName == loginDto.Username);

		if (user == null) return Unauthorized("Invalid Username or password");

		using var hmac = new HMACSHA512(user.PasswordSalt);

		var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

		for( var i = 0; i < computeHash.Length; i++)
		{
			if(computeHash[i] != user.PasswordHash[i])
				return Unauthorized("Invalid Username or password");
        }

		return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    #region Private methods
    private async Task <bool> UserExist(string username)
	{
		return await _context.Users.AnyAsync(x => x.UserName.ToLower()== username.ToLower());
	}

    #endregion
}
