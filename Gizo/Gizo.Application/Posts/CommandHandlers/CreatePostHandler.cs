using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Posts.Commands;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Posts.CommandHandlers;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly IRepository<Post> _postRepository;
    private readonly IUnitOfWork _uow;

    public CreatePostHandler(IRepository<Post> postRepository, IUnitOfWork uow)
    {
        _postRepository = postRepository;
        _uow = uow;
    }

    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken token)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = Post.CreatePost(request.UserProfileId, request.TextContent);
            await _postRepository.InsertAsync(post);
            await _uow.SaveChangesAsync(token);

            result.Data = post;
        }
        catch (PostNotValidException e)
        {
            e.ValidationErrors.ForEach(er => result.AddError(ErrorCode.ValidationError, er));
        }

        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}