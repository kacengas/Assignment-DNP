﻿using DTOs;
using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int it);
    Task<User> GetSingleAsync(int id);
    IQueryable<User> GetMany();
    Task<User> GetByUsernameAsync(string username);
}