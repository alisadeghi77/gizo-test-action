﻿using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Queries;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Posts.QueryHandlers;

public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsQuery, OperationResult<List<PostComment>>>
{
    private readonly DataContext _ctx;

    public GetPostCommentsHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<PostComment>>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();
        try
        {
            var post = await _ctx.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            result.Data = post.Comments.ToList();
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}