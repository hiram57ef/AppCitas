using AppCitas.Service.Data;
using AppCitas.Service.Entities;
using AppCitas.Service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AppCitas.Service.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)

    {
        _userRepository = userRepository;
    }
	[HttpGet]
	
	public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
	{
        return Ok(await _userRepository.GetUsersAsync());
	}

    [HttpGet("{username}")]
    public async Task<ActionResult<AppUser>> GetUsersByUsername(string username)
    {

        return await _userRepository.GetUserByUsernameAsync(username);
    }
}
