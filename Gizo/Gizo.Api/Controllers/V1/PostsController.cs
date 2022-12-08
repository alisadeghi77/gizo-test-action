using PostInteractionResponse = Gizo.Api.Contracts.Posts.Responses.PostInteractionResponse;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class PostsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllPosts(CancellationToken token)
    {
        var result = await _mediator.Send(new GetAllPostsQuery(), token);
        var mapped = _mapper.Map<List<PostResponse>>(result.Data);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

    }

    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    public async Task<IActionResult> GetById(long id, CancellationToken token)
    {
        var query = new GetPostByIdQuery() { PostId = id };
        var result = await _mediator.Send(query, token);
        var mapped = _mapper.Map<PostResponse>(result.Data);

        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest newPost, CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreatePostCommand()
        {
            UserProfileId = userProfileId,
            TextContent = newPost.TextContent
        };

        var result = await _mediator.Send(command, token);
        var mapped = _mapper.Map<PostResponse>(result.Data);

        return result.IsError ? HandleErrorResponse(result.Errors)
            : CreatedAtAction(nameof(GetById), new { id = result.Data.UserProfileId }, mapped);
    }

    [HttpPatch]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateModel]
    public async Task<IActionResult> UpdatePostText([FromBody] PostUpdateRequest updatedPost, long id, CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new UpdatePostTextCommand()
        {
            NewText = updatedPost.Text,
            PostId = id,
            UserProfileId = userProfileId
        };
        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    public async Task<IActionResult> DeletePost(long id, CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeletePostCommand() { PostId = id, UserProfileId = userProfileId };
        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostComments)]
    public async Task<IActionResult> GetCommentsByPostId(long postId, CancellationToken token)
    {
        var query = new GetPostCommentsQuery() { PostId = postId };
        var result = await _mediator.Send(query, token);

        if (result.IsError) HandleErrorResponse(result.Errors);

        var comments = _mapper.Map<List<PostCommentResponse>>(result.Data);
        return Ok(comments);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostComments)]
    [ValidateModel]
    public async Task<IActionResult> AddCommentToPost(long postId, [FromBody] PostCommentCreateRequest comment,
        CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new AddPostCommentCommand()
        {
            PostId = postId,
            UserProfileId = userProfileId,
            CommentText = comment.Text
        };

        var result = await _mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        var newComment = _mapper.Map<PostCommentResponse>(result.Data);

        return Ok(newComment);
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.CommentById)]
    public async Task<IActionResult> RemoveCommentFromPost(long postId, long commentId,
        CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new RemoveCommentFromPostCommand
        {
            UserProfileId = userProfileId,
            CommentId = commentId,
            PostId = postId
        };

        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpPut]
    [Route(ApiRoutes.Posts.CommentById)]
    [ValidateModel]
    public async Task<IActionResult> UpdateCommentText(long postId, long commentId,
        PostCommentUpdateRequest updatedComment, CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new UpdatePostCommentCommand
        {
            UserProfileId = userProfileId,
            PostId = postId,
            CommentId = commentId,
            UpdatedText = updatedComment.Text
        };

        var result = await _mediator.Send(command, token);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostInteractions)]
    public async Task<IActionResult> GetPostInteractions(long postId, CancellationToken token)
    {
        var query = new GetPostInteractionsQuery { PostId = postId };
        var result = await _mediator.Send(query, token);

        if (result.IsError) HandleErrorResponse(result.Errors);

        var mapped = _mapper.Map<List<PostInteractionResponse>>(result.Data);
        return Ok(mapped);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidateModel]
    public async Task<IActionResult> AddPostInteraction(long postId, PostInteractionCreateRequest interaction,
        CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new AddInteractionCommand
        {
            PostId = postId,
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
    public async Task<IActionResult> RemovePostInteraction(long postId, long interactionId,
        CancellationToken token)
    {
        var userProfileGuid = HttpContext.GetUserProfileIdClaimValue();
        var command = new RemovePostInteractionCommand
        {
            PostId = postId,
            InteractionId = interactionId,
            UserProfileId = userProfileGuid
        };

        var result = await _mediator.Send(command, token);
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var mapped = _mapper.Map<PostInteractionResponse>(result.Data);

        return Ok(mapped);
    }
}
