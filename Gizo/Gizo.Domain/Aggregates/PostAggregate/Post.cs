using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Exceptions;
using Gizo.Domain.Validators.PostValidators;

namespace Gizo.Domain.Aggregates.PostAggregate;

public class Post: BaseEntity<long>
{
    private readonly List<PostComment> _comments = new List<PostComment>();
    private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
    private Post()
    {
    }
    public long UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string TextContent { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime LastModified { get; private set; }
    public IEnumerable<PostComment> Comments { get { return _comments; } }
    public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

    //Factories
    /// <summary>
    /// Creates a new post instance
    /// </summary>
    /// <param name="userProfileId">User profile ID</param>
    /// <param name="textContent">Post content</param>
    /// <returns><see cref="Post"/></returns>
    /// <exception cref="PostNotValidException"></exception>
    public static Post CreatePost(long userProfileId, string textContent)
    {
        var validator = new PostValidator();
        var objectToValidate = new Post
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new PostNotValidException("Post is not valid");
        validationResult.Errors.ForEach(vr => exception.ValidationErrors.Add(vr.ErrorMessage));
        throw exception;
    }

    //public methods
    /// <summary>
    /// Updates the post text
    /// </summary>
    /// <param name="newText">The updated post text</param>
    /// <exception cref="PostNotValidException"></exception>
    public void UpdatePostText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
        {
            var exception = new PostNotValidException("Cannot update post." +
                                                      "Post text is not valid");
            exception.ValidationErrors.Add("The provided text is either null or contains only white space");
            throw exception;
        }
        TextContent = newText;
        LastModified = DateTime.UtcNow;
    }
}
