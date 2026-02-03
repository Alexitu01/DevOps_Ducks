using ITUMiniTwit.Core.Models;

namespace ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;

public interface IAuthorRepository
{
    public Author GetAuthorByName(string authorName);
    public Author GetAuthorByEmail(string email);
    public void AddAuthor(string authorName, string email);

    public Task Follow(string followerUserName, string followeeUserName);
    public Task Unfollow(string followerUserName, string followeeUserName);

    public Task<List<Author>> GetFollowing(string userNameOrEmail);
    public Task<List<Author>> GetFollowers(string userNameOrEmail);
}