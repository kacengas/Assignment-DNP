﻿using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepositories : IUserRepository
{
    List<User> users = new();

    public UserInMemoryRepositories()
    {
        _ = AddAsync(new User("Adrian", "1234")).Result;
        _ = AddAsync(new User("Plamen", "4321")).Result;
        _ = AddAsync(new User("Mario", "1243")).Result;
        _ = AddAsync(new User("Vlad", "2143")).Result;
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }
        
        users.Remove(existingUser);
        users.Add(user);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found'");
        }
        
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToGet = users.SingleOrDefault(u => u.Id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
       
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}