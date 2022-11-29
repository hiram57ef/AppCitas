﻿using AppCitas.Service.Data;
using AppCitas.Service.DTOs;
using AppCitas.Service.Entities;
using AppCitas.Service.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        var user=await _userRepository.GetUsersAsync();

        var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);
        return Ok(usersToReturn);
	}

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUsersByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        return _mapper.Map<MemberDto>(user);
    }
}
