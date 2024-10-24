﻿using DTOs;

namespace BlazorApp1.Services;

public interface IUserService
{
    Task<CreateUserDto> AddUserAsync(CreateUserDto createUserDto);
}