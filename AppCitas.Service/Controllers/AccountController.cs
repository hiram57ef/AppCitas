using AppCitas.Service.Data;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Cryptography;
using System.Text;

namespace AppCitas.Service.Controllers;

public class AccountController : BaseApiController
{
	private readonly DataContext _context;

	public AccountController(DataContext context)
	{
		_context = context;
	}

	[HttpPost("register")]
	public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
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

		return user;
	}

    #region Private methods
    private async Task <bool> UserExist(string username)
	{
		return await _context.Users.AnyAsync(x => x.UserName.ToLower()== username.ToLower());
	}

    #endregion
}
