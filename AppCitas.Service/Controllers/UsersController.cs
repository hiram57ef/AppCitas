using AppCitas.Service.Data;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppCitas.Service.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
	private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;

    }
	[HttpGet]
	
	public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
	{
        var user = await _userRepository.GetMembersAsync();
        return Ok(user);
	}

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUsersByUsername(string username)
    {
        return  await _userRepository.GetMemberAsync(username);

    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var usernmae = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(usernmae);

        _mapper.Map(memberUpdateDto, user);

        _userRepository.Update(user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }
}
