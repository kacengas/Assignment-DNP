﻿namespace DTOs;

public class CreateCommentDTO
{
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}