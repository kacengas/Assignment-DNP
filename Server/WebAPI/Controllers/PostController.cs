﻿using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    
    public PostController(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }
    
    // POST https://localhost:7276/Post
    [HttpPost]
    public async Task<IResult> CreatePost([FromBody]CreatePostDTO request)
    {
        Post post = new Post(request.Title, request.Content, request.UserId);
        
        await postRepository.AddAsync(post);
        return Results.Created($"posts/{post.Id}", post);
    }
    
    // GET https://localhost:7276/Post/{id}
    [HttpGet("{id:int}")]
    public async Task<IResult> GetSinglePost([FromRoute]int id)
    {
        var result = await postRepository.GetSingleAsync(id);
        if (result == null)
        {
            return Results.NotFound();
        }

        // var comments = commentRepository.GetMany().Where(c => c.PostId == id);
        // var postWithComments = new
        // {
        //     Post = result,
        //     Comments = comments
        // };

        return Results.Ok(result);
    }
    
    // DELETE https://localhost:7276/Post/{id}
    [HttpDelete("{id}")]
    public async Task<IResult> DeletePost([FromRoute]int id)
    {
        Post post = await postRepository.GetSingleAsync(id);
        if (post == null)
        {
            return Results.NotFound();
        }
        
        postRepository.DeleteAsync(post.Id);
        return Results.NoContent();
    }
    
    // PUT https://localhost:7276/Post/{id}
    [HttpPut("{id}")]
    public async Task<IResult> UpdatePost([FromRoute]int id, [FromBody]UpdatePostDTO request)
    {
        Post post = await postRepository.GetSingleAsync(id);
        if (post == null)
        {
            return Results.NotFound();
        }
        
        post.Title = request.Title;
        post.Content = request.Content;
        postRepository.UpdateAsync(post);
        return Results.NoContent();
    }
    
    //GET https://localhost:7276/Post
    [HttpGet]
    public IResult GetAllPosts([FromQuery] string? title, [FromQuery] int? userId, [FromQuery] string? userName)
    {
        var query = postRepository.GetMany();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var user = userRepository.GetSingleAsync(userId.Value).Result;
            if (user != null)
            {
                query = query.Where(p => p.UserId == user.Id);
            }
        }
        
        var posts = query.ToListAsync();
        return Results.Ok(posts);
    } 
}