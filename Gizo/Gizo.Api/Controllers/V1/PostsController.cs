﻿using PostInteractionResponse = Gizo.Api.Contracts.Posts.Responses.PostInteractionResponse;

namespace Gizo.Api.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllPosts(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllPostsQuery(), cancellationToken);
        var mapped = _mapper.Map<List<PostResponse>>(result.Data);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

    }

    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var postId = Guid.Parse(id);
        var query = new GetPostByIdQuery() { PostId = postId };
        var result = await _mediator.Send(query, cancellationToken);
        var mapped = _mapper.Map<PostResponse>(result.Data);

        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest newPost, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreatePostCommand()
        {
            UserProfileId = userProfileId,
            TextContent = newPost.TextContent
        };

        var result = await _mediator.Send(command, cancellationToken);
        var mapped = _mapper.Map<PostResponse>(result.Data);

        return result.IsError ? HandleErrorResponse(result.Errors)
            : CreatedAtAction(nameof(GetById), new { id = result.Data.UserProfileId }, mapped);
    }

    [HttpPatch]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePostText([FromBody] PostUpdateRequest updatedPost, string id, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new UpdatePostTextCommand()
        {
            NewText = updatedPost.Text,
            PostId = Guid.Parse(id),
            UserProfileId = userProfileId
        };
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeletePost(string id, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeletePostCommand() { PostId = Guid.Parse(id), UserProfileId = userProfileId };
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetCommentsByPostId(string postId, CancellationToken cancellationToken)
    {
        var query = new GetPostCommentsQuery() { PostId = Guid.Parse(postId) };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsError) HandleErrorResponse(result.Errors);

        var comments = _mapper.Map<List<PostCommentResponse>>(result.Data);
        return Ok(comments);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreateRequest comment,
        CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostCommentCommand()
        {
            PostId = Guid.Parse(postId),
            UserProfileId = userProfileId,
            CommentText = comment.Text
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var newComment = _mapper.Map<PostCommentResponse>(result.Data);

        return Ok(newComment);
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.CommentById)]
    [ValidateGuid("postId", "commentId")]
    public async Task<IActionResult> RemoveCommentFromPost(string postId, string commentId,
        CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var postGuid = Guid.Parse(postId);
        var commentGuid = Guid.Parse(commentId);
        var command = new RemoveCommentFromPostCommand
        {
            UserProfileId = userProfileId,
            CommentId = commentGuid,
            PostId = postGuid
        };

        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpPut]
    [Route(ApiRoutes.Posts.CommentById)]
    [ValidateGuid("postId", "commentId")]
    [ValidateModel]
    public async Task<IActionResult> UpdateCommentText(string postId, string commentId,
        PostCommentUpdateRequest updatedComment, CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var postGuid = Guid.Parse(postId);
        var commentGuid = Guid.Parse(commentId);

        var command = new UpdatePostCommentCommand
        {
            UserProfileId = userProfileId,
            PostId = postGuid,
            CommentId = commentGuid,
            UpdatedText = updatedComment.Text
        };

        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostInteractions(string postId, CancellationToken token)
    {
        var postGuid = Guid.Parse(postId);
        var query = new GetPostInteractionsQuery { PostId = postGuid };
        var result = await _mediator.Send(query, token);

        if (result.IsError) HandleErrorResponse(result.Errors);

        var mapped = _mapper.Map<List<PostInteractionResponse>>(result.Data);
        return Ok(mapped);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddPostInteraction(string postId, PostInteractionCreateRequest interaction,
        CancellationToken token)
    {
        var postGuid = Guid.Parse(postId);
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new AddInteractionCommand
        {
            PostId = postGuid,
            UserProfileId = userProfileId,
            Type = interaction.Type
        };

        var result = await _mediator.Send(command, token);

        if (result.IsError) HandleErrorResponse(result.Errors);

        var mapped = _mapper.Map<PostInteractionResponse>(result.Data);

        return Ok(mapped);
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.InteractionById)]
    [ValidateGuid("postId", "interactionId")]
    public async Task<IActionResult> RemovePostInteraction(string postId, string interactionId,
        CancellationToken token)
    {
        var postGuid = Guid.Parse(postId);
        var interactionGuid = Guid.Parse(interactionId);
        var userProfileGuid = HttpContext.GetUserProfileIdClaimValue();
        var command = new RemovePostInteractionCommand
        {
            PostId = postGuid,
            InteractionId = interactionGuid,
            UserProfileId = userProfileGuid
        };

        var result = await _mediator.Send(command, token);
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var mapped = _mapper.Map<PostInteractionResponse>(result.Data);

        return Ok(mapped);
    }
}
