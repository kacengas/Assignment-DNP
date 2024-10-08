﻿using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class PostListView
{
    private readonly IPostRepository postRepository;

    public PostListView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowPostList()
    {
        Console.Clear();
        Console.WriteLine("Showing list of posts");

        var posts = postRepository.GetMany();

        if (!posts.Any())
        {
            Console.WriteLine("No posts available.\n");
        }
        else
        {
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id} \n" +
                                  $"Title: {post.Title} \n" +
                                  $"Content: {post.Content} \n" +
                                  $"Posted by: {post.UserId} \n" +
                                  $"Time: {post.Date} \n"); 
            }
        }

        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();
    }
}